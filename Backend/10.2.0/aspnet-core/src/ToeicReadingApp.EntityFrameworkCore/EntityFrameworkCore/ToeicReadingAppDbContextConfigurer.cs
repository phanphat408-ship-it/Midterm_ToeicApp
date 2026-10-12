using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ToeicReadingApp.EntityFrameworkCore;

public static class ToeicReadingAppDbContextConfigurer
{
    public static void Configure(DbContextOptionsBuilder<ToeicReadingAppDbContext> builder, string connectionString)
    {
        builder.UseSqlServer(connectionString);
    }

    public static void Configure(DbContextOptionsBuilder<ToeicReadingAppDbContext> builder, DbConnection connection)
    {
        builder.UseSqlServer(connection);
    }
}
