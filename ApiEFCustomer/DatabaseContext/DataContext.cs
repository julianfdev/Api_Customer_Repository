using EFCoreInMemory.DataModel;
using Microsoft.EntityFrameworkCore;

namespace EFCoreInMemory.DatabaseContext
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<CustomerDataModel> Customer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase(databaseName: "CustomerDb");
        }

    }
}
