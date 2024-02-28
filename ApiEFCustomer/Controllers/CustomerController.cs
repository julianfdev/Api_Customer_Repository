using EFCoreInMemory.DatabaseContext;
using EFCoreInMemory.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EFCoreInMemory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DataContext _databaseContext;

        public CustomerController(DataContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        //[Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_databaseContext.Customer.ToList());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(204, Type = typeof(CustomerDataModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid id)
        {
            if (!await _databaseContext.Customer.AnyAsync(c => c.Id == id))
                return NotFound();

            var customer = await _databaseContext.Customer.FindAsync(id);

            return Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(204, Type = typeof(CustomerDataModel))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CustomerCreateDataModel customerCreate)
        {
            if (customerCreate == null)
                return BadRequest(ModelState);

            if (customerCreate.DNI == null)
                return BadRequest(ModelState);

            var customerInDb = await _databaseContext.Customer.AsNoTracking().FirstOrDefaultAsync(
                c => c.DNI.Trim().ToLower() == customerCreate.DNI.Trim().ToLower()
            );

            if (customerInDb != null)
            {
                ModelState.AddModelError("", "Customer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            CustomerDataModel customer = new CustomerDataModel();
            customer.Id = Guid.NewGuid();
            customer.Name = customerCreate.Name ?? "";
            customer.DNI = customerCreate.DNI ?? "";
            customer.Address = customerCreate.Address ?? "";
            customer.Phone = customerCreate.Phone ?? "";
            customer.Mobile = customerCreate.Mobile ?? "";
            customer.Email = customerCreate.Email ?? "";
            customer.State = customerCreate.State ?? "";
            customer.City = customerCreate.City ?? "";

            _databaseContext.Customer.Add(customer);
            await _databaseContext.SaveChangesAsync();

            return Ok(customer); //CreatedAtAction("Get",customer);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204, Type = typeof(CustomerDataModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Edit(Guid id, [FromBody] CustomerCreateDataModel customerUpdated)
        {
            if (customerUpdated == null)
                return BadRequest(ModelState);

            if (!await _databaseContext.Customer.AnyAsync(c => c.Id == id))
                return NotFound();

            CustomerDataModel customer = new CustomerDataModel
            {
                Id = id,
                Name = customerUpdated.Name ?? "",
                DNI = customerUpdated.DNI ?? "",
                Address = customerUpdated.Address ?? "",
                Phone = customerUpdated.Phone ?? "",
                Mobile = customerUpdated.Mobile ?? "",
                Email = customerUpdated.Email ?? "",
                State = customerUpdated.State ?? "",
                City = customerUpdated.City ?? ""
            };

            _databaseContext.Customer.Update(customer);
            var saved = await _databaseContext.SaveChangesAsync();

            if (saved <= 0) {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }
            return Ok(customer);
           
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _databaseContext.Customer.AnyAsync(c => c.Id == id))
                return NotFound();

            CustomerDataModel customer = await _databaseContext.Customer.FindAsync(id);
            _databaseContext.Customer.Remove(customer);
            await _databaseContext.SaveChangesAsync();

            return Ok(customer);
        }
    }
}
