namespace WebApiAutoresClase.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        //Relacion muchos a muchos
        public List<AutorLibro> AutorLibro { get; set; }
    }
}
