namespace Api.Microeservice.Autor.Aplicacion
{
    public interface IBuilder
    {
        void Reset();
        AutorDto Build();
        List<AutorDto> Consulta();
        AutorDto ConsultarFiltro(string autorGuid);
        IBuilder SetAutorLibroId(int autorLibroId);
        IBuilder SetNombre(string nombre);
        IBuilder SetApellido(string apellido);
        IBuilder SetFechaNacimiento(DateTime? fechaNacimiento);
        IBuilder SetAutorLibroGuid(string autorLibroGuid);
    }
}
