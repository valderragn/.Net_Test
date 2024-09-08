using Microsoft.Data.SqlClient;

namespace NetTest.Extensions
{
    public static class SqlDataReaderExtensions
    {
        public static T SafeGet<T>(this SqlDataReader reader, string columnName)
        {
            int colIndex = reader.GetOrdinal(columnName);
            if (!reader.IsDBNull(colIndex))
            {
                return reader.GetFieldValue<T>(colIndex);
            }
            return default;
        }
    }
}
