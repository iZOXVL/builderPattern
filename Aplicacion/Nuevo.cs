using Api.Microeservice.Autor.Aplicacion;
using Api.Microeservice.Autor.Modelo;
using Api.Microeservice.Autor.Persistencia;
using FluentValidation;
using MediatR;

namespace Api.Microservice.Autor.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string? Nombre { get; set; }
            public string? Apellido { get; set; }
            public DateTime? FechaNacimiento { get; set; }
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
                _builder
                    .SetNombre(request.Nombre)
                    .SetApellido(request.Apellido)
                    .SetFechaNacimiento(request.FechaNacimiento)
                    .SetAutorLibroGuid(Guid.NewGuid().ToString());

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
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el Autor del libro");
            }
        }
    }
}
