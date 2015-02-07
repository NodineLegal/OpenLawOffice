// -----------------------------------------------------------------------
// <copyright file="Matter.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Matters
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Matter
    {
        public static Common.Models.Matters.Matter Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"matter\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static List<Common.Models.Matters.Matter> List(bool? active)
        {
            if (!active.HasValue)
                return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                    "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null");
            else
                return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                    "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null AND \"active\"=@Active",
                    new { Active = active.Value });
        }

        public static List<Common.Models.Matters.Matter> List(bool? active, string contactFilter)
        {
            if (!active.HasValue)
            {
                if (!string.IsNullOrEmpty(contactFilter))
                    return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                        "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null AND " +
                        "\"id\" IN (SELECT \"matter_id\" FROM \"matter_contact\" WHERE " +
                        "\"contact_id\" IN (SELECT \"id\" FROM \"contact\" WHERE LOWER(\"display_name\") LIKE '%' || @ContactFilter || '%'))",
                        new { ContactFilter = contactFilter.ToLower() });
                else
                    return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                        "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null");
            }
            else
            {
                if (!string.IsNullOrEmpty(contactFilter))
                    return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                        "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null AND \"active\"= @Active AND " +
                        "\"id\" IN (SELECT \"matter_id\" FROM \"matter_contact\" WHERE " +
                        "\"contact_id\" IN (SELECT \"id\" FROM \"contact\" WHERE LOWER(\"display_name\") LIKE '%' || @ContactFilter || '%'))",
                        new { ContactFilter = contactFilter.ToLower(), Active = active.Value });
                else
                    return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                        "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null AND \"active\"=@Active",
                        new { Active = active.Value });
            }
        }

        public static List<Common.Models.Matters.Matter> List(bool? active, string contactFilter, string titleFilter, string caseNumberFilter, string jurisdictionFilter)
        {
            string sql = "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null ";

            if (contactFilter != null)
                contactFilter = contactFilter.ToLower();
            if (titleFilter != null)
                titleFilter = titleFilter.ToLower();
            if (caseNumberFilter != null)
                caseNumberFilter = caseNumberFilter.ToLower();
            if (jurisdictionFilter != null)
                jurisdictionFilter = jurisdictionFilter.ToLower();

            if (active.HasValue)
            {
                sql += " AND \"active\"=@Active ";
            }
            if (!string.IsNullOrEmpty(contactFilter))
            {
                sql += " AND \"id\" IN (SELECT \"matter_id\" FROM \"matter_contact\" WHERE " +
                    "\"contact_id\" IN (SELECT \"id\" FROM \"contact\" WHERE LOWER(\"display_name\") LIKE '%' || @ContactFilter || '%'))";
            }
            if (!string.IsNullOrEmpty(titleFilter))
            {
                sql += " AND LOWER(\"title\") LIKE '%' || @TitleFilter || '%' ";
            }
            if (!string.IsNullOrEmpty(caseNumberFilter))
            {
                sql += " AND LOWER(\"case_number\") LIKE '%' || @CaseNumberFilter || '%' ";
            }
            if (!string.IsNullOrEmpty(jurisdictionFilter))
            {
                sql += " AND LOWER(\"jurisdiction\") LIKE '%' || @JurisdictionFilter || '%' ";
            }

            if (active.HasValue)
                return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(sql,
                    new
                    {
                        Active = active.Value,
                        ContactFilter = contactFilter,
                        TitleFilter = titleFilter,
                        CaseNumberFilter = caseNumberFilter,
                        JurisdictionFilter = jurisdictionFilter
                    });
            else
                return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(sql,
                    new
                    {
                        ContactFilter = contactFilter,
                        TitleFilter = titleFilter,
                        CaseNumberFilter = caseNumberFilter,
                        JurisdictionFilter = jurisdictionFilter
                    });
        }

        public static List<Common.Models.Matters.Matter> ListTitlesOnly(string title)
        {
            if (!string.IsNullOrEmpty(title))
                title = title.ToLower();
            return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT DISTINCT \"title\" FROM \"matter\" WHERE \"utc_disabled\" is null AND LOWER(\"title\") LIKE '%' || @Title || '%'",
                new { Title = title });
        }

        public static List<Common.Models.Matters.Matter> ListCaseNumbersOnly(string caseNumber)
        {
            if (!string.IsNullOrEmpty(caseNumber))
                caseNumber = caseNumber.ToLower();
            return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT DISTINCT \"case_number\" FROM \"matter\" WHERE \"utc_disabled\" is null AND LOWER(\"case_number\") LIKE '%' || @CaseNumber || '%'",
                new { CaseNumber = caseNumber });
        }

        public static List<Common.Models.Matters.Matter> ListJurisdictionsOnly(string jurisdiction)
        {
            if (!string.IsNullOrEmpty(jurisdiction))
                jurisdiction = jurisdiction.ToLower();
            return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT DISTINCT \"jurisdiction\" FROM \"matter\" WHERE \"utc_disabled\" is null AND LOWER(\"jurisdiction\") LIKE '%' || @Jurisdiction || '%'",
                new { Jurisdiction = jurisdiction });
        }

        public static List<Common.Models.Matters.Matter> ListChildren(Guid? parentId)
        {
            List<Common.Models.Matters.Matter> list = new List<Common.Models.Matters.Matter>();
            IEnumerable<DBOs.Matters.Matter> ie = null;
            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                if (parentId.HasValue)
                    ie = conn.Query<DBOs.Matters.Matter>(
                        "SELECT \"matter\".* FROM \"matter\" " +
                        "WHERE \"matter\".\"utc_disabled\" is null  " +
                        "AND \"matter\".\"parent_id\"=@ParentId", new { ParentId = parentId.Value });
                else
                    ie = conn.Query<DBOs.Matters.Matter>(
                        "SELECT \"matter\".* FROM \"matter\" " +
                        "WHERE \"matter\".\"utc_disabled\" is null  " +
                        "AND \"matter\".\"parent_id\" is null");
            }

            foreach (DBOs.Matters.Matter dbo in ie)
                list.Add(Mapper.Map<Common.Models.Matters.Matter>(dbo));

            return list;
        }

        public static List<Common.Models.Matters.Matter> ListChildrenForContact(Guid? parentId, int contactId)
        {
            List<Common.Models.Matters.Matter> list = new List<Common.Models.Matters.Matter>();
            IEnumerable<DBOs.Matters.Matter> ie = null;
            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                if (parentId.HasValue)
                    ie = conn.Query<DBOs.Matters.Matter>(
                        "SELECT \"matter\".* FROM \"matter\" " +
                        "WHERE \"matter\".\"utc_disabled\" is null  " +
                        "AND \"matter\".\"parent_id\"=@ParentId " + 
                        "AND \"matter\".\"id\" IN (SELECT \"matter_id\" FROM \"matter_contact\" WHERE \"contact_id\"=@ContactId)", 
                        new { ParentId = parentId.Value, ContactId = contactId });
                else
                    ie = conn.Query<DBOs.Matters.Matter>(
                        "SELECT \"matter\".* FROM \"matter\" " +
                        "WHERE \"matter\".\"utc_disabled\" is null  " +
                        "AND \"matter\".\"parent_id\" is null " +
                        "AND \"matter\".\"id\" IN (SELECT \"matter_id\" FROM \"matter_contact\" WHERE \"contact_id\"=@ContactId)",
                        new { ContactId = contactId });
            }

            foreach (DBOs.Matters.Matter dbo in ie)
                list.Add(Mapper.Map<Common.Models.Matters.Matter>(dbo));

            return list;
        }

        public static List<Common.Models.Matters.Matter> ListAllMattersForContact(int contactId)
        {
            List<Common.Models.Matters.Matter> list = new List<Common.Models.Matters.Matter>();
            IEnumerable<DBOs.Matters.Matter> ie = null;
            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                ie = conn.Query<DBOs.Matters.Matter>(
                    "SELECT \"matter\".* FROM \"matter\" WHERE \"matter\".\"utc_disabled\" is null  " +
                    "AND \"matter\".\"id\" IN (SELECT \"matter_id\" FROM \"matter_contact\" WHERE \"contact_id\"=@ContactId)",
                    new { ContactId = contactId });
            }

            foreach (DBOs.Matters.Matter dbo in ie)
                list.Add(Mapper.Map<Common.Models.Matters.Matter>(dbo));

            return list;
        }

        public static Common.Models.Matters.Matter Create(Common.Models.Matters.Matter model,
            Common.Models.Account.Users creator)
        {
            // Matter
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Matters.Matter dbo = Mapper.Map<DBOs.Matters.Matter>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"matter\" (\"id\", \"title\", \"active\", \"parent_id\", \"synopsis\", " +
                    "\"minimum_charge\", \"estimated_charge\", \"maximum_charge\", " +
                    "\"utc_created\", \"utc_modified\", " +
                    "\"created_by_user_pid\", \"modified_by_user_pid\", \"jurisdiction\", \"case_number\", \"lead_attorney_contact_id\", \"bill_to_contact_id\") " +
                    "VALUES (@Id, @Title, @Active, @ParentId, @Synopsis, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId, " +
                    "@Jurisdiction, @CaseNumber, @LeadAttorneyContactId, @BillToContactId)",
                    dbo);
            }

            MatterContact.Create(new Common.Models.Matters.MatterContact()
            {
                Matter = model,
                Contact = new Common.Models.Contacts.Contact() { Id = dbo.LeadAttorneyContactId.Value },
                Role = "Lead Attorney"
            }, creator);

            return model;
        }

        public static Common.Models.Matters.Matter Edit(Common.Models.Matters.Matter model,
            Common.Models.Account.Users modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            List<Common.Models.Matters.MatterContact> leadAttorneyMatches;
            DBOs.Matters.Matter dbo = Mapper.Map<DBOs.Matters.Matter>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"matter\" SET " +
                    "\"title\"=@Title, \"active\"=@Active, \"parent_id\"=@ParentId, \"synopsis\"=@Synopsis, \"utc_modified\"=@UtcModified, " +
                    "\"minimum_charge\"=@MinimumCharge, \"estimated_charge\"=@EstimatedCharge, \"maximum_charge\"=@MaximumCharge, " +
                    "\"modified_by_user_pid\"=@ModifiedByUserPId, \"jurisdiction\"=@Jurisdiction, \"case_number\"=@CaseNumber, \"lead_attorney_contact_id\"=@LeadAttorneyContactId, \"bill_to_contact_id\"=@BillToContactId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            leadAttorneyMatches = MatterContact.ListForMatterByRole(dbo.Id, "Lead Attorney");

            if (leadAttorneyMatches.Count > 1)
                throw new Exception("More than one Lead Attorney found.");
            else if (leadAttorneyMatches.Count < 1)
            {   // Insert only
                MatterContact.Create(new Common.Models.Matters.MatterContact()
                {
                    Matter = model,
                    Contact = new Common.Models.Contacts.Contact() { Id = dbo.LeadAttorneyContactId.Value },
                    Role = "Lead Attorney"
                }, modifier);
            }
            else
            {   // Replace
                leadAttorneyMatches[0].Contact.Id = dbo.LeadAttorneyContactId.Value;
                MatterContact.Edit(leadAttorneyMatches[0], modifier);
            }

            return model;
        }
    }
}