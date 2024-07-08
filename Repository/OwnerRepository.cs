using Microsoft.EntityFrameworkCore;
using WebAPITest.DB;
using WebAPITest.Interface;
using WebAPITest.Models;

namespace WebAPITest.Repository
{
    public class OwnerRepository : IOwnerrepository
    {
        private readonly DbConnect _dbcontext;
        public OwnerRepository(DbConnect dbcontenxt) { 
                _dbcontext= dbcontenxt;
        }
        public bool CreateOwner(Owner owner)
        {
            _dbcontext.Add(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _dbcontext.Remove(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return _dbcontext.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
        {
            var owners = _dbcontext.pokemonOwners.Where(p => p.PokemonId== pokeId).Select(o => o.Owner).ToList();
            return owners;
        }

        public ICollection<Owner> GetOwners()
        {
            return _dbcontext.Owners.OrderBy(o=> o.FirstName).ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _dbcontext.pokemonOwners.Where(po => po.OwnerId == ownerId).Select(o => o.Pokemon).ToList();

        }

        public bool OwnerExists(int ownerId)
        {
            if (_dbcontext.Owners.Any(o=>o.Id == ownerId))
            {
                return true;
            }
            return false;
        }

        public bool Save()
        {
            var save = _dbcontext.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _dbcontext.Update(owner);
            return Save(); ;
        }
    }
}
