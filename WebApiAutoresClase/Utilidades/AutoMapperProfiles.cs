using AutoMapper;
using WebApiAutoresClase.DTOs;
using WebApiAutoresClase.Models;

namespace WebApiAutoresClase.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(x => x.Libros, opciones => opciones.MapFrom(MapAutorDTOLibros));
        }
        private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDTO)
        {
            var lista = new List<LibroDTO>();
            if (autor.AutorLibro == null)
            {
                return lista;
            }

            foreach (var autorLibro in autor.AutorLibro)
            {
                lista.Add(new LibroDTO()
                {
                    Id = autorLibro.LibroId,
                    Titulo = autorLibro.Libro.Titulo
                });
            }
            return lista;
        }
    }
}
