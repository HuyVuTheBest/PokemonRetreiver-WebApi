using WebAPITest.DB;
using WebAPITest.Interface;
using WebAPITest.Models;

namespace WebAPITest.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DbConnect _context;
        public PokemonRepository(DbConnect dbContext) { 
            _context = dbContext;
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.OrderBy(p => p.Id).ToList();   
        
        }


    }
}
