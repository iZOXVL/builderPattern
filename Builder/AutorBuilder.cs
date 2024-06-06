using Api.Microeservice.Autor.Aplicacion;
using Api.Microeservice.Autor.Modelo;
using Api.Microeservice.Autor.Persistencia;
using Api.Microservice.Autor.Aplicacion;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Microservice.Autor.Aplicacion
{
    public class AutorBuilder : IBuilder
    {
        private AutorDto _autorDto;
        private readonly ContextoAutor _context;

        public AutorBuilder(ContextoAutor context)
        {
            _context = context;
            Reset();
        }

        public void Reset()
        {
            _autorDto = new AutorDto();
        }

        public AutorDto Build()
        {
            AutorDto autor = _autorDto;
            Reset();
            return autor;
        }

        public List<AutorDto> Consulta()
        {
            var autores = _context.AutorLibros.ToList();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<AutorLibro, AutorDto>()).CreateMapper();
            return mapper.Map<List<AutorLibro>, List<AutorDto>>(autores);
        }

        public AutorDto ConsultarFiltro(string autorGuid)
        {
            var autor = _context.AutorLibros.FirstOrDefault(p => p.AutorLibroGuid == autorGuid);
            if (autor == null)
            {
                throw new Exception("No se encontró el autor");
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<AutorLibro, AutorDto>()).CreateMapper();
            return mapper.Map<AutorLibro, AutorDto>(autor);
        }

        public IBuilder SetAutorLibroId(int autorLibroId)
        {
            _autorDto.AutorLibroId = autorLibroId;
            return this;
        }

        public IBuilder SetNombre(string nombre)
        {
            _autorDto.Nombre = nombre;
            return this;
        }

        public IBuilder SetApellido(string apellido)
        {
            _autorDto.Apellido = apellido;
            return this;
        }

        public IBuilder SetFechaNacimiento(DateTime? fechaNacimiento)
        {
            _autorDto.FechaNacimiento = fechaNacimiento;
            return this;
        }

        public IBuilder SetAutorLibroGuid(string autorLibroGuid)
        {
            _autorDto.AutorLibroGuid = autorLibroGuid;
            return this;
        }
    }
}
