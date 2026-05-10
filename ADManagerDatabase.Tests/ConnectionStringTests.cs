using ADManager.Common.Data.Database;
namespace ADManagerDatabase.Tests
{
    public class ConnectionStringTests
    {
        private const string Valid_SQLite_Connection_String = "Data Source=C:\\ProgramData\\AD Manager\\database.db;";
        private const string Valid_SQLite_Connection_String2 = "data source=C:\\ProgramData\\AD Manager\\database.db;";
        private const string Valid_SQLite_Connection_String3 = "Data Source=C:\\ProgramData\\AD Manager\\database.db";
        private const string Valid_SQL_Connection_String = "Data Source=sql-admanager-org;Database=AD ManagerTest;User Id=sa;Password=admanager;";
        private const string Valid_SQL_Connection_String2 = "data source=sql-admanager-org;database=AD ManagerTest;user id=sa;password=admanager;";
        private const string Valid_SQL_Connection_String3 = "Data Source=sql-admanager-org;Database=AD ManagerTest;User Id=sa;Password=admanager";
        private const string Valid_SQLExpress_Connection_String = "Data Source=sql-admanager-org\\SQLEXPRESS,1433;Database=AD ManagerTest;User Id=sa;Password=admanager;";
        private const string Valid_SQLExpress_Connection_String2 = "data source=sql-admanager-org\\SQLEXPRESS,1433;database=AD ManagerTest;user id=sa;password=admanager;";
        private const string Valid_SQLExpress_Connection_String3 = "Data Source=sql-admanager-org\\SQLEXPRESS,1433;Database=AD ManagerTest;User Id=sa;Password=admanager";

        [Theory]
        [InlineData(Valid_SQLite_Connection_String)]
        [InlineData(Valid_SQLite_Connection_String2)]
        [InlineData(Valid_SQLite_Connection_String3)]
        public void Valid_SQLite_Returns_Valid_File(string raw)
        {
            var cstring = new DatabaseConnectionString(raw, DatabaseType.SQLite);

            // Test File.FullPath property
            Assert.Equal(raw.Replace(";", "").Split("=")[1], cstring.File.FullPath);

            // Test FileBased property
            Assert.True(cstring.FileBased);

            // Test Database property
            Assert.Equal("File Based", cstring.GetDatabase());
        }

        [Theory]
        [InlineData(Valid_SQL_Connection_String)]
        [InlineData(Valid_SQL_Connection_String2)]
        [InlineData(Valid_SQL_Connection_String3)]
        public void Valid_SQL_Returns_Valid_Data(string raw)
        {
            var cstring = new DatabaseConnectionString(raw, DatabaseType.SQL);

            // Test ServerAddress property
            Assert.Equal("sql-admanager-org", cstring.GetServerAddress());

            // Test Database property
            Assert.Equal("AD ManagerTest", cstring.GetDatabase());

            // Test FileBased property
            Assert.False(cstring.FileBased);

            // Test InstanceName property (should be null for non-SQLExpress)
            Assert.Null(cstring.InstanceName);

            // Test ServerPort property (should be default 1433 for SQL)
            Assert.Equal(1433, cstring.GetServerPort());
        }

        [Theory]
        [InlineData(Valid_SQLExpress_Connection_String)]
        [InlineData(Valid_SQLExpress_Connection_String2)]
        [InlineData(Valid_SQLExpress_Connection_String3)]
        public void Valid_SQLExpress_Returns_Valid_Data(string raw)
        {
            var cstring = new DatabaseConnectionString(raw, DatabaseType.SQL);

            // Test ServerAddress property
            Assert.Equal("sql-admanager-org", cstring.GetServerAddress());

            // Test InstanceName property
            Assert.Equal("SQLEXPRESS", cstring.InstanceName);

            // Test ServerPort property
            Assert.Equal(1433, cstring.GetServerPort());

            // Test Database property
            Assert.Equal("AD ManagerTest", cstring.GetDatabase());

            // Test FileBased property
            Assert.False(cstring.FileBased);
        }
    }
}