using System;
using System.IO;

namespace ArmaForces.Boderator.Core.Tests.TestUtilities;

public static class TestDatabaseConstants
{
    public static string TestDbConnectionString => TestSqlServerConnectionString ?? throw new InvalidOperationException("Connection string is empty");
    
    private static readonly string TestSqliteConnectionString = "Data Source=" + Path.Join(Directory.GetCurrentDirectory(), "test.db");

    private const string TestSqlServerConnectionString = "Data Source=127.0.0.1,49443; User ID=Boderator; Password=boderator-test1; Database=BODERATOR_TEST";
}
