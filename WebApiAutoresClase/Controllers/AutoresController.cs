using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutoresClase.Data;
using WebApiAutoresClase.DTOs;
using WebApiAutoresClase.Models;

namespace WebApiAutoresClase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AutoresController(ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {
            var existeAutor = await _context.Autores.AnyAsync(x => x.Nombre == autorCreacionDTO.Nombre);
            if (existeAutor)
            {
                return BadRequest($"Autor {autorCreacionDTO.Nombre} duplicado");
            }
            var autor = _mapper.Map<Autor>(autorCreacionDTO);
            _context.Add(autor);
            await _context.SaveChangesAsync();

            var autorDTO = _mapper.Map<AutorDTO>(autor);
            return Ok(autorDTO);
        }

        [HttpGet("primero")]
        public async Task<ActionResult<AutorDTO>> PrimerAutor()
        {
            var autor = await _context.Autores.FirstOrDefaultAsync();
            return _mapper.Map<AutorDTO>(autor);
        }

        [HttpGet("{id:int}/{nombre?}", Name = "obtenerAutor")]
        public async Task<ActionResult<AutorDTO>> Get(int id, string nombre)
        {
            var autor = await _context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if(autor == null)
            {
                return NotFound(); 
            }

            return _mapper.Map<AutorDTO>(autor);
        }
    }
}
