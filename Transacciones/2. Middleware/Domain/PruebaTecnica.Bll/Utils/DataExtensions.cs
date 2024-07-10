using System.Data;
using System.Reflection;

namespace PruebaTecnica.Test.Bll.Utils
{
    public static class DataExtensions
    {
        /// <summary>
        /// Permite tranformar un DataTable a un entidad especifica.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToEntityClass<T>(this DataTable dataTable)
        {
            List<T> list = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                T item = row.GetItem<T>();
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// Permite obtener el valor de una propiedad de un DataRow
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T GetItem<T>(this DataRow dataRow)
        {
            Type typeFromHandle = typeof(T);
            T val = Activator.CreateInstance<T>();
            foreach (DataColumn column in dataRow.Table.Columns)
            {
                PropertyInfo[] properties = typeFromHandle.GetProperties();
                foreach (PropertyInfo propertyInfo in properties)
                {
                    if (propertyInfo.Name == column.ColumnName && dataRow[column.ColumnName] != DBNull.Value)
                    {
                        propertyInfo.SetValue(val, ChangeType(dataRow[column.ColumnName], propertyInfo.PropertyType), null);
                    }
                }
            }
            return val;
        }

        public static object ChangeType(object value, Type t)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }

        /// <summary>
        /// Validar existencia de datos en un DataTable
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool HasData(this DataTable dataTable)
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}