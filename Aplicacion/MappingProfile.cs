using Api.Microeservice.Autor.Modelo;
using Api.Microservice.Autor.Aplicacion;
using AutoMapper;

namespace Api.Microeservice.Autor.Aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AutorLibro, AutorDto>();
        }
    }
}
