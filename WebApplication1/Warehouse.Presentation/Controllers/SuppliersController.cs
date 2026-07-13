using MediatR;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Application.Suppliers.Commands;
using Warehouse.Application.Suppliers.Commands.CreateSupplier;
using Warehouse.Application.Suppliers.Commands.DeactivateSupplier;
using Warehouse.Application.Suppliers.Queries;
using Warehouse.Application.Suppliers.Queries.GetSupplierById;
using Warehouse.Application.Suppliers.Queries.ListSuppliers;

namespace Warehouse.Presentation.Controllers
{
    [ApiController]
    [Route("api/suppliers")]
    public class SuppliersController : ControllerBase
    {
        private readonly IMediator _mediator;


        public SuppliersController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // GET /api/suppliers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var suppliers =
                await _mediator.Send(
                    new ListSuppliersQuery()
                );


            return Ok(suppliers);
        }


        // GET /api/suppliers/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id)
        {
            var supplier =
                await _mediator.Send(
                    new GetSupplierByIdQuery(id)
                );


            if (supplier == null)
                return NotFound();


            return Ok(supplier);
        }


        // POST /api/suppliers
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateSupplierCommand command)
        {
            var response = await _mediator.Send(command);


            return CreatedAtAction(
                nameof(GetById),
                new { id = response.Id },
                response
            );
        }


        // DELETE /api/suppliers/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deactivate(
            Guid id)
        {
            var result =
                await _mediator.Send(
                    new DeactivateSupplierCommand(id)
                );


            if (!result.Success)
                return NotFound();


            return NoContent();
        }
    }
}