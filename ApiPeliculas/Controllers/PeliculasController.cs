using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Models;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers;
[Route("api/peliculas")]
[ApiController]
public class PeliculasController : ControllerBase
{
    private readonly IPeliculaRepositorio _pelRepo;
    private readonly IMapper _mapper;
    public PeliculasController(IPeliculaRepositorio pelRepo, IMapper mapper)
    {
        _pelRepo = pelRepo;
        _mapper = mapper;
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetPeliculas()
    {
        var listaPeliculas = _pelRepo.GetPeliculas();

        var listaPeliculasDto = new List<PeliculaDto>();

        foreach (var lista in listaPeliculas)
        {
            listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(lista));
        }
        return Ok(listaPeliculasDto);
    }


    [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetPelicula(int peliculaId)
    {
        var itemPelicula = _pelRepo.GetPelicula(peliculaId);

        if (itemPelicula == null)
        {
            return NotFound();
        }

        var itempeliculaDto = _mapper.Map<PeliculaDto>(itemPelicula);


        return Ok(itempeliculaDto);
    }




    [HttpPost]
    [ProducesResponseType(201, Type = typeof(PeliculaDto))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CrearPelicula([FromBody] PeliculaDto crearPeliculaDto)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (crearPeliculaDto == null)
        {
            return BadRequest(ModelState);
        }
        if (_pelRepo.ExistePelicula(crearPeliculaDto.Nombre))
        {
            ModelState.AddModelError("", "La pelicula ya existe");
            return StatusCode(404, ModelState);
        }

        var pelicula = _mapper.Map<Pelicula>(crearPeliculaDto);
        if (!_pelRepo.CrearPelicula(pelicula))
        {
            ModelState.AddModelError("", $"Algo salio mal guardando el registro {pelicula.Nombre}");
            return StatusCode(500, ModelState);
        }

        return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);

    }


    [HttpPatch("{peliculaId:int}", Name = "ActualizarPatchPelicula")]
    [ProducesResponseType(204)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult ActualizarPatchPelicula(int peliculaId, [FromBody] PeliculaDto peliculaDto)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var pelicula = _mapper.Map<Pelicula>(peliculaDto);

        if (!_pelRepo.ActualizarPelicula(pelicula))
        {
            ModelState.AddModelError("", $"Algo salio mal actualizando el registro {pelicula.Nombre}");
            return StatusCode(500, ModelState);
        }

        return NoContent();

    }

    [HttpDelete("{peliculaId:int}", Name = "EliminarPelicula")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult EliminarPelicula(int peliculaId)
    {

        if (!_pelRepo.ExistePelicula(peliculaId))
        {
            return NotFound();
        }

        var pelicula = _pelRepo.GetPelicula(peliculaId);

        if (!_pelRepo.EliminarPelicula(pelicula))
        {
            ModelState.AddModelError("", $"Algo salio mal borrando el registro {pelicula.Nombre}");
            return StatusCode(500, ModelState);
        }

        return NoContent();

    }

    [HttpGet("GetPeliculasEnCategoria/{categoriaId:int}")]
    public IActionResult GetPeliculasEnCategoria(int categoriaId)
    {
        var listaPeliculas = _pelRepo.GetPeliculasEnCategoria(categoriaId);

        if (listaPeliculas == null)
        {
            return NotFound();
        }

        var itemPelicula = new List<PeliculaDto>();

        foreach (var item in listaPeliculas)
        {
            itemPelicula.Add(_mapper.Map<PeliculaDto>(item));
        }
        return Ok(itemPelicula);
    }



    [HttpGet("Buscar")]
    public IActionResult Buscar(string nombre)
    {

        try
        {
            var resultado = _pelRepo.BuscarPelicula(nombre.Trim());
            if (resultado.Any())
            {
                return Ok(resultado);
            }
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
        }

        
    }


}
