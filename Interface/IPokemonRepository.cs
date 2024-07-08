using WebAPITest.Dto;
using WebAPITest.Models;

namespace WebAPITest.Interface
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemonByID(int id);
        Pokemon GetPokemonByName(string name);
        ICollection<Pokemon> GetPokemonListByName(string name);
        decimal GetPokemonRating(int id);
        Pokemon GetPokemonTrimToUpper(PokemonDto pokemonCreate);
        bool CheckPokemonExist(int id);
        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool DeletePokemon(Pokemon pokemon);
        bool Save();
    }
}
