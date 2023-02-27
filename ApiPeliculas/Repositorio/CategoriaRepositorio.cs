using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repositorio.IRepositorio;

namespace ApiPeliculas.Repositorio;

public class CategoriaRepositorio : ICategoriaRepositorio
{

    private readonly ApplicationDbContext _bd;

    public CategoriaRepositorio(ApplicationDbContext bd)
    {
        _bd = bd;
    }

    public bool ActualizarCategoria(Categoria categoria)
    {
        categoria.FechaCreacion = DateTime.Now;
        _bd.Categoria.Update(categoria);
        return Guardar();
    }


    public bool CrearCategoria(Categoria categoria)
    {
        categoria.FechaCreacion = DateTime.Now;
        _bd.Categoria.Add(categoria);
        return Guardar();
    }

    public bool EliminarCategoria(Categoria categoria)
    {
        _bd.Categoria.Remove(categoria);
        return Guardar();
    }

    public bool ExisteCategoria(string nombre)
    {
        bool valor = _bd.Categoria.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
        return valor;
    }

    public bool ExisteCategoria(int id)
    {
        bool valor = _bd.Categoria.Any(c => c.Id == id);
        return valor;
    }

    public Categoria GetCategoria(int categoriaId)
    {
        return _bd.Categoria.FirstOrDefault(c => c.Id == categoriaId);
    }

    public ICollection<Categoria> GetCategorias()
    {
        return _bd.Categoria.OrderBy(c => c.Nombre).ToList();
    }

    public bool Guardar()
    {
        return _bd.SaveChanges() >= 0 ? true : false;
    }
}
