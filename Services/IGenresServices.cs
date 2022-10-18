namespace MoviesAPI.Services
{
    public interface IGenresServices
    {
        Task<IEnumerable<Genre>> GetAll();
        Task<Genre> GetById(byte id);
        Task<Genre> Create(Genre genre);
        Genre Update(Genre genre);
        void Delete(Genre genre);
        Task<bool> IsValidGenre(byte id);
    }
}
