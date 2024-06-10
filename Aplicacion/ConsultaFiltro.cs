using Api.Microeservice.Autor.Aplicacion;
using Api.Microeservice.Autor.Modelo;
using Api.Microeservice.Autor.Persistencia;
using AutoMapper;
using Grpc.Net.Client;
using ImagenService;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

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
                var autor = _builder.ConsultarFiltro(request.AutorGuid);

                if (autor == null)
                {
                    throw new KeyNotFoundException("Autor no encontrado");
                }

                
                using var channel = GrpcChannel.ForAddress("http://localhost:5000"); 
                var client = new ImagenService.ImagenService.ImagenServiceClient(channel);

                var imageResponse = await client.GetImageAsync(new GetImageRequest { AutorLibroGuid = autor.AutorLibroGuid });

                if (imageResponse != null && imageResponse.Imagen.Length > 0)
                {
                    autor.ImagenBase64 = imageResponse.Imagen.ToBase64();
                }
                else
                {
                    autor.ImagenBase64 = null;
                }

                return autor;
            }
        }
    }
}
