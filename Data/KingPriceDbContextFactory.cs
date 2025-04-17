namespace API.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class KingPriceDbContextFactory : IDesignTimeDbContextFactory<KingPriceDbContext>
    {
        public KingPriceDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<KingPriceDbContext> optionsBuilder = new();
            optionsBuilder.UseSqlServer("Server=localhost;Database=YourDb;Trusted_Connection=True;");

            return new KingPriceDbContext(optionsBuilder.Options);
        }
    }
}
