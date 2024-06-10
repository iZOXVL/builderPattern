using Api.Microeservice.Autor.Modelo;
using Api.Microeservice.Autor.Persistencia;
using AutoMapper;
using Grpc.Net.Client;
using ImagenService;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Api.Microeservice.Autor.Aplicacion;

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
            private readonly ContextoAutor _context;
            private readonly IMapper _mapper;

            public Manejador(IBuilder builder, ContextoAutor context, IMapper mapper)
            {
                _builder = builder;
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<AutorDto>> Handle(ListaAutor request, CancellationToken cancellationToken)
            {
      
                var autores = _builder.Consulta();

             
                using var channel = GrpcChannel.ForAddress("http://localhost:5000"); 
                var client = new ImagenService.ImagenService.ImagenServiceClient(channel);

                var responseGrpc = await client.GetAllImagesAsync(new GetAllImagesRequest());

              
                var imagenesDic = responseGrpc.Images.ToDictionary(img => img.AutorLibroGuid, img => img.Imagen.ToBase64());

             
                var autoresConImagen = new List<AutorDto>();

                foreach (var autor in autores)
                {
                    string imagenBase64 = null;
                    if (imagenesDic.ContainsKey(autor.AutorLibroGuid))
                    {
                        imagenBase64 = imagenesDic[autor.AutorLibroGuid];
                    }

                    var autorConImagen = new AutorDto.Builder()
                        .SetAutorLibroId(autor.AutorLibroId)
                        .SetNombre(autor.Nombre)
                        .SetApellido(autor.Apellido)
                        .SetFechaNacimiento(autor.FechaNacimiento)
                        .SetAutorLibroGuid(autor.AutorLibroGuid)
                        .SetImagenBase64(imagenBase64)
                        .Build();

                    autoresConImagen.Add(autorConImagen);
                }

                return autoresConImagen;
            }
        }
    }
}
