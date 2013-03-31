using System;
using System.Reflection;
using AutoMapper;

namespace OpenLawOffice.Common
{
    public static class ObjectMapper
    {
        public static void MapAssembly(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in assembly.GetTypes())
            {
                MapType(type);
            }

            Mapper.AssertConfigurationIsValid();
        }

        public static void MapType(Type type)
        {
            Common.Models.MapMeAttribute mapMeAttribute;
            object[] attributes = type.GetCustomAttributes(typeof(Common.Models.MapMeAttribute), false);

            if (attributes == null || attributes.Length <= 0) 
                return;

            if (attributes.Length > 1)
                throw new System.Reflection.AmbiguousMatchException("Only a single MapMe attribute is allowed.");

            mapMeAttribute = (Common.Models.MapMeAttribute)attributes[0];

            ConstructorInfo ci = type.GetConstructor(new Type[] { });

            if (ci == null) 
                throw new TargetException("Must have a constructor with no parameters.");
            
            MethodInfo mapMeMethod = type.GetMethod(mapMeAttribute.MapMethodName);

            if (mapMeMethod == null) 
                throw new TargetException("MapMe method could not be found.");


            object obj = ci.Invoke(null);
            mapMeMethod.Invoke(obj, null);
        }
    }
}
