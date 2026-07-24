using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SmartAssetTracking.App.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AssetDbContext>
    {
        public AssetDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AssetDbContext>();

            optionsBuilder.UseSqlite("Data Source=assets.db");

            return new AssetDbContext(optionsBuilder.Options);
        }
    }
}