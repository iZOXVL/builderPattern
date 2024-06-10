using Api.Microeservice.Autor.Aplicacion;
using Api.Microservice.Autor.Aplicacion;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Microeservice.Autor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AutorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            var result = await _mediator.Send(data);

            if (result == Unit.Value)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Error al guardar el autor");
            }
        }
        //get
        [HttpGet]
        public async Task<ActionResult<List<AutorDto>>> GetAutores()
        {
            var autores = await _mediator.Send(new Consulta.ListaAutor());
            return Ok(autores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AutorDto>> GetAutorLibro(string id)
        {
            return await _mediator.Send(new ConsultarFiltro.AutorUnico { AutorGuid = id });
        }
    }
}
