using System;
using System.Reflection;
using AutoMapper;

namespace OpenLawOffice.WinClient.ViewModels
{
    public abstract class ViewModelBase<TModel>
        : ViewModelBase
        where TModel : Common.Models.ModelBase
    {
        public ViewModelBase()
            : base()
        {
        }

        public ViewModelBase(TModel model)
            : base(model)
        {
        }

        public TModel Model
        {
            get
            {
                if (_model == null)
                    throw new UnboundModelException("ViewModel does not have a Model bound.");
                return (TModel)_model;
            }

            protected set
            {
                _model = value;
            }
        }
    }

    public abstract class ViewModelBase 
        : System.ComponentModel.INotifyPropertyChanged, IViewModel
    {
        protected Common.Models.ModelBase _model;

        public ViewModelBase()
        {
            _model = null;
        }

        public ViewModelBase(Common.Models.ModelBase model)
        {
            _model = model;
            State = StateType.Unknown;
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public bool IsDummy { get; set; }

        public virtual bool IsHierarchical { get { return false; } }

        public StateType State { get; protected set; }
        public static TViewModel Create<TViewModel>(Common.Models.ModelBase model)
            where TViewModel : ViewModelBase
        {
            TViewModel viewModel = Mapper.Map<TViewModel>(model);
            viewModel.Bind(model);
            viewModel.State = StateType.Synchronized;
            return viewModel;
        }

        public static IViewModel Create(Common.Models.ModelBase model, Type viewModelType)
        {
            ViewModelBase viewModel = (ViewModelBase)Mapper.Map(model, model.GetType(), viewModelType);
            viewModel.Bind(model);
            viewModel.State = StateType.Synchronized;
            return viewModel;
        }

        public static IViewModel Create(Type viewModelType)
        {
            ViewModelBase viewModel = null;

            ConstructorInfo ci = viewModelType.GetConstructor(new Type[] { });
            if (ci == null)
                throw new TargetException("Must have a public parameterless constructor.");

            viewModel = (ViewModelBase)ci.Invoke(null);

            return viewModel;
        }

        public void Bind(Common.Models.ModelBase model)
        {
            _model = model;
        }

        public override bool Equals(object obj)
        {
            Type thisType = GetType();
            Type thatType = obj.GetType();

            if (thisType != thatType)
                return base.Equals(obj);

            PropertyInfo thisIdInfo = thisType.GetProperty("Id");
            PropertyInfo thatIdInfo = thatType.GetProperty("Id");

            object thisId = thisIdInfo.GetValue(this, null);
            object thatId = thatIdInfo.GetValue(obj, null);

            if (thisId == null && thatId == null)
                return base.Equals(obj);
            else if (thisId == null ^ thatId == null) // one XOR the other
                return false;

            Type idType = thisId.GetType();

            if (typeof(int).IsAssignableFrom(idType))
            {
                if ((int)thisId == (int)thatId)
                    return true;
            }
            else if (typeof(int?).IsAssignableFrom(idType))
            {
                if ((int?)thisId == (int?)thatId)
                    return true;
            }
            else if (typeof(long).IsAssignableFrom(idType))
            {
                if ((long)thisId == (long)thatId)
                    return true;
            }
            else if (typeof(long?).IsAssignableFrom(idType))
            {
                if ((long?)thisId == (long?)thatId)
                    return true;
            }
            else if (typeof(Guid).IsAssignableFrom(idType))
            {
                if ((Guid)thisId == (Guid)thatId)
                    return true;
            }
            else if (typeof(Guid?).IsAssignableFrom(idType))
            {
                if ((Guid?)thisId == (Guid?)thatId)
                    return true;
            }
            else
            {
                return base.Equals(obj);
            }

            return false;
        }

        public TModel Export<TModel>()
        {
            return Mapper.Map<TModel>(this);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Common.Models.ModelBase GetModel()
        {
            return _model;
        }

        public T GetModel<T>()
            where T : Common.Models.ModelBase
        {
            return (T)_model;
        }

        public void UpdateModel()
        {
            _model = (Common.Models.ModelBase)Mapper.Map(this, this.GetType(), _model.GetType());
            State = StateType.Synchronized;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            State = StateType.ViewModelIsNewer;
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}