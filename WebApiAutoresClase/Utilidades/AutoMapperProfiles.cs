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
        }
    }
}
