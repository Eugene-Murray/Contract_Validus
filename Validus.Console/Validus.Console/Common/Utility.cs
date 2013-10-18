using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;
using Validus.Core.Validation;

namespace Validus.Console
{
    public static class Utility
    {
        public static string XmlSerializeToString(object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

	    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering, params object[] values)
	    {
		    var type = typeof(T);
		    var property = type.GetProperty(ordering);
		    var parameter = Expression.Parameter(type, "p");
		    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
		    var orderByExp = Expression.Lambda(propertyAccess, parameter);
		    var resultExp = Expression.Call(typeof(Queryable), "OrderBy", new[] { type, property.PropertyType },
		                                    source.Expression, Expression.Quote(orderByExp));
		    
			return source.Provider.CreateQuery<T>(resultExp);
	    }

	    public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string ordering, params object[] values)
	    {
		    var type = typeof(T);
		    var property = type.GetProperty(ordering);
		    var parameter = Expression.Parameter(type, "p");
		    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
		    var orderByExp = Expression.Lambda(propertyAccess, parameter);
		    var resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new[] { type, property.PropertyType },
		                                    source.Expression, Expression.Quote(orderByExp));

		    return source.Provider.CreateQuery<T>(resultExp);
	    }

	    public static T XmlDeserializeFromString<T>(string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        public static object XmlDeserializeFromString(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
	        var result = default(object);

            using (var reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

		public static void SetObjectPropertyValue(object target, string propertyName, object source, string columnName)
		{
			var property = default(PropertyInfo);
			var value = default(object);

			try
			{
				property = target.GetType().GetProperty(propertyName);

				if (source is DataRow)
				{
					value = ((DataRow)source)[columnName];
				}
				else if (source is DbDataReader)
				{
					value = ((DbDataReader)source)[columnName];
				}

				// TODO: What if you want to set the property to null ?
				if (property != null && value != null && value != DBNull.Value)
				{
					property.SetValue(target, value, null);
				}
			}
			catch
			{
				if (property != null)
				{
					property.SetValue(target, null, null);
				}
			}
        }

        public static void PrintErrorResults(IEnumerable<ValidationResult> results, List<string> errors) // TODO: Change parameter to "out List<string> errors"
        {
            foreach (var validationResult in results)
            {
                errors.Add(validationResult.ErrorMessage);

                if (validationResult is CompositeValidationResult)
                {
                    Utility.PrintErrorResults(((CompositeValidationResult)validationResult).Results, errors);
                }
            }
        }
    }
}