using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApplication.ModelsAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MoviesController : ControllerBase
    {
        private static List<Movie> movies = new List<Movie>()
        { 
            new Movie() { ID = Guid.NewGuid(), Name = "Game of Thrones" },
            new Movie() { ID = Guid.NewGuid(), Name = "Star Wars" },
            new Movie() { ID = Guid.NewGuid(), Name = "The class" },
            new Movie() { ID = Guid.NewGuid(), Name = "Predator" },
            new Movie() { ID = Guid.NewGuid(), Name = "Rocky" }
        };
       
        
    /*    public MoviesController()
        {
            GetSampleMovies();
        }

        public IEnumerable<Movie> Get()
        {
            return movies;
        }
    */
    [HttpGet("{ID}")]
        public async Task<Movie> GetByID(Guid id)
        {
            var movie = movies.FirstOrDefault(m => m.ID == id);
            
            return movie;
        }

     /*   private void GetSampleMovies()
        {
            movies.Add(new Movie() { ID = Guid.NewGuid(), Name = "Game of Thrones" });
            movies.Add(new Movie() { ID = Guid.NewGuid(), Name = "Star Wars" });
            movies.Add(new Movie() { ID = Guid.NewGuid(), Name = "The class" });
            movies.Add(new Movie() { ID = Guid.NewGuid(), Name = "Predator" });
            movies.Add(new Movie() { ID = Guid.NewGuid(), Name = "Rocky" });
        }*/

        [HttpGet]
        public Movie[] Get()
        {
            
            return movies.ToArray();
        }

        [HttpPost]
        public void Post([FromBody] Movie movie)
        {
            if (movie.ID == Guid.Empty)
                movie.ID = Guid.NewGuid();

            movies.Add(movie);
        }
        [HttpPut]
        public void Put([FromBody] Movie movie)
        {
            Movie currentMovie = movies.FirstOrDefault(x => x.ID == movie.ID);
            currentMovie.Name = movie.Name;
        }


        [HttpDelete("{ID}")]
        public void Delete(Guid id)
        {
            movies.RemoveAll(movie => movie.ID == id);

            
        }

    }

}