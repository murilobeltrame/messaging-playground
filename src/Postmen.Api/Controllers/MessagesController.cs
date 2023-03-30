using Application.Messages.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Postmen.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Send(
            [FromServices] IMediator mediator,
            [FromBody] CreateMessageCommand request)
        {
            await mediator.Send(request);
            return Ok();
        }
    }
}
