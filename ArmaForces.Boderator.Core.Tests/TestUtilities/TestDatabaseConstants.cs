using System.IO;

namespace ArmaForces.Boderator.Core.Tests.TestUtilities;

public static class TestDatabaseConstants
{
    public static readonly string TestSqliteConnectionString = "Data Source=" + Path.Join(Directory.GetCurrentDirectory(), "test.db");

    public static readonly string TestSqlServerConnectionString =
        "Data Source=127.0.0.1,49443; User ID=Boderator; Password=boderator-test1; Database=BODERATOR_TEST";
}
