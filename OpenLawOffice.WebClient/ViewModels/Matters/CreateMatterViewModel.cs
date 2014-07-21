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

        public Matters.MatterContactViewModel MatterContact { get; set; }
    }
}