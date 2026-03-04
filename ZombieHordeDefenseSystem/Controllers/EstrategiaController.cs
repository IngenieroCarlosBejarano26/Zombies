using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZombieHordeDefenseSystem.Application.DTOs;
using ZombieHordeDefenseSystem.Application.Querys;

namespace ZombieHordeDefenseSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstrategiaController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("optimal-strategy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CalcularEstrategia([FromQuery] int balasDisponibles, [FromQuery] int tiempoDisponible)
        {
            CalcularEstrategiaRespuestaDTO mejorEstrategia = await _mediator.Send(new CalcularEstrategiaQuery(balasDisponibles, tiempoDisponible));
            return Ok(mejorEstrategia);
        }
    }
}