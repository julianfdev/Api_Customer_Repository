using EFCoreInMemory.DatabaseContext;
using EFCoreInMemory.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{

    public class TestDataFixture : IDisposable
    {
        public DataContext context { get; set; }

        public TestDataFixture()
        {
            var builder = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "MyDb");
            context = new DataContext(builder.Options);
            InitializeTestDB();
        }

        private void InitializeTestDB()
        {

            CustomerDataModel customer_1 = new CustomerDataModel()
            {
                Id = Guid.NewGuid(),
                Name = "John Smith",
                DNI = "123456",
                Phone = "1111",
                Mobile = "2222",
                Address = "Kennedy 123",
                City = "CityName",
                Email = "john@mail.com",
                State = "Texas"
            };
            CustomerDataModel customer_2 = new CustomerDataModel()
            {
                Id = new Guid("44ae1669-eb5d-4cc6-a960-70bf06ff0099"),
                Name = "Jennifer Stone",
                DNI = "987654",
                Phone = "3333",
                Mobile = "4444",
                Address = "Bush 321",
                City = "CityName",
                Email = "jenni@mail.com",
                State = "Orlando"
            };
            context.Customer.Add(customer_1);
            context.Customer.Add(customer_2);
            context.SaveChanges();
        }


        public void Dispose()
        {
            context.Dispose();
        }
    }
}
