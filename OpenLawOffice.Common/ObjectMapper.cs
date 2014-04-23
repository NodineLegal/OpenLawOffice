using System;
using System.Reflection;
using AutoMapper;
using NLog;

namespace OpenLawOffice.Common
{
    public static class ObjectMapper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void MapAssembly(Assembly assembly)
        {
            try
            {
                logger.Debug("Mapping assembly: " + assembly.FullName);
                Type[] types = assembly.GetTypes();
                foreach (Type type in assembly.GetTypes())
                {
                    MapType(type);
                }
            }
            catch (Exception ex)
            {
                logger.FatalException("Exception mapping assembly: " + assembly.FullName, ex);
                throw ex;
            }

            try
            {
                Mapper.AssertConfigurationIsValid();
            }
            catch (AutoMapperConfigurationException ex)
            {
                logger.FatalException("Mapping configuration error.", ex);
                throw ex;
            }
            catch (Exception ex)
            {
                logger.FatalException("An unexpected exception occured while attempting to call Mapper.AssertConfigurationIsValid().", ex);
                throw ex;
            }
        }

        public static void MapType(Type type)
        {
            try
            {
                MethodInfo mapMeMethod;
                Common.Models.MapMeAttribute mapMeAttribute;
                object[] attributes = type.GetCustomAttributes(typeof(Common.Models.MapMeAttribute), false);

                if (attributes == null || attributes.Length <= 0)
                    return;

                if (attributes.Length > 1)
                {
                    logger.Fatal("Multiple MapMe attributes found on type '" + type.FullName + "'.  MapMe attribute is only allowed a single instance per class.");
                    throw new System.Reflection.AmbiguousMatchException("Only a single MapMe attribute is allowed.");
                }

                mapMeAttribute = (Common.Models.MapMeAttribute)attributes[0];

                ConstructorInfo ci = type.GetConstructor(new Type[] { });

                if (ci == null)
                {
                    mapMeMethod = type.GetMethod("BuildMappings", BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
                    if (mapMeMethod == null)
                    {
                        logger.Fatal("Unable to map type '" + type.FullName + "' because I was unable to find a BuildMappings method.");
                        throw new TargetException("Must heither a static implementation of BuildMappings() or a public constructor and instance implementation of BuildMappings().");
                    }

                    logger.Debug("Mapping type: " + type.FullName);
                    mapMeMethod.Invoke(null, null);
                }
                else
                {
                    mapMeMethod = type.GetMethod(mapMeAttribute.MapMethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

                    if (mapMeMethod == null)
                    {
                        logger.Fatal("Unable to map type '" + type.FullName + "' because I was unable to find a BuildMappings method.");
                        throw new TargetException("MapMe method could not be found.");
                    }

                    logger.Debug("Mapping type: " + type.FullName);
                    object obj = ci.Invoke(null);
                    mapMeMethod.Invoke(obj, null);
                }
            }
            catch (Exception ex)
            {
                logger.FatalException("An unexpected exception occurred while attempting to map the type '" + type.FullName + "'", ex);
                throw ex;
            }
        }
    }
}