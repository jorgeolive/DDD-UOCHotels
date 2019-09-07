using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Queries;
using UOCHotels.RoomServiceManagement.Application.ReadModel;

namespace RoomServiceManagement.Api.Controllers
{
    [Route("api/rooms/")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        //Early on, I had the repositories collection around here. With mediator, I
        // can inyect the needed repositories in the command handlers, which is awesome! :)

        //This helps control the aggregate boundary for transactions as well. The aggregate
        //repository will be instantiated only one time on the command handler. 

        readonly IMediator _mediator;

        public RoomController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateRoom([FromBody] CreateRoomRequest command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            //Review : how to handle when the domain layer throws an exception?      
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoomModel>> GetRoomByAddress([FromBody] GetRoomByAddressQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            //Review : how to handle when the domain layer throws an exception?      
        }
    }
}
