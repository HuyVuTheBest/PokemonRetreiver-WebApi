using WebAPITest.Models;

namespace WebAPITest.Interface
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();

    }
}
