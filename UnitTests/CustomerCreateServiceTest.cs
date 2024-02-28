using EFCoreInMemory.DatabaseContext;
using EFCoreInMemory.DataModel;
using Microsoft.EntityFrameworkCore;
using EFCoreInMemory.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NuGet.Frameworks;

namespace UnitTests
{
    public class CustomerCreateServiceTest : IClassFixture<TestDataFixture>
    {
        private DataContext _storeAppContext;

        public CustomerCreateServiceTest(TestDataFixture fixture)
        {
            _storeAppContext = fixture.context;
        }

        [Fact]
        public async void GetAll_Customer_Success()
        {
            var customerController = new CustomerController(_storeAppContext);

            //Act
            var result = await customerController.GetAll();
            var resultType = result as OkObjectResult;
            var resultList = resultType.Value as List<CustomerDataModel>;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<CustomerDataModel>>(resultType.Value);
            Assert.Equal(2, resultList.Count);
        }

        [Fact]
        public async void Get_Customer_Success()
        {
            var customerController = new CustomerController(_storeAppContext);
            Guid valid_id = new Guid("44ae1669-eb5d-4cc6-a960-70bf06ff0099");

            //Act
            var result = await customerController.Get(valid_id);
            var resultType = result as OkObjectResult;
            var fetchedItem = resultType.Value as CustomerDataModel;

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(valid_id, fetchedItem.Id);
        }

        [Fact]
        public async void Get_Customer_NotFound()
        {
            var customerController = new CustomerController(_storeAppContext);
            Guid valid_id = new Guid("44ae1669-eb5d-4cc6-a960-70bf06ff0000");

            //Act
            var result = await customerController.Get(valid_id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Customer_Success()
        {

            // Act
            var customerController = new CustomerController(_storeAppContext);
            var toBeAdded = GetCustomerCreate();
            var customer = await customerController.Create(toBeAdded);

            // Assert
            var resultType = customer as OkObjectResult;
            var customerItem = resultType.Value as CustomerDataModel;
            Assert.IsType<OkObjectResult>(resultType);
            Assert.IsType<CustomerDataModel>(resultType.Value);

            Assert.Equal(toBeAdded.Name, customerItem.Name);
            Assert.Equal(toBeAdded.Address, customerItem.Address);
            Assert.Equal(toBeAdded.City, customerItem.City);
            Assert.Equal(toBeAdded.DNI, customerItem.DNI);
            Assert.Equal(toBeAdded.Email, customerItem.Email);
            Assert.Equal(toBeAdded.Phone, customerItem.Phone);
            Assert.Equal(toBeAdded.State, customerItem.State);
        }

        [Fact]
        public async void Delete_Customer_Success()
        {
            // Act
            var customerController = new CustomerController(_storeAppContext);
            Guid valid_id = new Guid("44ae1669-eb5d-4cc6-a960-70bf06ff0099");

            var successResult = await customerController.Delete(valid_id);

            var actionResult = Assert.IsType<OkObjectResult>(successResult);
            var returnValue = Assert.IsType<CustomerDataModel>(actionResult.Value);
        }


        private CustomerCreateDataModel GetCustomerCreate()
        {
            return new CustomerCreateDataModel
            {
                Name = "Simon",
                Address = "Address",
                City = "City",
                DNI = "788999",
                Email = "test@test.com",
                Mobile = "234234234",
                Phone = "4455543222",
                State = "state",
            };
        }
    }
}
