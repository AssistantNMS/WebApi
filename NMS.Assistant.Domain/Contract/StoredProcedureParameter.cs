using System;
using System.Collections.Generic;
using System.Data;

namespace NMS.Assistant.Domain.Contract
{
    public class StoredProcedureParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string TypeName { get; set; }
        public SqlDbType DataType { get; set; }
        
        public static StoredProcedureParameter StringParam(string name, string value)
        {
            return new StoredProcedureParameter
            {
                Name = name,
                Value = value,
                DataType = SqlDbType.NVarChar,
            };
        }
        
        public static StoredProcedureParameter GuidParam(string name, Guid? value)
        {
            return new StoredProcedureParameter
            {
                Name = name,
                Value = value,
                DataType = SqlDbType.UniqueIdentifier,
            };
        }

        public static StoredProcedureParameter IntParam(string name, int value)
        {
            return new StoredProcedureParameter
            {
                Name = name,
                Value = value,
                DataType = SqlDbType.Int,
            };
        }

        public static StoredProcedureParameter DecimalParam(string name, decimal value)
        {
            return new StoredProcedureParameter
            {
                Name = name,
                Value = value,
                DataType = SqlDbType.Decimal,
            };
        }

        public static StoredProcedureParameter BoolParam(string name, bool value)
        {
            return new StoredProcedureParameter
            {
                Name = name,
                Value = value,
                DataType = SqlDbType.Bit,
            };
        }

        public static StoredProcedureParameter GuidListParam(string name, string typeName, List<Guid> guidList)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Guid", typeof(Guid));
            foreach (Guid guid in guidList ?? new List<Guid>())
            {
                table.Rows.Add(guid);
            }

            return new StoredProcedureParameter
            {
                Name = name,
                Value = table,
                TypeName = typeName,
                DataType = SqlDbType.Structured,
            };
        }

        public static StoredProcedureParameter EnumListParam(string name, string typeName, List<int> enumList)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            foreach (int enumObj in enumList ?? new List<int>())
            {
                table.Rows.Add(enumObj);
            }

            return new StoredProcedureParameter
            {
                Name = name,
                Value = table,
                TypeName = typeName,
                DataType = SqlDbType.Structured,
            };
        }

        public static StoredProcedureParameter StringListParam(string name, string typeName, List<string> stringList)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(string));
            foreach (string param in stringList ?? new List<string>())
            {
                table.Rows.Add(param);
            }

            return new StoredProcedureParameter
            {
                Name = name,
                Value = table,
                TypeName = typeName,
                DataType = SqlDbType.Structured,
            };
        }

        public static StoredProcedureParameter NullableDateParam(string name, DateTime? value)
        {
            return new StoredProcedureParameter
            {
                Name = name,
                Value = value,
                DataType = SqlDbType.DateTime,
            };
        }
    }
}
