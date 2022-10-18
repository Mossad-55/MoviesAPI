using AutoMapper;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesServices _moviesServices;
        private readonly IGenresServices _genresServices;
        private readonly IMapper _mapper;

        private new List<string> _allowedExtensions = new() { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 8388608;

        public MoviesController(IMoviesServices moviesServices, IGenresServices genresServices, IMapper mapper)
        {
            _moviesServices = moviesServices;
            _genresServices = genresServices;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _moviesServices.GetAll();

            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _moviesServices.GetById(id);
            if (movie is null)
                return NotFound();

            var movieDetailsDto = _mapper.Map<MovieDetailsDto>(movie);
            return Ok(movieDetailsDto);
        }

        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        {
            var movies = await _moviesServices.GetAll(genreId);

            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm]MovieDto dto)
        {
            if (dto.Poster is null)
                return BadRequest("Poster is required");

            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName.ToLower())))
                return BadRequest("Only support (.jpg - .png) for the poster");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allower size for poster is 8Mbs only");

            var isValidGenre = await _genresServices.IsValidGenre(dto.GenereId);
            if (!isValidGenre)
                return BadRequest($"No genere with Id: {dto.GenereId}");

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = dataStream.ToArray();

            await _moviesServices.Add(movie);

            return Ok(movie);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] MovieDto dto)
        {
            var movie = await _moviesServices.GetById(id);
            if (movie is null)
                return NotFound();

            var isValidGenre = await _genresServices.IsValidGenre(dto.GenereId);
            if (!isValidGenre)
                return BadRequest($"No genere with Id: {dto.GenereId}");

            if(dto.Poster != null)
            {
                if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName.ToLower())))
                    return BadRequest("Only support (.jpg - .png) for the poster");

                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allower size for poster is 8Mbs only");
                
                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);

                movie.Poster = dataStream.ToArray();
            }

            movie.Title = dto.Title;
            movie.GenereId = dto.GenereId;
            movie.Year = dto.Year;
            movie.StoryLine = dto.StoryLine;
            movie.Rate = dto.Rate;

            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _moviesServices.GetById(id);

            if(movie is null)
                return NotFound();

            _moviesServices.Delete(movie);

            return Ok();
        }

        
    }
}
