using ApiPeliculas.Data;
using ApiPeliculas.Repositorio;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using ApiPeliculas.PeliculasMapper;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

//Configuramos la conexion a sql server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql"));
});

//Añadimos cache
builder.Services.AddResponseCaching();

//Agregamos los repositorios
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();

//Agregar el AutoMapper
builder.Services.AddAutoMapper(typeof(PeliculasMapper));

// Add services to the container.

builder.Services.AddControllers(opcion =>
{
    //Cache profile. Un cache global
    opcion.CacheProfiles.Add("PorDefecto30Segundos", new CacheProfile() { Duration = 30});
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Soporte para CORS
//Habilitamos un dominio o multiples dominios o cualquier dominio (ojo con la seguridad)
//Ejemplo: http://localhost:4200, es el puerto que levanta Angular
//(*) para todos los dominios
builder.Services.AddCors(p => p.AddPolicy("PolicyCors", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/apiPeliculas/swagger/ApiPeliculasCategorias/swagger.json", "API Categorias Peliculas");
        options.SwaggerEndpoint("/apiPeliculas/swagger/ApiPeliculas/swagger.json", "API Peliculas");
        options.RoutePrefix = "";
    });
}

app.UseHttpsRedirection();

//Soporte para CORS
//Mismo nombre que AddPolicy
app.UseCors("PolicyCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
