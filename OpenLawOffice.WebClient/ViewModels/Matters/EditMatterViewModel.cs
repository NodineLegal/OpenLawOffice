using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenLawOffice.WebClient.ViewModels.Matters
{
    public class EditMatterViewModel
    {
        public MatterViewModel Matter { get; set; }

        public Matters.MatterContactViewModel LeadAttorney { get; set; }
    }
}