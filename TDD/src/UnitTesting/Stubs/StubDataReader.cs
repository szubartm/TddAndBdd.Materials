using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;

namespace UnitTesting.Stubs
{
    public sealed class StubDataReader : IDataReader
    {
        private readonly string[] _headers;
        private readonly Type[] _types;
        private readonly IEnumerator<object[]> _recordsEnumerator;

        public StubDataReader(IEnumerable<(string Name, Type Type)> headers, IEnumerable<object[]> records)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            _headers = headers?.Select(t => t.Name).ToArray() ?? throw new ArgumentNullException(nameof(headers));
            // ReSharper disable once PossibleMultipleEnumeration
            _types = headers.Select(t => t.Type).ToArray();
            _recordsEnumerator = records?.GetEnumerator() ?? throw new ArgumentNullException(nameof(records));
        }

        public static IEnumerable<(string Name, Type Type)> GetHeaders(Type type) => type.GetProperties().Select(p => (p.Name, p.PropertyType)).ToList();

        public string GetName(int i) => _headers[i];

        public string GetDataTypeName(int i) => GetFieldType(i).Name;

        public Type GetFieldType(int i) => _types[i];

        public object GetValue(int i)
        {
            if (IsClosed) throw new InvalidOperationException("Reader is closed");
            if (_recordsEnumerator.Current == null) throw new InvalidOperationException("Reading did not start");

            object[] row = _recordsEnumerator.Current;
            return row[i];
        }

        public int GetValues(object[] values)
        {
            int i = 0;
            while (i < values.Length)
                values[i] = GetValue(i++);
            return i;
        }

        public int GetOrdinal(string name) => Array.IndexOf(_headers, name);

        public bool GetBoolean(int i) => (bool)GetValue(i);

        public byte GetByte(int i) => (byte)GetValue(i);

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            byte[] source = (byte[])GetValue(i);

            long howManyRead = 0;
            for (long sourceIndex = fieldOffset; sourceIndex < source.Length || howManyRead < length; sourceIndex++, howManyRead++)
                buffer[bufferoffset + howManyRead] = source[sourceIndex];

            return howManyRead;
        }

        public char GetChar(int i) => (char)GetValue(i);

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            object val = GetValue(i);
            var chars = val as char[];
            char[] charArray = chars ?? ((val as IFormattable)?.ToString(null, CultureInfo.InvariantCulture) ?? val?.ToString() ?? "").ToCharArray();

            long howManyRead = 0;
            for (long sourceIndex = fieldoffset; sourceIndex < charArray.Length || howManyRead < length; sourceIndex++, howManyRead++)
                buffer[bufferoffset + howManyRead] = charArray[sourceIndex];

            return howManyRead;
        }

        public Guid GetGuid(int i) => (Guid)GetValue(i);

        public short GetInt16(int i) => (short)GetValue(i);

        public int GetInt32(int i) => (int)GetValue(i);

        public long GetInt64(int i) => (long)GetValue(i);

        public float GetFloat(int i) => (float)GetValue(i);

        public double GetDouble(int i) => (double)GetValue(i);

        public string GetString(int i) => (string)GetValue(i);

        public decimal GetDecimal(int i) => (decimal)GetValue(i);

        public DateTime GetDateTime(int i) => (DateTime)GetValue(i);

        public bool IsDBNull(int i) => GetValue(i) == null;

        public int FieldCount => _headers.Length;

        public DataTable GetSchemaTable()
        {
            bool IsNullableType(Type type, out Type underlyingType)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    underlyingType = Nullable.GetUnderlyingType(type);
                    return true;
                }
                else
                {
                    underlyingType = type;
                    return false;
                }
            }

            DataTable dt = NewSchemaTable();
            for (int i = 0; i < _headers.Length; i++)
            {
                var row = dt.NewRow();
                Type valueType = GetFieldType(i);
                bool allowDbNull = !valueType.IsValueType || IsNullableType(valueType, out valueType);

                row["ColumnName"] = _headers[i];
                row["ColumnOrdinal"] = i;
                row["DataType"] = valueType;
                row["ProviderType"] = _dbTypeMap.TryGetValue(valueType, out var providerType) ? providerType : DbType.Object;
                row["AllowDBNull"] = allowDbNull;
                row["IsReadOnly"] = true;

                row["IsUnique"] = false;
                row["IsKey"] = false;
                row["IsAutoIncrement"] = false;
                row["BaseSchemaName"] = "Schema";
                row["BaseCatalogName"] = "Catalog";
                row["BaseTableName"] = "Table";
                row["BaseColumnName"] = _headers[i];

                //row["ColumnSize"]=       ; // typeof(int)));
                //row["NumericPrecision"]=       ; // typeof(short)));
                //row["NumericScale"]=       ; // typeof(short)));
                //row["IsLong"]=       ; // typeof(bool)));
                //row["IsRowVersion"]=       ; // typeof(bool)));

                dt.Rows.Add(row);
            }

            return dt;
        }

        private DataTable NewSchemaTable()
        {
            var table = new DataTable("SchemaTable") { Locale = CultureInfo.InvariantCulture, MinimumCapacity = FieldCount };
            DataColumnCollection columns = table.Columns;

            var columnTypes = new List<(string ColumnName, Type ColumnType)>
            {
                ("ColumnName", typeof(string)),
                ("ColumnOrdinal", typeof(int)),
                ("DataType", typeof(object)),
                ("ProviderType", typeof(DbType)),
                ("AllowDBNull", typeof(bool)),
                ("IsReadOnly", typeof(bool)),
                ("IsUnique", typeof(bool)),
                ("IsKey", typeof(bool)),
                ("IsAutoIncrement", typeof(bool)),
                ("BaseSchemaName", typeof(string)),
                ("BaseCatalogName", typeof(string)),
                ("BaseTableName", typeof(string)),
                ("BaseColumnName", typeof(string))
                //("ColumnSize", typeof(int)),
                //("NumericPrecision", typeof(short)),
                //("NumericScale", typeof(short)),
                //("IsLong", typeof(bool)),
                //("IsRowVersion", typeof(bool)),
            }.AsReadOnly();

            foreach (var (columnName, columnType) in columnTypes)
                columns.Add(new DataColumn(columnName, columnType) { ReadOnly = true });

            return table;
        }

        private static readonly IDictionary<Type, DbType> _dbTypeMap = new ReadOnlyDictionary<Type, DbType>(
            new Dictionary<Type, DbType>
            {
                [typeof(byte)] = DbType.Byte,
                [typeof(sbyte)] = DbType.SByte,
                [typeof(short)] = DbType.Int16,
                [typeof(ushort)] = DbType.UInt16,
                [typeof(int)] = DbType.Int32,
                [typeof(uint)] = DbType.UInt32,
                [typeof(long)] = DbType.Int64,
                [typeof(ulong)] = DbType.UInt64,
                [typeof(float)] = DbType.Single,
                [typeof(double)] = DbType.Double,
                [typeof(decimal)] = DbType.Decimal,
                [typeof(bool)] = DbType.Boolean,
                [typeof(string)] = DbType.String,
                [typeof(char)] = DbType.StringFixedLength,
                [typeof(Guid)] = DbType.Guid,
                [typeof(DateTime)] = DbType.DateTime,
                [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
                [typeof(byte[])] = DbType.Binary,
                [typeof(byte?)] = DbType.Byte,
                [typeof(sbyte?)] = DbType.SByte,
                [typeof(short?)] = DbType.Int16,
                [typeof(ushort?)] = DbType.UInt16,
                [typeof(int?)] = DbType.Int32,
                [typeof(uint?)] = DbType.UInt32,
                [typeof(long?)] = DbType.Int64,
                [typeof(ulong?)] = DbType.UInt64,
                [typeof(float?)] = DbType.Single,
                [typeof(double?)] = DbType.Double,
                [typeof(decimal?)] = DbType.Decimal,
                [typeof(bool?)] = DbType.Boolean,
                [typeof(char?)] = DbType.StringFixedLength,
                [typeof(Guid?)] = DbType.Guid,
                [typeof(DateTime?)] = DbType.DateTime,
                [typeof(DateTimeOffset?)] = DbType.DateTimeOffset,

            });


        public bool Read() => _recordsEnumerator.MoveNext();

        public object this[string name] => GetValue(GetOrdinal(name));

        public object this[int i] => GetValue(i);

        #region State
        public void Dispose() => Close();

        public void Close()
        {
            IsClosed = true;
            _recordsEnumerator.Dispose();
        }

        public bool IsClosed { get; private set; }

        public int Depth => throw new NotSupportedException();

        public bool NextResult() => throw new NotSupportedException();

        public IDataReader GetData(int i) => throw new NotSupportedException();

        public int RecordsAffected => 0;
        #endregion
    }
}
