using Microsoft.AspNetCore.Mvc;
using System.Web.WebPages.Html;
using WebAPITest.Interface;
using WebAPITest.Models;

namespace WebAPITest.Controllers
{
    [Route("api/GetPokemons")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        public PokemonController(IPokemonRepository pokemonRepository)
        {
            _pokemonRepository = pokemonRepository;
        }
            
        [HttpGet]
        [ProducesResponseType(200,Type= typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemonList() {
            var pokemonList = _pokemonRepository.GetPokemons();
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemonList);


        }

    }
}
