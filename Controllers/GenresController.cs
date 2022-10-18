
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresServices _genresServices;
        public GenresController(IGenresServices genresServices)
        {
            _genresServices = genresServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _genresServices.GetAll();
            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreDto dto)
        {
            if(dto is null)
                return BadRequest();

            var genre = new Genre
            {
                Name = dto.Name,
            };
            await _genresServices.Create(genre);

            return Ok(genre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(byte id, [FromBody] GenreDto dto)
        {
            if (id == 0)
                return BadRequest();

            var genre = await _genresServices.GetById(id);
            if (genre is null)
                return NotFound($"No genre was found with Id: {id}");
            genre.Name = dto.Name;
            _genresServices.Update(genre);

            return Ok(genre);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            if (id == 0)
                return BadRequest();

            var genre = await _genresServices.GetById(id);
            if (genre is null)
                return NotFound($"No genre was found with Id: {id}");

            _genresServices.Delete(genre);

            return Ok($"Genere with Id: {id} was deleted successfully");
        }
    }
}