// -----------------------------------------------------------------------
// <copyright file="Contact.cs" company="Nodine Legal, LLC">
// Licensed to Nodine Legal, LLC under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  Nodine Legal, LLC licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenLawOffice.Data.Contacts
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Contact
    {
        public static Common.Models.Contacts.Contact Get(int id)
        {
            return DataHelper.Get<Common.Models.Contacts.Contact, DBOs.Contacts.Contact>(
                "SELECT * FROM \"contact\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Contacts.Contact Get(string displayName)
        {
            return DataHelper.Get<Common.Models.Contacts.Contact, DBOs.Contacts.Contact>(
                "SELECT * FROM \"contact\" WHERE \"display_name\"=@displayName AND \"utc_disabled\" is null",
                new { displayName = displayName });
        }

        public static List<Common.Models.Matters.Matter> ListMattersForContact(int contactId)
        {
            List<DBOs.Matters.Matter> dbo = null;
            List<Common.Models.Matters.Matter> modelList = new List<Common.Models.Matters.Matter>();

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                dbo = conn.Query<DBOs.Matters.Matter>
                    ("SELECT \"matter\".* FROM \"task_assigned_contact\" " +
                    "JOIN \"task_matter\" ON \"task_assigned_contact\".\"task_id\"=\"task_matter\".\"task_id\" " +
                    "JOIN \"matter\" ON \"task_matter\".\"matter_id\"=\"matter\".\"id\" " +
                    "WHERE \"task_assigned_contact\".\"contact_id\"=@ContactId " +
                    "UNION " +
                    "SELECT \"matter\".* FROM \"matter_contact\" " +
                    "JOIN \"matter\" ON \"matter_contact\".\"matter_id\"=\"matter\".\"id\" " +
                    "WHERE \"matter_contact\".\"contact_id\"=@ContactId",
                    new { ContactId = contactId }).ToList();
            }

            dbo.ForEach(x =>
            {
                modelList.Add(Mapper.Map<Common.Models.Matters.Matter>(x));
            });

            return modelList;
        }

        public static List<Tuple<Common.Models.Matters.Matter, Common.Models.Matters.MatterContact, Common.Models.Contacts.Contact>>
            ListMatterRelationshipsForContact(int contactId, Guid matterId)
        {
            List<Tuple<DBOs.Matters.Matter, DBOs.Matters.MatterContact, DBOs.Contacts.Contact>> dbo = null;
            List<Tuple<Common.Models.Matters.Matter, Common.Models.Matters.MatterContact, Common.Models.Contacts.Contact>> modelList =
                new List<Tuple<Common.Models.Matters.Matter, Common.Models.Matters.MatterContact, Common.Models.Contacts.Contact>>();

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Open();
                dbo = conn.Query<DBOs.Matters.Matter, DBOs.Matters.MatterContact, DBOs.Contacts.Contact,
                    Tuple<DBOs.Matters.Matter, DBOs.Matters.MatterContact, DBOs.Contacts.Contact>>
                    ("SELECT * FROM \"matter\" " +
                    "JOIN \"matter_contact\" ON \"matter\".\"id\"=\"matter_contact\".\"matter_id\" " +
                    "JOIN \"contact\" ON \"matter_contact\".\"contact_id\"=\"contact\".\"id\" " +
                    "WHERE \"matter\".\"id\"=@MatterId " +
                    "AND \"contact\".\"id\"!=@ContactId",
                    (mtr, matterContact, contact) =>
                    {
                        return new Tuple<DBOs.Matters.Matter, DBOs.Matters.MatterContact, DBOs.Contacts.Contact>(mtr, matterContact, contact);
                    },
                    new { MatterId = matterId, ContactId = contactId }).ToList();
            }

            dbo.ForEach(x =>
            {
                Common.Models.Matters.Matter m = Mapper.Map<Common.Models.Matters.Matter>(x.Item1);
                Common.Models.Matters.MatterContact mc = Mapper.Map<Common.Models.Matters.MatterContact>(x.Item2);
                Common.Models.Contacts.Contact c = Mapper.Map<Common.Models.Contacts.Contact>(x.Item3);
                modelList.Add(new Tuple<Common.Models.Matters.Matter, Common.Models.Matters.MatterContact, Common.Models.Contacts.Contact>(m, mc, c));
            });

            return modelList;
        }

        public static List<Common.Models.Contacts.Contact> List()
        {
            return DataHelper.List<Common.Models.Contacts.Contact, DBOs.Contacts.Contact>(
                "SELECT * FROM \"contact\" WHERE \"utc_disabled\" is null ORDER BY \"display_name\" ASC");
        }

        public static List<Common.Models.Contacts.Contact> List(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName))
                displayName = displayName.ToLower();
            return DataHelper.List<Common.Models.Contacts.Contact, DBOs.Contacts.Contact>(
                "SELECT * FROM \"contact\" WHERE \"utc_disabled\" is null AND " +
                "LOWER(\"display_name\") LIKE '%' || @DisplayName || '%' ORDER BY \"display_name\" ASC",
                new { DisplayName = displayName });
        }

        public static List<Common.Models.Contacts.Contact> ListEmployeesOnly()
        {
            return DataHelper.List<Common.Models.Contacts.Contact, DBOs.Contacts.Contact>(
                "SELECT * FROM \"contact\" WHERE \"is_our_employee\"=true AND \"utc_disabled\" is null ORDER BY \"display_name\" ASC");
        }

        public static Common.Models.Contacts.Contact Create(Common.Models.Contacts.Contact model,
            Common.Models.Account.Users creator)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;

            DBOs.Contacts.Contact dbo = Mapper.Map<DBOs.Contacts.Contact>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                model.Id = dbo.Id = conn.Execute("INSERT INTO \"contact\" (" +
                    "\"referred_by_name\", \"gender\", \"business_home_page\", \"personal_home_page\", " +
                    "\"instant_messaging_address\", \"language\", \"spouse_name\", " +
                    "\"profession\", \"assistant_name\", \"manager_name\", \"office_location\", " +
                    "\"department_name\", \"company_name\", \"title\", \"wedding\", " +
                    "\"birthday\", \"telephone10_telephone_number\", \"telephone10_display_name\", " +
                    "\"telephone9_telephone_number\", \"telephone9_display_name\", \"telephone8_telephone_number\", " +
                    "\"telephone8_display_name\", \"telephone7_telephone_number\", \"telephone7_display_name\", " +
                    "\"telephone6_telephone_number\", \"telephone6_display_name\", \"telephone5_telephone_number\", " +
                    "\"telephone5_display_name\", \"telephone4_telephone_number\", \"telephone4_display_name\", " +
                    "\"telephone3_telephone_number\", \"telephone3_display_name\", \"telephone2_telephone_number\", " +
                    "\"telephone2_display_name\", \"telephone1_telephone_number\", \"telephone1_display_name\", " +
                    "\"address3_address_post_office_box\", \"address3_address_country_code\", \"address3_address_country\", " +
                    "\"address3_address_postal_code\", \"address3_address_state_or_province\", \"address3_address_city\", " +
                    "\"address3_address_street\", \"address3_display_name\", \"address2_address_post_office_box\", " +
                    "\"address2_address_country_code\", \"address2_address_country\", \"address2_address_postal_code\", " +
                    "\"address2_address_state_or_province\", \"address2_address_city\", \"address2_address_street\", " +
                    "\"address2_display_name\", \"address1_address_post_office_box\", \"address1_address_country_code\", " +
                    "\"address1_address_country\", \"address1_address_postal_code\", \"address1_address_state_or_province\", " +
                    "\"address1_address_city\", \"address1_address_street\", \"address1_display_name\", " +
                    "\"fax3_fax_number\", \"fax3_display_name\", \"fax2_fax_number\", " +
                    "\"fax2_display_name\", \"fax1_fax_number\", \"fax1_display_name\", " +
                    "\"email3_email_address\", \"email3_display_name\", \"email2_email_address\", " +
                    "\"email2_display_name\", \"email1_email_address\", \"email1_display_name\", " +
                    "\"display_name\", \"initials\", \"given_name\", \"middle_name\", \"surname\", " +
                    "\"display_name_prefix\", \"generation\", \"nickname\", \"is_organization\", \"is_our_employee\", " +
                    "\"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") VALUES (" +
                    "@ReferredByName, @Gender, @BusinessHomePage, " +
                    "@PersonalHomePage, @InstantMessagingAddress, @Language, @SpouseName, @Profession, " +
                    "@AssistantName, @ManagerName, @OfficeLocation, @DepartmentName, @CompanyName, " +
                    "@Title, @Wedding, @Birthday, @Telephone10TelephoneNumber, @Telephone10DisplayName, " +
                    "@Telephone9TelephoneNumber, @Telephone9DisplayName, @Telephone8TelephoneNumber, " +
                    "@Telephone8DisplayName, @Telephone7TelephoneNumber, @Telephone7DisplayName, " +
                    "@Telephone6TelephoneNumber, @Telephone6DisplayName, @Telephone5TelephoneNumber, " +
                    "@Telephone5DisplayName, @Telephone4TelephoneNumber, @Telephone4DisplayName, " +
                    "@Telephone3TelephoneNumber, @Telephone3DisplayName, @Telephone2TelephoneNumber, " +
                    "@Telephone2DisplayName, @Telephone1TelephoneNumber, @Telephone1DisplayName, " +
                    "@Address3AddressPostOfficeBox, @Address3AddressCountryCode, @Address3AddressCountry, " +
                    "@Address3AddressPostalCode, @Address3AddressStateOrProvince, @Address3AddressCity, " +
                    "@Address3AddressStreet, @Address3DisplayName, @Address2AddressPostOfficeBox, " +
                    "@Address2AddressCountryCode, @Address2AddressCountry, @Address2AddressPostalCode, " +
                    "@Address2AddressStateOrProvince, @Address2AddressCity, @Address2AddressStreet, " +
                    "@Address2DisplayName, @Address1AddressPostOfficeBox, @Address1AddressCountryCode, " +
                    "@Address1AddressCountry, @Address1AddressPostalCode, @Address1AddressStateOrProvince, " +
                    "@Address1AddressCity, @Address1AddressStreet, @Address1DisplayName, " +
                    "@Fax3FaxNumber, @Fax3DisplayName, @Fax2FaxNumber, @Fax2DisplayName, @Fax1FaxNumber, " +
                    "@Fax1DisplayName, @Email3EmailAddress, @Email3DisplayName, @Email2EmailAddress, @Email2DisplayName, " +
                    "@Email1EmailAddress, @Email1DisplayName, @DisplayName, @Initials, @GivenName, " +
                    "@MiddleName, @Surname, @DisplayNamePrefix, @Generation, @Nickname, @IsOrganization, @IsOurEmployee, " +
                    "@UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)", dbo);

                model.Id = conn.Query<DBOs.Contacts.Contact>("SELECT currval(pg_get_serial_sequence('contact', 'id')) AS \"id\"").Single().Id;
            }

            return model;
        }

        public static Common.Models.Contacts.Contact Edit(Common.Models.Contacts.Contact model,
            Common.Models.Account.Users modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Contacts.Contact dbo = Mapper.Map<DBOs.Contacts.Contact>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"contact\" SET \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId, " +
                    "\"referred_by_name\"=@ReferredByName, \"gender\"=@Gender, \"business_home_page\"=@BusinessHomePage, " +
                    "\"personal_home_page\"=@PersonalHomePage, \"instant_messaging_address\"=@InstantMessagingAddress, " +
                    "\"language\"=@Language, \"spouse_name\"=@SpouseName, \"profession\"=@Profession, \"assistant_name\"=@AssistantName, " +
                    "\"manager_name\"=@ManagerName, \"office_location\"=@OfficeLocation, " +
                    "\"department_name\"=@DepartmentName, \"company_name\"=@CompanyName, \"title\"=@Title, \"wedding\"=@Wedding, " +
                    "\"birthday\"=@Birthday, \"telephone10_telephone_number\"=@Telephone10TelephoneNumber, \"telephone10_display_name\"=@Telephone10DisplayName, " +
                    "\"telephone9_telephone_number\"=@Telephone9TelephoneNumber, \"telephone9_display_name\"=@Telephone9DisplayName, \"telephone8_telephone_number\"=@Telephone8TelephoneNumber, " +
                    "\"telephone8_display_name\"=@Telephone8DisplayName, \"telephone7_telephone_number\"=@Telephone7TelephoneNumber, \"telephone7_display_name\"=@Telephone7DisplayName, " +
                    "\"telephone6_telephone_number\"=@Telephone6TelephoneNumber, \"telephone6_display_name\"=@Telephone6DisplayName, \"telephone5_telephone_number\"=@Telephone5TelephoneNumber, " +
                    "\"telephone5_display_name\"=@Telephone5DisplayName, \"telephone4_telephone_number\"=@Telephone4TelephoneNumber, \"telephone4_display_name\"=@Telephone4DisplayName, " +
                    "\"telephone3_telephone_number\"=@Telephone3TelephoneNumber, \"telephone3_display_name\"=@Telephone3DisplayName, \"telephone2_telephone_number\"=@Telephone2TelephoneNumber, " +
                    "\"telephone2_display_name\"=@Telephone2DisplayName, \"telephone1_telephone_number\"=@Telephone1TelephoneNumber, \"telephone1_display_name\"=@Telephone1DisplayName, " +
                    "\"address3_address_post_office_box\"=@Address3AddressPostOfficeBox, \"address3_address_country_code\"=@Address3AddressCountryCode, \"address3_address_country\"=@Address3AddressCountry, " +
                    "\"address3_address_postal_code\"=@Address3AddressPostalCode, \"address3_address_state_or_province\"=@Address3AddressStateOrProvince, \"address3_address_city\"=@Address3AddressCity, " +
                    "\"address3_address_street\"=@Address3AddressStreet, \"address3_display_name\"=@Address3DisplayName, \"address2_address_post_office_box\"=@Address2AddressPostOfficeBox, " +
                    "\"address2_address_country_code\"=@Address2AddressCountryCode, \"address2_address_country\"=@Address2AddressCountry, \"address2_address_postal_code\"=@Address2AddressPostalCode, " +
                    "\"address2_address_state_or_province\"=@Address2AddressStateOrProvince, \"address2_address_city\"=@Address2AddressCity, \"address2_address_street\"=@Address2AddressStreet, " +
                    "\"address2_display_name\"=@Address2DisplayName, \"address1_address_post_office_box\"=@Address1AddressPostOfficeBox, \"address1_address_country_code\"=@Address1AddressCountryCode, " +
                    "\"address1_address_country\"=@Address1AddressCountry, \"address1_address_postal_code\"=@Address1AddressPostalCode, \"address1_address_state_or_province\"=@Address1AddressStateOrProvince, " +
                    "\"address1_address_city\"=@Address1AddressCity, \"address1_address_street\"=@Address1AddressStreet, \"address1_display_name\"=@Address1DisplayName, " +
                    "\"fax3_fax_number\"=@Fax3FaxNumber, \"fax3_display_name\"=@Fax3DisplayName, \"fax2_fax_number\"=@Fax2FaxNumber, " +
                    "\"fax2_display_name\"=@Fax2DisplayName, \"fax1_fax_number\"=@Fax1FaxNumber, \"fax1_display_name\"=@Fax1DisplayName, " +
                    "\"email3_email_address\"=@Email3EmailAddress, \"email3_display_name\"=@Email3DisplayName, \"email2_email_address\"=@Email2EmailAddress, " +
                    "\"email2_display_name\"=@Email2DisplayName, \"email1_email_address\"=@Email1EmailAddress, \"email1_display_name\"=@Email1DisplayName, " +
                    "\"display_name\"=@DisplayName, \"initials\"=@Initials, \"given_name\"=@GivenName, \"middle_name\"=@MiddleName, \"surname\"=@Surname, " +
                    "\"display_name_prefix\"=@DisplayNamePrefix, \"generation\"=@Generation, \"nickname\"=@Nickname, \"is_organization\"=@IsOrganization, \"is_our_employee\"=@IsOurEmployee " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }
    }
}