using ApiPeliculas.Models;

namespace ApiPeliculas.Repositorio.IRepositorio;

public interface IPeliculaRepositorio
{
    ICollection<Pelicula> GetPeliculas();
    Pelicula GetPelicula(int peliculaId);
    bool ExistePelicula(string nombre);
    bool ExistePelicula(int id);
    bool CrearPelicula(Pelicula pelicula);
    bool ActualizarPelicula(Pelicula pelicula);
    bool EliminarPelicula(Pelicula pelicula);

    //Metodos para buscar peliculas por categoria y nombre
    ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId);
    ICollection<Pelicula> BuscarPelicula(string nombre);

    bool Guardar();
}

