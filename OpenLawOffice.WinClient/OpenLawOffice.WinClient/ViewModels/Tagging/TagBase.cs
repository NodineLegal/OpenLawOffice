using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenLawOffice.WinClient.ViewModels.Tagging
{
    public abstract class TagBase<TModel> : ViewModelCore<TModel>
        where TModel : Common.Models.Tagging.TagBase, new()
    {
        private Guid? _id;
        public Guid? Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private TagCategory _tagCategory;
        public TagCategory TagCategory
        {
            get { return _tagCategory; }
            set
            {
                _tagCategory = value;
                OnPropertyChanged("TagCategory");
            }
        }

        private string _tag;
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
                OnPropertyChanged("Tag");
            }
        }

        public TagBase()
            : base()
        {
        }

        public TagBase(TModel model)
            : base(model)
        {
        }
    }
}
