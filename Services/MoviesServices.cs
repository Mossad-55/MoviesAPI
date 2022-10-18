using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Services
{
    public class MoviesServices : IMoviesServices
    {
        private readonly AppDbContext _db;

        public MoviesServices(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Movie> Add(Movie movie)
        {
            await _db.AddAsync(movie);
            _db.SaveChanges();
            return movie;
        }

        public void Delete(Movie movie)
        {
            _db.Remove(movie);
            _db.SaveChanges();
        }

        public async Task<IEnumerable<Movie>> GetAll(byte genreId = 0)
        {
            // Genere will be Complex Object 
            /*return await _db.Movies
                .Where(m => m.GenereId == genreId || genreId == 0)
                .OrderByDescending(x => x.Rate)
                .Include(m => m.Genere)
                .ToListAsync();
*/
            return await _db.Movies
                .Where(m => m.GenereId == genreId || genreId == 0)
                .OrderByDescending(m => m.Rate)
                .Include(m => m.Genere)
                .ToListAsync();
        }

        public async Task<Movie> GetById(int id)
        {
            return await _db.Movies.Include(m => m.Genere).SingleOrDefaultAsync(x => x.Id == id);
        }

        public Movie Update(Movie movie)
        {
            _db.Update(movie);
            _db.SaveChanges();
            return movie;
        }
    }
}
