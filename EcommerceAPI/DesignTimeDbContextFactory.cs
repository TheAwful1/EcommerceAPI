using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? "server=localhost;port=3306;database=ecommerce_dummy;user=root;password=root";

        optionsBuilder.UseMySql(
            connectionString, // ✅ Ahora sí usa la variable
            new MySqlServerVersion(new Version(8, 0, 36))
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}