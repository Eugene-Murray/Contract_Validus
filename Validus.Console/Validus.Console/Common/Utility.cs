using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
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

            using (TextWriter writer = new StringWriter(sb))
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
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string ordering, params object[] values)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static T XmlDeserializeFromString<T>(string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        public static object XmlDeserializeFromString(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }     

        public static void SetObjectPropertyValue(object obj, string propertyName, DataRow row, string columnName)
        {
            PropertyInfo property = obj.GetType().GetProperty(propertyName);
            try
            {
                if (row[columnName] != null)
                {
                    object value = row[columnName];
                    if ((value != null) && (value != DBNull.Value))
                    {
                        if (property != null)
                        {
                            property.SetValue(obj, value, null);
                        }
                    }
                }
            }
            catch
            {
                if (property != null)
                {
                    property.SetValue(obj, null, null);
                }
            }
            finally
            {
            }
        }

        public static void SetObjectPropertyValue(object obj, string propertyName, DbDataReader reader, string columnName)
        {
            PropertyInfo property = obj.GetType().GetProperty(propertyName);
            try
            {
                if (reader[columnName] != null)
                {
                    object value = reader[columnName];
                    if ((value != null) && (value != DBNull.Value))
                    {
                        if (property != null)
                        {
                            property.SetValue(obj, value, null);
                        }
                    }
                }
            }
            catch
            {
                if (property != null)
                {
                    property.SetValue(obj, null, null);
                }
            }
            finally
            {
            }
        }

        public static void PrintErrorResults(IEnumerable<ValidationResult> results, List<string> errors)
        {
            foreach (var validationResult in results)
            {
                errors.Add(validationResult.ErrorMessage);
                if (validationResult is CompositeValidationResult)
                {
                    PrintErrorResults(((CompositeValidationResult)validationResult).Results, errors);
                }
            }
        }

    }
}