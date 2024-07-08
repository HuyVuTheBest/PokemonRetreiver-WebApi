using Microsoft.EntityFrameworkCore;
using WebAPITest.DB;
using WebAPITest.Dto;
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

        public bool CheckPokemonExist(int id)
        {
            return _context.Pokemons.Any(p=>p.Id == id);
        }

        public ICollection<Pokemon> GetPokemonListByName(string name)
        {
            return _context.Pokemons.Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{name.ToLower()}%")).ToList();
        }


        public Pokemon GetPokemonByID(int id)
        {
            return _context.Pokemons.Where(p=> p.Id == id).FirstOrDefault();
        }

        

        public decimal GetPokemonRating(int id)
        {

            var reviews = _context.Reviews.Where(r => r.Pokemon.Id == id).ToList();

            if (reviews.Count == 0)
            {
                return 0;
            }

            return (decimal)reviews.Average(r => r.Rating);

        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.OrderBy(p => p.Id).ToList();   
        
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0? true : false;
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save(); 
        }
        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = _context.Owners.Where( o => o.Id == ownerId).FirstOrDefault();
            var category = _context.Categories.Where(a => a.Id == categoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon= pokemon
            };

            _context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon,
            };

            _context.Add(pokemonCategory);
            _context.Add(pokemon);
            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return Save(); 
        }
        public Pokemon GetPokemonByName(string name)
        {
            return _context.Pokemons.Where(p => p.Name == name).FirstOrDefault();

        }
        public Pokemon GetPokemonTrimToUpper(PokemonDto pokemonCreate)
        {
            return GetPokemons().Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper()).FirstOrDefault(); ;
        }



     
    }
}
