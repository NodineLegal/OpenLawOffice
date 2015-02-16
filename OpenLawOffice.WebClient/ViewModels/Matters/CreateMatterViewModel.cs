using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenLawOffice.WebClient.ViewModels.Matters
{
    public class CreateMatterViewModel
    {
        public MatterViewModel Matter { get; set; }

        public Matters.ResponsibleUserViewModel ResponsibleUser { get; set; }

        public Matters.MatterContactViewModel LeadAttorney { get; set; }

        public Contacts.ContactViewModel Contact1 { get; set; }
        public string Role1 { get; set; }

        public Contacts.ContactViewModel Contact2 { get; set; }
        public string Role2  { get; set; }

        public Contacts.ContactViewModel Contact3 { get; set; }
        public string Role3 { get; set; }

        public Contacts.ContactViewModel Contact4 { get; set; }
        public string Role4 { get; set; }

        public Contacts.ContactViewModel Contact5 { get; set; }
        public string Role5 { get; set; }

        public Contacts.ContactViewModel Contact6 { get; set; }
        public string Role6 { get; set; }
    }
}