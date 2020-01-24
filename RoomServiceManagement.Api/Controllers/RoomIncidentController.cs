using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UOCHotels.RoomServiceManagement.Application.Commands;

namespace RoomServiceManagement.Api.Controllers
{
    [Authorize]
    [Route("api/roomincidents/")]
    [ApiController]
    public class RoomIncidentController : ControllerBase
    {
        readonly IMediator _mediator;

        public RoomIncidentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateRoomIncident([FromBody] AddRoomIncidentRequest request)
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

        [HttpPost("{roomIncidentId:Guid}/pictures")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddPicture(IFormFile file, Guid roomIncidentId)
        {
            try
            {
                await _mediator.Send(new AddRoomIncidentPictureRequest()
                {
                  File = file,
                  RoomIncidentId = roomIncidentId
                });

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
