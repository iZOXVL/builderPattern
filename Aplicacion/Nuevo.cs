using Api.Microeservice.Autor.Modelo;
using Api.Microeservice.Autor.Persistencia;
using FluentValidation;
using MediatR;
using Google.Protobuf;
using Grpc.Net.Client;
using ImagenService;
using Api.Microeservice.Autor.Aplicacion;

namespace Api.Microservice.Autor.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public DateTime? FechaNacimiento { get; set; }
            public string ImagenBase64 { get; set; } // Nueva propiedad para la imagen en base64
        }

        public class EjecutarValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutarValidacion()
            {
                RuleFor(p => p.Nombre).NotEmpty();
                RuleFor(p => p.Apellido).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IBuilder _builder;
            private readonly ContextoAutor _context;

            public Manejador(IBuilder builder, ContextoAutor context)
            {
                _builder = builder;
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var guid = Guid.NewGuid().ToString();

                _builder
                    .SetNombre(request.Nombre)
                    .SetApellido(request.Apellido)
                    .SetFechaNacimiento(request.FechaNacimiento)
                    .SetAutorLibroGuid(guid);

                var autorLibro = _builder.Build();

                _context.AutorLibros.Add(new AutorLibro
                {
                    Nombre = autorLibro.Nombre,
                    Apellido = autorLibro.Apellido,
                    FechaNacimiento = autorLibro.FechaNacimiento,
                    AutorLibroGuid = autorLibro.AutorLibroGuid
                });

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    
                    using var channel = GrpcChannel.ForAddress("http://localhost:5000"); 
                    var client = new ImagenService.ImagenService.ImagenServiceClient(channel);

                    var requestGrpc = new UploadImageRequest
                    {
                        AutorLibroGuid = autorLibro.AutorLibroGuid,
                        Imagen = ByteString.CopyFrom(Convert.FromBase64String(request.ImagenBase64))
                    };

                    var responseGrpc = await client.UploadImageAsync(requestGrpc);

                    if (responseGrpc.Message == "Imagen almacenada correctamente")
                    {
                        return Unit.Value;
                    }
                    else
                    {
                        throw new Exception("Error al guardar la imagen en MongoDB");
                    }
                }

                throw new Exception("No se pudo insertar el Autor del libro");
            }
        }
    }
}
