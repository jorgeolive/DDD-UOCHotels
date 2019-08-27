using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Dto;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Application.Queries;
using UOCHotels.RoomServiceManagement.Domain;
using UOCHotels.RoomServiceManagement.Domain.Infraestructure;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

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

        [HttpGet("[controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomServiceDto>> Get(Guid serviceId)
        {
            var roomServiceDto = await _mediator.Send<RoomServiceDto>(new GetRoomServiceByIdQuery(serviceId));

            if (roomServiceDto != null)
            {
                return roomServiceDto;
            }

            return NotFound();
        }

        [HttpGet("[controller]/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RoomService>>> GetByRoomId([FromQuery]Guid roomId)
        {
            //var roomService = await _roomServiceRepository.GetByRoomId(new RoomId(roomId));

            //if (roomService.Any())
            //{
            //    return roomService.ToList();
            //}

            return NotFound();
        }

        [HttpPost("[controller]/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateRoomService([FromBody] Guid roomId)
        {
            try
            {
                await _mediator.Send(new CreateRoomServiceCommand(roomId));
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
