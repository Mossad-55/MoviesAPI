namespace MoviesAPI.Services
{
    public class GenresServices : IGenresServices
    {
        private readonly AppDbContext _db;
        public GenresServices(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Genre> Create(Genre genre)
        {
            await _db.Genres.AddAsync(genre);
            _db.SaveChanges();
            return genre;
        }

        public void Delete(Genre genre)
        {
            _db.Remove(genre);
            _db.SaveChanges();
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await _db.Genres.OrderBy(g => g.Name).ToListAsync();
        }

        public async Task<Genre> GetById(byte id)
        {
            return await _db.Genres.SingleOrDefaultAsync(g => g.Id == id);
        }

        public Task<bool> IsValidGenre(byte id)
        {
            return _db.Genres.AnyAsync(g => g.Id == id);
        }

        public Genre Update(Genre genre)
        {
            _db.Genres.Update(genre);
            _db.SaveChanges();
            return genre;
        }
    }
}
