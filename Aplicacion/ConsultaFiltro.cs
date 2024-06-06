using Api.Microeservice.Autor.Aplicacion;
using Api.Microeservice.Autor.Modelo;
using Api.Microeservice.Autor.Persistencia;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Microservice.Autor.Aplicacion
{
    public class ConsultarFiltro
    {
        public class AutorUnico : IRequest<AutorDto>
        {
            public string? AutorGuid { get; set; }
        }

        public class Manejador : IRequestHandler<AutorUnico, AutorDto>
        {
            private readonly IBuilder _builder;

            public Manejador(IBuilder builder)
            {
                _builder = builder;
            }
            public async Task<AutorDto> Handle(AutorUnico request, CancellationToken cancellationToken)
            {
                return _builder.ConsultarFiltro(request.AutorGuid);
            }
        }
    }
}
