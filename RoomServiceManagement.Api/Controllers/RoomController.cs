using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Dto;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Application.Queries;
using UOCHotels.RoomServiceManagement.Domain;
using UOCHotels.RoomServiceManagement.Domain.Infraestructure;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

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

        [HttpPost("[controller]/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateRoom([FromBody] CreateRoomCommand command)
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
    }
}
