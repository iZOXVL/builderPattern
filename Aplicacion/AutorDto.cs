using Api.Microeservice.Autor.Aplicacion;

namespace Api.Microeservice.Autor.Aplicacion
{
    /// <summary>
    /// Entidad del tipo AutorDto
    /// </summary>
    public class AutorDto : IAutorDto
    {
        public int AutorLibroId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string AutorLibroGuid { get; set; }

        // Implementación del patrón Builder
        public class Builder
        {
            private readonly AutorDto _autorDto;

            public Builder()
            {
                _autorDto = new AutorDto();
            }

            public Builder SetAutorLibroId(int autorLibroId)
            {
                _autorDto.AutorLibroId = autorLibroId;
                return this;
            }

            public Builder SetNombre(string nombre)
            {
                _autorDto.Nombre = nombre;
                return this;
            }

            public Builder SetApellido(string apellido)
            {
                _autorDto.Apellido = apellido;
                return this;
            }

            public Builder SetFechaNacimiento(DateTime? fechaNacimiento)
            {
                _autorDto.FechaNacimiento = fechaNacimiento;
                return this;
            }

            public Builder SetAutorLibroGuid(string autorLibroGuid)
            {
                _autorDto.AutorLibroGuid = autorLibroGuid;
                return this;
            }

            public AutorDto Build()
            {
                return _autorDto;
            }
        }
    }
}
