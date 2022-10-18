namespace MoviesAPI.Services
{
    public interface IMoviesServices
    {
        Task<IEnumerable<Movie>> GetAll(byte genreId = 0);
        Task<Movie> GetById(int id);
        Task<Movie> Add(Movie movie);
        Movie Update(Movie movie);
        void Delete(Movie movie);



    }
}
