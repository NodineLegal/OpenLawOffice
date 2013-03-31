using System;
using System.Collections.Generic;

namespace OpenLawOffice.WinClient
{
    public class ControllerManager : Common.Singleton<ControllerManager>
    {
        private Dictionary<Type, Controllers.ControllerBase> _modelMapToController;
        private Dictionary<Type, Controllers.ControllerBase> _controllers;

        public ControllerManager()
        {
            _modelMapToController = new Dictionary<Type, Controllers.ControllerBase>();
            _controllers = new Dictionary<Type, Controllers.ControllerBase>();
        }

        public void ScanAssembly(System.Reflection.Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in assembly.GetTypes())
            {
                ScanType(type);
            }
        }

        public void ScanType(Type type)
        {
            foreach (Controllers.HandleAttribute att in type.GetCustomAttributes(typeof(Controllers.HandleAttribute), false))
            {
                if (!_controllers.ContainsKey(type))
                {
                    _controllers.Add(type, (Controllers.ControllerBase)type.GetConstructor(new Type[] { }).Invoke(null));
                }

                _modelMapToController.Add(att.ModelType, _controllers[type]);
            }
        }

        public void LoadUI<TModel>()
            where TModel : Common.Models.ModelBase
        {
            if (!_modelMapToController.ContainsKey(typeof(TModel)))
                throw new ArgumentException("Type argument cannot be found in Model mappings.");

            _modelMapToController[typeof(TModel)].LoadUI();
        }

        public void GetData(ViewModels.IViewModel obj)
        {
            System.Reflection.FieldInfo fi = obj.GetType().GetField("_model", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (fi == null)
                throw new ArgumentException("Object does not have a _model field.");

            Common.Models.ModelBase model = (Common.Models.ModelBase)fi.GetValue(obj);

            _modelMapToController[model.GetType()].GetData(obj);
        }
    }
}
