namespace WebApiAutoresClase.Models
{
    public class AutorLibro
    {
        //Llave primaria compuesta configurada en el DbContext
        public int LibroId { get; set; }
        public int AutorId { get; set; }
        //Relacion muchos a muchos
        public Libro Libro { get; set; }
        public Autor Autor { get; set; }
    }
}
