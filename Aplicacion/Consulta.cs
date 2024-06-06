using Api.Microeservice.Autor.Aplicacion;
using Api.Microeservice.Autor.Modelo;
using Api.Microeservice.Autor.Persistencia;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Microservice.Autor.Aplicacion
{
    public class Consulta
    {
        public class ListaAutor : IRequest<List<AutorDto>>
        {
        }

        public class Manejador : IRequestHandler<ListaAutor, List<AutorDto>>
        {
            private readonly IBuilder _builder;
            public Manejador(IBuilder builder)
            {
                _builder = builder;
            }
            public async Task<List<AutorDto>> Handle(ListaAutor request, CancellationToken cancellationToken)
            {
                return _builder.Consulta();
            }
        }
    }
}
