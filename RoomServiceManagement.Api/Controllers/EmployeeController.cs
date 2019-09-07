using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Queries;
using UOCHotels.RoomServiceManagement.Application.ReadModel;
using UOCHotels.RoomServiceManagement.Domain;

namespace RoomServiceManagement.Api.Controllers
{
    [Route("api/employees/")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        //Early on, I had the repositories collection around here. With mediator, I
        // can inyect the needed repositories in the command handlers, which is awesome! :)

        //This helps control the aggregate boundary for transactions as well. The aggregate
        //repository will be instantiated only one time on the command handler. 

        readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //Just for testing, employees should be fed by events in other bounded context.
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateEmployee([FromBody] AddEmployeeRequest request)
        {
            try
            {
                await _mediator.Send(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            //Review : how to handle when the domain layer throws an exception?      
        }

        [HttpGet("{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeModel>> Get(Guid employeeId)
        {
            var RoomServiceModel = await _mediator.Send<EmployeeModel>(new GetEmployeeByIdQuery(employeeId));

            if (RoomServiceModel != null)
            {
                return RoomServiceModel;
            }

            return NotFound();
        }
    }
}
