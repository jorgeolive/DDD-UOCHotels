using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Application.Queries;
using UOCHotels.RoomServiceManagement.Application.ReadModel;

namespace RoomServiceManagement.Api.Controllers
{
    [Route("api/roomservices/")]
    [ApiController]
    public class RoomServicesController : ControllerBase
    {
        //Early on, I had the repositories collection around here. With mediator, I
        // can inyect the needed repositories in the command handlers, which is awesome! :)

        //This helps control the aggregate boundary for transactions as well. The aggregate
        //repository will be instantiated only one time on the command handler. 

        readonly IMediator _mediator;

        public RoomServicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{roomServiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomServiceModel>> Get(Guid roomServiceId)
        {
            var roomService = await _mediator.Send<RoomServiceModel>(new GetRoomServiceByIdQuery(roomServiceId));

            if (roomService != null)
            {
                return roomService;
            }

            return NotFound();
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RoomServiceModel>>> GetByRoomId([FromQuery]Guid roomId)
        {
            var results = await _mediator.Send(new GetRoomServicesByRoomIdQuery(roomId));

            if (results.Any())
            {
                return results.ToList();
            }

            return NotFound();
        }

        //Server assigns identity, so it's a post(NOT idempotent)
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateRoomService([FromBody] CreateRoomServiceRequest request)
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

        [HttpPost("{roomServiceId:Guid}/plan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PlanRoomService(Guid roomServiceId, [FromBody] PlanRoomServiceRequest request)
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

        [HttpPost("{roomServiceId:Guid}/start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> StartRoomService(Guid roomServiceId, [FromBody] StartRoomServiceRequest request)
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

        [HttpPost("{roomServiceId:Guid}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult> AddComment(Guid roomServiceId, [FromBody] AddCommentRequest request)
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
    }
}
