using Microsoft.AspNetCore.Mvc;
using MoviesApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MoviesApplicationMVC.Controllers
{
    public class MVCMovieController : Controller
    {
        public List<Movie> movies = new List<Movie>();
        public IActionResult Index()
        {
            var movies = GetMoviesFromAPI();

            return View(movies);
        }
        private object GetMoviesFromAPI()
        {
           try
            {
                var resultList = new List<Movie>();

                var client = new HttpClient();
                var getDataTask = client.GetAsync("https://localhost:44312/api/Movies")
                    .ContinueWith(response =>
                    {
                        var result = response.Result;
                        if(result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var readResult = result.Content.ReadAsAsync<List<Movie>>();
                            readResult.Wait();
                            resultList = readResult.Result;
                        }
                    });
                getDataTask.Wait();
                movies = resultList;
                return resultList;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public object GetByID(Guid? id)
        { try
            {
                Movie resultMovie = new Movie();
                var client = new HttpClient();
                var getDataTask = client.GetAsync("https://localhost:44312/api/Movies" + id)
                    .ContinueWith(response =>
                    {
                        var result = response.Result;
                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var readResult = result.Content.ReadAsAsync<Movie>();
                            readResult.Wait();
                            resultMovie = readResult.Result;
                        }
                    });
                getDataTask.Wait();
                return resultMovie;
            }
            catch(Exception ex)
            {
                throw;
            }
            }
        [HttpPut]
        public async Task<object> Details(Guid id)
        {
            try
            {
                Movie movie = new Movie();
                var client = new HttpClient();
                var getDataTask = await client.GetAsync("https://localhost:44312/api/Movies" + id);
                {
                    movie = (Movie)GetByID(id);
                }
                return View(movie);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
          public IActionResult Details(Guid? id)
          {


              if (id == null)
              {
                  return NotFound();
              }

              Movie movie = (Movie)GetByID(id); 
            Console.Out.Write((Movie)GetByID(id));
              if (movie == null)
              {
                  return NotFound();
              }

              return View(movie);
          }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public object Create(Movie movie)
        {
            try
            {
               
                var client = new HttpClient();

                StringContent content = new StringContent(JsonConvert.SerializeObject(movie),
                    Encoding.UTF8,"application/json");

                var getDataTask = client.PostAsync("https://localhost:44312/api/Movies", content)
                    .ContinueWith(response =>
                    {
                        var result = response.Result;
                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                               var readResult = result.Content.ReadAsAsync<Movie>();
                               readResult.Wait();
                              
                           
                        }
                    });
                getDataTask.Wait();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public IActionResult Create([FromForm] Movie movie)
         {
             if (ModelState.IsValid)
             {
                 movie.ID = Guid.NewGuid();
                 movies.Add(movie);
                 return RedirectToAction(nameof(Index));
             }
             return View(movie);
         }*/

        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie movie = (Movie)GetByID(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [FromForm] Movie movie)
        {
            if (id != movie.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Movie currentMovie = (Movie)GetByID(id);
                currentMovie.Name = movie.Name;
                
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<string> DeleteConfirmed(Guid id)
        {
            try
            {
                string message = "";
                var client = new HttpClient();
                var getDataTask = await client.DeleteAsync("https://localhost:44312/api/Movies" + id);
                    {
                    message = await getDataTask.Content.ReadAsStringAsync();
                }
                return message;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

         public IActionResult Delete(Guid? id)
         {
             if (id == null)
             {
                 return NotFound();
             }

             Movie movie = (Movie)GetByID(id);
             if (movie == null)
             {
                 return NotFound();
             }

             return View(movie);
         }

        /* [HttpPost, ActionName("Delete")]
         [ValidateAntiForgeryToken]
         public IActionResult DeleteConfirmed(Guid id)
         {
             Movie movie = (Movie)GetByID(id);
             movies.Remove(movie);
             return RedirectToAction(nameof(Index));
         }*/

        private bool MovieExists(Guid id)
        {
            return movies.Any(e => e.ID == id);
        }
    }
}
