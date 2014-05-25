// -----------------------------------------------------------------------
// <copyright file="SelectableContactViewModel.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.WebClient.ViewModels.Contacts
{
    using System;
    using AutoMapper;
    using OpenLawOffice.Common.Models;

    [MapMe]
    public class SelectableContactViewModel : ContactViewModel
    {
        public bool IsSelected { get; set; }

        public new void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Contacts.Contact, SelectableContactViewModel>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Modified, opt => opt.MapFrom(src => src.Modified))
                .ForMember(dst => dst.Disabled, opt => opt.MapFrom(src => src.Disabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Security.UserViewModel()
                    {
                        Id = db.CreatedBy.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Security.UserViewModel()
                    {
                        Id = db.ModifiedBy.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(db =>
                {
                    if (db.DisabledBy == null || !db.DisabledBy.Id.HasValue) return null;
                    return new ViewModels.Security.UserViewModel()
                    {
                        Id = db.DisabledBy.Id.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.IsOrganization, opt => opt.MapFrom(src => src.IsOrganization))
                .ForMember(dst => dst.IsOurEmployee, opt => opt.MapFrom(src => src.IsOurEmployee))
                .ForMember(dst => dst.Nickname, opt => opt.MapFrom(src => src.Nickname))
                .ForMember(dst => dst.Generation, opt => opt.MapFrom(src => src.Generation))
                .ForMember(dst => dst.DisplayNamePrefix, opt => opt.MapFrom(src => src.DisplayNamePrefix))
                .ForMember(dst => dst.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dst => dst.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dst => dst.GivenName, opt => opt.MapFrom(src => src.GivenName))
                .ForMember(dst => dst.Initials, opt => opt.MapFrom(src => src.Initials))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dst => dst.Email1DisplayName, opt => opt.MapFrom(src => src.Email1DisplayName))
                .ForMember(dst => dst.Email1EmailAddress, opt => opt.MapFrom(src => src.Email1EmailAddress))
                .ForMember(dst => dst.Email2DisplayName, opt => opt.MapFrom(src => src.Email2DisplayName))
                .ForMember(dst => dst.Email2EmailAddress, opt => opt.MapFrom(src => src.Email2EmailAddress))
                .ForMember(dst => dst.Email3DisplayName, opt => opt.MapFrom(src => src.Email3DisplayName))
                .ForMember(dst => dst.Fax1DisplayName, opt => opt.MapFrom(src => src.Fax1DisplayName))
                .ForMember(dst => dst.Fax1FaxNumber, opt => opt.MapFrom(src => src.Fax1FaxNumber))
                .ForMember(dst => dst.Fax2DisplayName, opt => opt.MapFrom(src => src.Fax2DisplayName))
                .ForMember(dst => dst.Fax2FaxNumber, opt => opt.MapFrom(src => src.Fax2FaxNumber))
                .ForMember(dst => dst.Fax3DisplayName, opt => opt.MapFrom(src => src.Fax3DisplayName))
                .ForMember(dst => dst.Fax3FaxNumber, opt => opt.MapFrom(src => src.Fax3FaxNumber))
                .ForMember(dst => dst.Address1DisplayName, opt => opt.MapFrom(src => src.Address1DisplayName))
                .ForMember(dst => dst.Address1AddressStreet, opt => opt.MapFrom(src => src.Address1AddressStreet))
                .ForMember(dst => dst.Address1AddressCity, opt => opt.MapFrom(src => src.Address1AddressCity))
                .ForMember(dst => dst.Address1AddressStateOrProvince, opt => opt.MapFrom(src => src.Address1AddressStateOrProvince))
                .ForMember(dst => dst.Address1AddressPostalCode, opt => opt.MapFrom(src => src.Address1AddressPostalCode))
                .ForMember(dst => dst.Address1AddressCountry, opt => opt.MapFrom(src => src.Address1AddressCountry))
                .ForMember(dst => dst.Address1AddressCountryCode, opt => opt.MapFrom(src => src.Address1AddressCountryCode))
                .ForMember(dst => dst.Address1AddressPostOfficeBox, opt => opt.MapFrom(src => src.Address1AddressPostOfficeBox))
                .ForMember(dst => dst.Address2DisplayName, opt => opt.MapFrom(src => src.Address2DisplayName))
                .ForMember(dst => dst.Address2AddressStreet, opt => opt.MapFrom(src => src.Address2AddressStreet))
                .ForMember(dst => dst.Address2AddressCity, opt => opt.MapFrom(src => src.Address2AddressCity))
                .ForMember(dst => dst.Address2AddressStateOrProvince, opt => opt.MapFrom(src => src.Address2AddressStateOrProvince))
                .ForMember(dst => dst.Address2AddressPostalCode, opt => opt.MapFrom(src => src.Address2AddressPostalCode))
                .ForMember(dst => dst.Address2AddressCountry, opt => opt.MapFrom(src => src.Address2AddressCountry))
                .ForMember(dst => dst.Address2AddressCountryCode, opt => opt.MapFrom(src => src.Address2AddressCountryCode))
                .ForMember(dst => dst.Address2AddressPostOfficeBox, opt => opt.MapFrom(src => src.Address2AddressPostOfficeBox))
                .ForMember(dst => dst.Address3DisplayName, opt => opt.MapFrom(src => src.Address3DisplayName))
                .ForMember(dst => dst.Address3AddressStreet, opt => opt.MapFrom(src => src.Address3AddressStreet))
                .ForMember(dst => dst.Address3AddressCity, opt => opt.MapFrom(src => src.Address3AddressCity))
                .ForMember(dst => dst.Address3AddressStateOrProvince, opt => opt.MapFrom(src => src.Address3AddressStateOrProvince))
                .ForMember(dst => dst.Address3AddressPostalCode, opt => opt.MapFrom(src => src.Address3AddressPostalCode))
                .ForMember(dst => dst.Address3AddressCountry, opt => opt.MapFrom(src => src.Address3AddressCountry))
                .ForMember(dst => dst.Address3AddressCountryCode, opt => opt.MapFrom(src => src.Address3AddressCountryCode))
                .ForMember(dst => dst.Address3AddressPostOfficeBox, opt => opt.MapFrom(src => src.Address3AddressPostOfficeBox))
                .ForMember(dst => dst.Telephone1DisplayName, opt => opt.MapFrom(src => src.Telephone1DisplayName))
                .ForMember(dst => dst.Telephone1TelephoneNumber, opt => opt.MapFrom(src => src.Telephone1TelephoneNumber))
                .ForMember(dst => dst.Telephone2DisplayName, opt => opt.MapFrom(src => src.Telephone2DisplayName))
                .ForMember(dst => dst.Telephone2TelephoneNumber, opt => opt.MapFrom(src => src.Telephone2TelephoneNumber))
                .ForMember(dst => dst.Telephone3DisplayName, opt => opt.MapFrom(src => src.Telephone3DisplayName))
                .ForMember(dst => dst.Telephone3TelephoneNumber, opt => opt.MapFrom(src => src.Telephone3TelephoneNumber))
                .ForMember(dst => dst.Telephone4DisplayName, opt => opt.MapFrom(src => src.Telephone4DisplayName))
                .ForMember(dst => dst.Telephone4TelephoneNumber, opt => opt.MapFrom(src => src.Telephone4TelephoneNumber))
                .ForMember(dst => dst.Telephone5DisplayName, opt => opt.MapFrom(src => src.Telephone5DisplayName))
                .ForMember(dst => dst.Telephone5TelephoneNumber, opt => opt.MapFrom(src => src.Telephone5TelephoneNumber))
                .ForMember(dst => dst.Telephone6DisplayName, opt => opt.MapFrom(src => src.Telephone6DisplayName))
                .ForMember(dst => dst.Telephone6TelephoneNumber, opt => opt.MapFrom(src => src.Telephone6TelephoneNumber))
                .ForMember(dst => dst.Telephone7DisplayName, opt => opt.MapFrom(src => src.Telephone7DisplayName))
                .ForMember(dst => dst.Telephone7TelephoneNumber, opt => opt.MapFrom(src => src.Telephone7TelephoneNumber))
                .ForMember(dst => dst.Telephone8DisplayName, opt => opt.MapFrom(src => src.Telephone8DisplayName))
                .ForMember(dst => dst.Telephone8TelephoneNumber, opt => opt.MapFrom(src => src.Telephone8TelephoneNumber))
                .ForMember(dst => dst.Telephone9DisplayName, opt => opt.MapFrom(src => src.Telephone9DisplayName))
                .ForMember(dst => dst.Telephone9TelephoneNumber, opt => opt.MapFrom(src => src.Telephone9TelephoneNumber))
                .ForMember(dst => dst.Telephone10DisplayName, opt => opt.MapFrom(src => src.Telephone10DisplayName))
                .ForMember(dst => dst.Telephone10TelephoneNumber, opt => opt.MapFrom(src => src.Telephone10TelephoneNumber))
                .ForMember(dst => dst.Birthday, opt => opt.MapFrom(src => src.Birthday))
                .ForMember(dst => dst.Wedding, opt => opt.MapFrom(src => src.Wedding))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dst => dst.DepartmentName, opt => opt.MapFrom(src => src.DepartmentName))
                .ForMember(dst => dst.OfficeLocation, opt => opt.MapFrom(src => src.OfficeLocation))
                .ForMember(dst => dst.ManagerName, opt => opt.MapFrom(src => src.ManagerName))
                .ForMember(dst => dst.AssistantName, opt => opt.MapFrom(src => src.AssistantName))
                .ForMember(dst => dst.Profession, opt => opt.MapFrom(src => src.Profession))
                .ForMember(dst => dst.SpouseName, opt => opt.MapFrom(src => src.SpouseName))
                .ForMember(dst => dst.Language, opt => opt.MapFrom(src => src.Language))
                .ForMember(dst => dst.InstantMessagingAddress, opt => opt.MapFrom(src => src.InstantMessagingAddress))
                .ForMember(dst => dst.PersonalHomePage, opt => opt.MapFrom(src => src.PersonalHomePage))
                .ForMember(dst => dst.BusinessHomePage, opt => opt.MapFrom(src => src.BusinessHomePage))
                .ForMember(dst => dst.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dst => dst.ReferredByName, opt => opt.MapFrom(src => src.ReferredByName))
                .ForMember(dst => dst.IsSelected, opt => opt.Ignore());


            Mapper.CreateMap<SelectableContactViewModel, Common.Models.Contacts.Contact>()
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Modified, opt => opt.MapFrom(src => src.Modified))
                .ForMember(dst => dst.Disabled, opt => opt.MapFrom(src => src.Disabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(model =>
                {
                    if (model.CreatedBy == null || !model.CreatedBy.Id.HasValue)
                        return null;
                    return new Common.Models.Security.User()
                    {
                        Id = model.CreatedBy.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(model =>
                {
                    if (model.ModifiedBy == null || !model.ModifiedBy.Id.HasValue)
                        return null;
                    return new Common.Models.Security.User()
                    {
                        Id = model.ModifiedBy.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(model =>
                {
                    if (model.DisabledBy == null || !model.DisabledBy.Id.HasValue)
                        return null;
                    return new Common.Models.Security.User()
                    {
                        Id = model.DisabledBy.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.IsOrganization, opt => opt.MapFrom(src => src.IsOrganization))
                .ForMember(dst => dst.IsOurEmployee, opt => opt.MapFrom(src => src.IsOurEmployee))
                .ForMember(dst => dst.Nickname, opt => opt.MapFrom(src => src.Nickname))
                .ForMember(dst => dst.Generation, opt => opt.MapFrom(src => src.Generation))
                .ForMember(dst => dst.DisplayNamePrefix, opt => opt.MapFrom(src => src.DisplayNamePrefix))
                .ForMember(dst => dst.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dst => dst.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dst => dst.GivenName, opt => opt.MapFrom(src => src.GivenName))
                .ForMember(dst => dst.Initials, opt => opt.MapFrom(src => src.Initials))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dst => dst.Email1DisplayName, opt => opt.MapFrom(src => src.Email1DisplayName))
                .ForMember(dst => dst.Email1EmailAddress, opt => opt.MapFrom(src => src.Email1EmailAddress))
                .ForMember(dst => dst.Email2DisplayName, opt => opt.MapFrom(src => src.Email2DisplayName))
                .ForMember(dst => dst.Email2EmailAddress, opt => opt.MapFrom(src => src.Email2EmailAddress))
                .ForMember(dst => dst.Email3DisplayName, opt => opt.MapFrom(src => src.Email3DisplayName))
                .ForMember(dst => dst.Fax1DisplayName, opt => opt.MapFrom(src => src.Fax1DisplayName))
                .ForMember(dst => dst.Fax1FaxNumber, opt => opt.MapFrom(src => src.Fax1FaxNumber))
                .ForMember(dst => dst.Fax2DisplayName, opt => opt.MapFrom(src => src.Fax2DisplayName))
                .ForMember(dst => dst.Fax2FaxNumber, opt => opt.MapFrom(src => src.Fax2FaxNumber))
                .ForMember(dst => dst.Fax3DisplayName, opt => opt.MapFrom(src => src.Fax3DisplayName))
                .ForMember(dst => dst.Fax3FaxNumber, opt => opt.MapFrom(src => src.Fax3FaxNumber))
                .ForMember(dst => dst.Address1DisplayName, opt => opt.MapFrom(src => src.Address1DisplayName))
                .ForMember(dst => dst.Address1AddressStreet, opt => opt.MapFrom(src => src.Address1AddressStreet))
                .ForMember(dst => dst.Address1AddressCity, opt => opt.MapFrom(src => src.Address1AddressCity))
                .ForMember(dst => dst.Address1AddressStateOrProvince, opt => opt.MapFrom(src => src.Address1AddressStateOrProvince))
                .ForMember(dst => dst.Address1AddressPostalCode, opt => opt.MapFrom(src => src.Address1AddressPostalCode))
                .ForMember(dst => dst.Address1AddressCountry, opt => opt.MapFrom(src => src.Address1AddressCountry))
                .ForMember(dst => dst.Address1AddressCountryCode, opt => opt.MapFrom(src => src.Address1AddressCountryCode))
                .ForMember(dst => dst.Address1AddressPostOfficeBox, opt => opt.MapFrom(src => src.Address1AddressPostOfficeBox))
                .ForMember(dst => dst.Address2DisplayName, opt => opt.MapFrom(src => src.Address2DisplayName))
                .ForMember(dst => dst.Address2AddressStreet, opt => opt.MapFrom(src => src.Address2AddressStreet))
                .ForMember(dst => dst.Address2AddressCity, opt => opt.MapFrom(src => src.Address2AddressCity))
                .ForMember(dst => dst.Address2AddressStateOrProvince, opt => opt.MapFrom(src => src.Address2AddressStateOrProvince))
                .ForMember(dst => dst.Address2AddressPostalCode, opt => opt.MapFrom(src => src.Address2AddressPostalCode))
                .ForMember(dst => dst.Address2AddressCountry, opt => opt.MapFrom(src => src.Address2AddressCountry))
                .ForMember(dst => dst.Address2AddressCountryCode, opt => opt.MapFrom(src => src.Address2AddressCountryCode))
                .ForMember(dst => dst.Address2AddressPostOfficeBox, opt => opt.MapFrom(src => src.Address2AddressPostOfficeBox))
                .ForMember(dst => dst.Address3DisplayName, opt => opt.MapFrom(src => src.Address3DisplayName))
                .ForMember(dst => dst.Address3AddressStreet, opt => opt.MapFrom(src => src.Address3AddressStreet))
                .ForMember(dst => dst.Address3AddressCity, opt => opt.MapFrom(src => src.Address3AddressCity))
                .ForMember(dst => dst.Address3AddressStateOrProvince, opt => opt.MapFrom(src => src.Address3AddressStateOrProvince))
                .ForMember(dst => dst.Address3AddressPostalCode, opt => opt.MapFrom(src => src.Address3AddressPostalCode))
                .ForMember(dst => dst.Address3AddressCountry, opt => opt.MapFrom(src => src.Address3AddressCountry))
                .ForMember(dst => dst.Address3AddressCountryCode, opt => opt.MapFrom(src => src.Address3AddressCountryCode))
                .ForMember(dst => dst.Address3AddressPostOfficeBox, opt => opt.MapFrom(src => src.Address3AddressPostOfficeBox))
                .ForMember(dst => dst.Telephone1DisplayName, opt => opt.MapFrom(src => src.Telephone1DisplayName))
                .ForMember(dst => dst.Telephone1TelephoneNumber, opt => opt.MapFrom(src => src.Telephone1TelephoneNumber))
                .ForMember(dst => dst.Telephone2DisplayName, opt => opt.MapFrom(src => src.Telephone2DisplayName))
                .ForMember(dst => dst.Telephone2TelephoneNumber, opt => opt.MapFrom(src => src.Telephone2TelephoneNumber))
                .ForMember(dst => dst.Telephone3DisplayName, opt => opt.MapFrom(src => src.Telephone3DisplayName))
                .ForMember(dst => dst.Telephone3TelephoneNumber, opt => opt.MapFrom(src => src.Telephone3TelephoneNumber))
                .ForMember(dst => dst.Telephone4DisplayName, opt => opt.MapFrom(src => src.Telephone4DisplayName))
                .ForMember(dst => dst.Telephone4TelephoneNumber, opt => opt.MapFrom(src => src.Telephone4TelephoneNumber))
                .ForMember(dst => dst.Telephone5DisplayName, opt => opt.MapFrom(src => src.Telephone5DisplayName))
                .ForMember(dst => dst.Telephone5TelephoneNumber, opt => opt.MapFrom(src => src.Telephone5TelephoneNumber))
                .ForMember(dst => dst.Telephone6DisplayName, opt => opt.MapFrom(src => src.Telephone6DisplayName))
                .ForMember(dst => dst.Telephone6TelephoneNumber, opt => opt.MapFrom(src => src.Telephone6TelephoneNumber))
                .ForMember(dst => dst.Telephone7DisplayName, opt => opt.MapFrom(src => src.Telephone7DisplayName))
                .ForMember(dst => dst.Telephone7TelephoneNumber, opt => opt.MapFrom(src => src.Telephone7TelephoneNumber))
                .ForMember(dst => dst.Telephone8DisplayName, opt => opt.MapFrom(src => src.Telephone8DisplayName))
                .ForMember(dst => dst.Telephone8TelephoneNumber, opt => opt.MapFrom(src => src.Telephone8TelephoneNumber))
                .ForMember(dst => dst.Telephone9DisplayName, opt => opt.MapFrom(src => src.Telephone9DisplayName))
                .ForMember(dst => dst.Telephone9TelephoneNumber, opt => opt.MapFrom(src => src.Telephone9TelephoneNumber))
                .ForMember(dst => dst.Telephone10DisplayName, opt => opt.MapFrom(src => src.Telephone10DisplayName))
                .ForMember(dst => dst.Telephone10TelephoneNumber, opt => opt.MapFrom(src => src.Telephone10TelephoneNumber))
                .ForMember(dst => dst.Birthday, opt => opt.MapFrom(src => src.Birthday))
                .ForMember(dst => dst.Wedding, opt => opt.MapFrom(src => src.Wedding))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dst => dst.DepartmentName, opt => opt.MapFrom(src => src.DepartmentName))
                .ForMember(dst => dst.OfficeLocation, opt => opt.MapFrom(src => src.OfficeLocation))
                .ForMember(dst => dst.ManagerName, opt => opt.MapFrom(src => src.ManagerName))
                .ForMember(dst => dst.AssistantName, opt => opt.MapFrom(src => src.AssistantName))
                .ForMember(dst => dst.Profession, opt => opt.MapFrom(src => src.Profession))
                .ForMember(dst => dst.SpouseName, opt => opt.MapFrom(src => src.SpouseName))
                .ForMember(dst => dst.Language, opt => opt.MapFrom(src => src.Language))
                .ForMember(dst => dst.InstantMessagingAddress, opt => opt.MapFrom(src => src.InstantMessagingAddress))
                .ForMember(dst => dst.PersonalHomePage, opt => opt.MapFrom(src => src.PersonalHomePage))
                .ForMember(dst => dst.BusinessHomePage, opt => opt.MapFrom(src => src.BusinessHomePage))
                .ForMember(dst => dst.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dst => dst.ReferredByName, opt => opt.MapFrom(src => src.ReferredByName));
        }
    }
}