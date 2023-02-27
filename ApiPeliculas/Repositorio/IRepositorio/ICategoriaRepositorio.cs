using ApiPeliculas.Models;

namespace ApiPeliculas.Repositorio.IRepositorio;

public interface ICategoriaRepositorio
{
    ICollection<Categoria> GetCategorias();
    Categoria GetCategoria(int categoriaId);
    bool ExisteCategoria(string nombre);
    bool ExisteCategoria(int id);
    bool CrearCategoria(Categoria categoria);
    bool ActualizarCategoria(Categoria categoria);
    bool EliminarCategoria(Categoria categoria);
    bool Guardar();
}

