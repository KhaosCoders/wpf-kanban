using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KC.WPF_Kanban.Utils
{
    /// <summary>
    /// This resolver can dynamicaly access properties of objects
    /// </summary>
    public static class PropertyPathResolver
    {
        private static Dictionary<Type, List<PropertyInfo>> _knownProperties = new Dictionary<Type, List<PropertyInfo>>();

        /// <summary>
        /// Returns the value of a given property path on a given object
        /// </summary>
        public static object ResolvePath(object dataContext, string propertyPath)
        {
            object result = null;

            string[] parts = propertyPath.Split('.');
            object value = dataContext;
            foreach (string part in parts)
            {
                value = ResolveProperty(value, part);
                if (value == null)
                {
                    break;
                }
            }
            if (value != dataContext)
            {
                result = value;
            }
            return result;
        }

        private static object ResolveProperty(object context, string propertyPath)
        {
            if (context == null)
            {
                throw new ArgumentException("Parameter `context` must not be null");
            }
            if (string.IsNullOrWhiteSpace(propertyPath))
            {
                throw new ArgumentException("Parameter `propertyPath` is mandatory");
            }
            if (propertyPath[0] == '[')
            {
                return ResolveIndexer(context, propertyPath);
            }
            object value = null;
            // Read Properties from Type
            Type type = context.GetType();
            if (!_knownProperties.TryGetValue(type, out List<PropertyInfo> properties))
            {
                properties = type.GetProperties().ToList();
                _knownProperties.Add(type, properties);
            }
            // Find the correct Property
            PropertyInfo property = properties.Find(p => p.Name.Equals(propertyPath));
            if (property != null)
            {
                value = property.GetValue(context);
            }
            return value;
        }

        private static object ResolveIndexer(object context, string indexer)
        {
            if (context == null)
            {
                throw new ArgumentException("Parameter `context` must not be null");
            }
            if (string.IsNullOrWhiteSpace(indexer))
            {
                throw new ArgumentException("Parameter `propertyPath` is mandatory");
            }
            if (indexer[0] != '[')
            {
                throw new ArgumentException("Parameter `indexer` must be in format [indexer]");
            }
            // Strip [ and ]
            string[] indexerValues = indexer.Substring(1, indexer.Length - 2).Split(',');
            // Read Properties from Type
            Type type = context.GetType();
            if (!_knownProperties.TryGetValue(type, out List<PropertyInfo> properties))
            {
                properties = type.GetProperties().ToList();
                _knownProperties.Add(type, properties);
            }
            // Find fitting indexers
            PropertyInfo[] indexerProperties = properties.Where(p => p.GetIndexParameters().Length == indexerValues.Length).ToArray();
            if (indexerProperties == null || indexerProperties.Length == 0)
            {
                throw new InvalidOperationException("There is no indexer property with this amount of parameters: " + indexer);
            }

            foreach (PropertyInfo indexerProperty in indexerProperties)
            {
                // convert parameters to correct type
                ParameterInfo[] parameterInfos = indexerProperty.GetIndexParameters();
                object[] parameters = new object[parameterInfos.Length];
                bool parsedSuccessfull = true;
                for (int parameterIndex = 0; parameterIndex < parameterInfos.Length; parameterIndex++)
                {
                    ParameterInfo parameterInfo = parameterInfos[parameterIndex];
                    parameters[parameterIndex] = ParseToType(indexerValues[parameterIndex], parameterInfo.ParameterType, out parsedSuccessfull);
                    if (!parsedSuccessfull)
                    {
                        break;
                    }
                }
                // If convert failed
                if (!parsedSuccessfull)
                {
                    continue;
                }
                // Access indexer
                return indexerProperty.GetValue(context, parameters);
            }

            return null;
        }

        private static object ParseToType(string sValue, Type targetType, out bool parsedSuccessfull)
        {
            parsedSuccessfull = true;
            if (targetType == typeof(string))
            {
                return sValue;
            }
            else if (targetType == typeof(int))
            {
                int value = 0;
                if (int.TryParse(sValue, out value))
                {
                    return value;
                }
            }
            parsedSuccessfull = false;
            return null;
        }
    }
}
