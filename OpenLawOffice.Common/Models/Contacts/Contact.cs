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

namespace OpenLawOffice.Common.Models.Contacts
{
    using System;
    using AutoMapper;

    /// <summary>
    /// Represents a system contact, loosely based on MS-OXONTC's message syntax for contact object properties
    /// http://msdn.microsoft.com/en-us/library/ee179200(v=exchg.80).aspx
    /// </summary>
    [MapMe]
    [Can(CanFlags.Get | CanFlags.Create | CanFlags.Update | CanFlags.Delete)]
    public class Contact : Core, IHasIntId
    {
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the contact is an organization.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the contact is an organization; otherwise, <c>false</c>.
        /// </value>
        /// <author>Lucas Nodine</author>
        public bool IsOrganization { get; set; }

        #region Contact Name

        /// <summary>
        /// Gets or sets the nickname of the contact.
        /// </summary>
        /// <value>
        /// The nickname.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets the generation suffix of the contact, such as "Jr.", "Sr.", or "III".
        /// </summary>
        /// <value>
        /// The generation.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Generation { get; set; }

        /// <summary>
        /// Gets or sets the title of the contact, such as "Mr." or "Mrs.".
        /// </summary>
        /// <value>
        /// The display name prefix.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string DisplayNamePrefix { get; set; }

        /// <summary>
        /// Gets or sets the surname (family name) of the contact.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the middle name(s) of the contact.
        /// </summary>
        /// <value>
        /// The middle name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the given name (first name) of the contact.
        /// </summary>
        /// <value>
        /// The given name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the initials of the contact.
        /// </summary>
        /// <value>
        /// The initials.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Initials { get; set; }

        /// <summary>
        /// Gets or sets the full name of the contact.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string DisplayName { get; set; }

        #endregion

        #region Electronic Address Properties

        /// <summary>
        /// Gets or sets the user-readable display name for the e-mail address of the contact such as "Home", "Work", "Phone", etc.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Email1DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Email1EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the user-readable display name for the e-mail address of the contact such as "Home", "Work", "Phone", etc.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Email2DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Email2EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the user-readable display name for the e-mail address of the contact such as "Home", "Work", "Phone", etc.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Email3DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Email3EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the user-readable display name for the e-mail address of the contact such as "Home", "Work", "Phone", etc..
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Fax1DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the fax number for the contact.
        /// </summary>
        /// <value>
        /// The fax number.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Fax1FaxNumber { get; set; }

        /// <summary>
        /// Gets or sets the user-readable display name for the e-mail address of the contact such as "Home", "Work", "Phone", etc..
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Fax2DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the fax number for the contact.
        /// </summary>
        /// <value>
        /// The fax number.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Fax2FaxNumber { get; set; }

        /// <summary>
        /// Gets or sets the user-readable display name for the e-mail address of the contact such as "Home", "Work", "Phone", etc..
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Fax3DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the fax number for the contact.
        /// </summary>
        /// <value>
        /// The fax number.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Fax3FaxNumber { get; set; }

        #endregion

        #region Physical Address Properties

        /// <summary>
        /// Gets or sets the user-readable display name for the address of the contact such as "Home", "Work", etc.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address1DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the street portion of the address.
        /// </summary>
        /// <value>
        /// The street.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address1AddressStreet { get; set; }

        /// <summary>
        /// Gets or sets the city portion of the address.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address1AddressCity { get; set; }

        /// <summary>
        /// Gets or sets the state or province portion of the address.
        /// </summary>
        /// <value>
        /// The state or province.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address1AddressStateOrProvince { get; set; }

        /// <summary>
        /// Gets or sets the postal code portion of the address.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address1AddressPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the country portion of the address.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address1AddressCountry { get; set; }

        /// <summary>
        /// Gets or sets the country/region code portion of the address.
        /// </summary>
        /// <value>
        /// The country/region code.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address1AddressCountryCode { get; set; }

        /// <summary>
        /// Gets or sets the post office box portion of the address.
        /// </summary>
        /// <value>
        /// The post office box.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address1AddressPostOfficeBox { get; set; }

        /// <summary>
        /// Gets or sets the user-readable display name for the address of the contact such as "Home", "Work", etc.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address2DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the street portion of the address.
        /// </summary>
        /// <value>
        /// The street.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address2AddressStreet { get; set; }

        /// <summary>
        /// Gets or sets the city portion of the address.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address2AddressCity { get; set; }

        /// <summary>
        /// Gets or sets the state or province portion of the address.
        /// </summary>
        /// <value>
        /// The state or province.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address2AddressStateOrProvince { get; set; }

        /// <summary>
        /// Gets or sets the postal code portion of the address.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address2AddressPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the country portion of the address.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address2AddressCountry { get; set; }

        /// <summary>
        /// Gets or sets the country/region code portion of the address.
        /// </summary>
        /// <value>
        /// The country/region code.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address2AddressCountryCode { get; set; }

        /// <summary>
        /// Gets or sets the post office box portion of the address.
        /// </summary>
        /// <value>
        /// The post office box.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address2AddressPostOfficeBox { get; set; }

        /// <summary>
        /// Gets or sets the user-readable display name for the address of the contact such as "Home", "Work", etc.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address3DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the street portion of the address.
        /// </summary>
        /// <value>
        /// The street.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address3AddressStreet { get; set; }

        /// <summary>
        /// Gets or sets the city portion of the address.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address3AddressCity { get; set; }

        /// <summary>
        /// Gets or sets the state or province portion of the address.
        /// </summary>
        /// <value>
        /// The state or province.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address3AddressStateOrProvince { get; set; }

        /// <summary>
        /// Gets or sets the postal code portion of the address.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address3AddressPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the country portion of the address.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address3AddressCountry { get; set; }

        /// <summary>
        /// Gets or sets the country/region code portion of the address.
        /// </summary>
        /// <value>
        /// The country/region code.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address3AddressCountryCode { get; set; }

        /// <summary>
        /// Gets or sets the post office box portion of the address.
        /// </summary>
        /// <value>
        /// The post office box.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Address3AddressPostOfficeBox { get; set; }

        #endregion

        #region Telephone Properties

        /// <summary>
        /// Gets or sets the user-readable display name for the telephone number of the contact such as "Home", "Work", "Mobile", "Assistant", etc.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone1DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone1TelephoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user-readable display name for the telephone number of the contact such as "Home", "Work", "Mobile", "Assistant", etc.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone2DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone2TelephoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user-readable display name for the telephone number of the contact such as "Home", "Work", "Mobile", "Assistant", etc.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone3DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone3TelephoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user-readable display name for the telephone number of the contact such as "Home", "Work", "Mobile", "Assistant", etc.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone4DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone4TelephoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user-readable display name for the telephone number of the contact such as "Home", "Work", "Mobile", "Assistant", etc.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone5DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone5TelephoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user-readable display name for the telephone number of the contact such as "Home", "Work", "Mobile", "Assistant", etc.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone6DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone6TelephoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user-readable display name for the telephone number of the contact such as "Home", "Work", "Mobile", "Assistant", etc.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone7DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone7TelephoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user-readable display name for the telephone number of the contact such as "Home", "Work", "Mobile", "Assistant", etc.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone8DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone8TelephoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user-readable display name for the telephone number of the contact such as "Home", "Work", "Mobile", "Assistant", etc.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone9DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone9TelephoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user-readable display name for the telephone number of the contact such as "Home", "Work", "Mobile", "Assistant", etc.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone10DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Telephone10TelephoneNumber { get; set; }

        #endregion

        #region Event Properties

        /// <summary>
        /// Gets or sets the birthday of the contact.
        /// </summary>
        /// <value>
        /// The birthday.
        /// </value>
        /// <author>Lucas Nodine</author>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Gets or sets the wedding anniversary of the contact.
        /// </summary>
        /// <value>
        /// The wedding anniversary.
        /// </value>
        /// <author>Lucas Nodine</author>
        public DateTime? Wedding { get; set; }

        #endregion

        #region Professional Properties

        /// <summary>
        /// Gets or sets the job title of the contact.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the name of the company that employs the contact.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the department to which the contact belongs.
        /// </summary>
        /// <value>
        /// The name of the department.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Gets or sets the location of the office that the contact works in.
        /// </summary>
        /// <value>
        /// The office location.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string OfficeLocation { get; set; }

        /// <summary>
        /// Gets or sets the name of the contact's manager.
        /// </summary>
        /// <value>
        /// The name of the manager.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string ManagerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the contact's assistant.
        /// </summary>
        /// <value>
        /// The name of the assistant.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string AssistantName { get; set; }

        /// <summary>
        /// Gets or sets the profession of the contact.
        /// </summary>
        /// <value>
        /// The profession.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Profession { get; set; }

        #endregion

        #region Other Contact Properties

        /// <summary>
        /// Gets or sets the name of the contact's spouse/partner.
        /// </summary>
        /// <value>
        /// The name of the spouse/partner.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string SpouseName { get; set; }

        /// <summary>
        /// Gets or sets the language that the contact uses.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the contact's instant messaging address.
        /// </summary>
        /// <value>
        /// The instant messaging address.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string InstantMessagingAddress { get; set; }

        /// <summary>
        /// Gets or sets the personal home page URL.
        /// </summary>
        /// <value>
        /// The personal home page.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string PersonalHomePage { get; set; }

        /// <summary>
        /// Gets or sets the business home page URL.
        /// </summary>
        /// <value>
        /// The business home page.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string BusinessHomePage { get; set; }

        /// <summary>
        /// Gets or sets the gender of the contact.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the name of the person who referred this contact.
        /// </summary>
        /// <value>
        /// The referrer.
        /// </value>
        /// <author>Lucas Nodine</author>
        public string ReferredByName { get; set; }

        #endregion

        public Contact()
        {
        }
        
        public override void BuildMappings()
        {
        }
    }
}
