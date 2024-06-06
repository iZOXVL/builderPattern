namespace Api.Microeservice.Autor.Aplicacion
{
    public interface IAutorDto
    {
        int AutorLibroId { get; set; }
        string Nombre { get; set; }
        string Apellido { get; set; }
        DateTime? FechaNacimiento { get; set; }
        string AutorLibroGuid { get; set; }
    }
}
