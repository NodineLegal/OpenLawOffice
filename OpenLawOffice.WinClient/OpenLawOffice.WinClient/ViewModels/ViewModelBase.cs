using System;
using System.Windows.Threading;
using AutoMapper;

namespace OpenLawOffice.WinClient.ViewModels
{
    public abstract class ViewModelBase<TModel> 
        : ViewModelBase
        where TModel : Common.Models.ModelBase
    {
        public TModel Model 
        { 
            get 
            {
                if (_model == null)
                    throw new UnboundModelException("ViewModel does not have a Model bound.");
                return (TModel)_model; 
            } 
            protected set { _model = value; }
        }

        public ViewModelBase()
            : base()
        {
        }

        public ViewModelBase(TModel model)
            : base(model)
        {
        }
    }

    public abstract class ViewModelBase : System.ComponentModel.INotifyPropertyChanged, IViewModel
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected Common.Models.ModelBase _model;

        public bool IsDummy { get; set; }
        public virtual bool IsHierarchical { get { return false; } }
        public StateType State { get; protected set; }

        public ViewModelBase()
        {
            _model = null;
        }

        public ViewModelBase(Common.Models.ModelBase model)
        {
            _model = model;
            State = StateType.Unknown;
        }
        
        protected void OnPropertyChanged(string propertyName)
        {
            State = StateType.ViewModelIsNewer;
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        public static TViewModel Create<TViewModel>(Common.Models.ModelBase model)
            where TViewModel : ViewModelBase
        {
            TViewModel viewModel = Mapper.Map<TViewModel>(model);
            viewModel.Bind(model);
            viewModel.State = StateType.Synchronized;
            return viewModel;
        }

        public TModel Export<TModel>()
        {
            return Mapper.Map<TModel>(this);
        }

        public void UpdateModel()
        {
            _model = (Common.Models.ModelBase)Mapper.Map(this, this.GetType(), _model.GetType());
            State = StateType.Synchronized;
        }

        public void Bind(Common.Models.ModelBase model)
        {
            _model = model;
        }
    }
}
