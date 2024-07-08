using Microsoft.AspNetCore.Mvc;
using WebAPITest.Dto;
using WebAPITest.Extension;
using WebAPITest.Interface;
using WebAPITest.Models;
using WebAPITest.Repository;

namespace WebAPITest.Controllers
{
    [Route("api/GetPokemons")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ICategoryRepository _categoryRepository;


        public PokemonController(IPokemonRepository pokemonRepository, IReviewRepository reviewRepository, ICategoryRepository categoryRepository)
        {
            _pokemonRepository = pokemonRepository;
            _reviewRepository = reviewRepository;
            _categoryRepository = categoryRepository;
        }

     



        [HttpGet]
        [ProducesResponseType(200,Type= typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemonList() {
            var pokemonList = _pokemonRepository.GetPokemons();

            var newPokemonList = pokemonList
                                .Where(pokemon => pokemon != null)
                                .Select(pokemon => ToDtoExtension.PokemonMapToDto(pokemon))
                                .ToList();



            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(newPokemonList);


        }


        [HttpGet ("GetPokemonById/{id}")]
        public IActionResult GetPokemon(int id)
        {

            if(!_pokemonRepository.CheckPokemonExist(id)) return NotFound() ;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
             

            var pokemon = _pokemonRepository.GetPokemonByID(id);

            return Ok(ToDtoExtension.PokemonMapToDto(pokemon));
        }

        [HttpGet("PokemonListByName/{name}")]
        public IActionResult GetPokemonListName(string name)
        {
            var pokemonList = _pokemonRepository.GetPokemonListByName(name);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
         
            var pokemonListdto = pokemonList.Select(p => ToDtoExtension.PokemonMapToDto(p)).ToList();
            return Ok(pokemonListdto);
        }
        [HttpGet("PokemonByName/{name}")]
        public IActionResult GetPokemonName(string name)
        {
            var pokemon = _pokemonRepository.GetPokemonByName(name);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(pokemon == null) return NotFound() ;

           
            return Ok(ToDtoExtension.PokemonMapToDto(pokemon));
        }



        [HttpGet("ratings/{id}")]
        public IActionResult GetPokemonRatings(int id)
        {

            if (!_pokemonRepository.CheckPokemonExist(id)) return NotFound();

            var ratings = _pokemonRepository.GetPokemonRating(id);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            return Ok(ratings);
        }


        [HttpPost("CreatePokemon")]
     
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PokemonDto pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var pokemons = _pokemonRepository.GetPokemonTrimToUpper(pokemonCreate);

            if (!_categoryRepository.CategoryExist(catId))
            {
                return BadRequest(ModelState);
            }

            if (pokemons != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = pokemonCreate;


            if (!_pokemonRepository.CreatePokemon(ownerId, catId, ToDtoExtension.PokemonDtoMap(pokemonMap)))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created " + pokemonMap.Name);
        }

        [HttpPut("{pokeId}")]
        public IActionResult UpdatePokemon(int pokeId,
           [FromQuery] int ownerId, [FromQuery] int catId,
           [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
                return BadRequest(ModelState);

            if (pokeId != updatedPokemon.Id)
                return BadRequest(ModelState);

            if (!_pokemonRepository.CheckPokemonExist(pokeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var pokemonMap = ToDtoExtension.PokemonDtoMap( updatedPokemon);

            if (!_pokemonRepository.UpdatePokemon(ownerId, catId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfuly Updated Pokemon " + updatedPokemon.Name);
        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokeId)
        {
            if (!_pokemonRepository.CheckPokemonExist(pokeId))
            {
                return NotFound();
            }

            var reviewsToDelete = _reviewRepository.GetReviewsOfAPokemon(pokeId);
            var pokemonToDelete = _pokemonRepository.GetPokemonByID(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting reviews");
            }

            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return Ok("Successfuly Deleted Pokemon " + pokemonToDelete.Name);
        }
    }
}
