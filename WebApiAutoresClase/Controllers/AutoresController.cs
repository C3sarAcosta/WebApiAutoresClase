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

        [HttpGet("{id:int}", Name = "obtenerAutor")]
        public async Task<ActionResult<AutorDTOConLibros>> Get(int id)
        {
            //var autor = await _context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            //var augtores2 = await _context.Autores.Where(x => x.Nombre.Contains(nombre)).ToListAsync();
            var autor = await _context.Autores
                .Include(x => x.AutorLibro) //Traemos la tabla autorLibro
                .ThenInclude(x => x.Libro) //Traemos la tabla libro
                .FirstOrDefaultAsync(x => x.Id == id);
            if(autor == null)
            {
                return NotFound(); 
            }

            return _mapper.Map<AutorDTOConLibros>(autor);
        }

        [HttpPut("{id:int}")] 
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id)
        {
            var existe =  await _context.Autores.AnyAsync(x => x.Id == id);
            if(!existe)
            {
                return NotFound();
            }
            var autor = _mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;
            _context.Update(autor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _context.Autores.AnyAsync(x => x.Id == id);
            if(!existe)
            {
                return NotFound();
            }
            _context.Remove(new Autor { Id = id });
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
