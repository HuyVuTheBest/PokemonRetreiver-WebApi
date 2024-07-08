using WebAPITest.DB;
using WebAPITest.Interface;
using WebAPITest.Models;

namespace WebAPITest.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DbConnect _dbContext;
        public CountryRepository(DbConnect _dbcontext) {
            _dbContext = _dbcontext;
        }
        public bool CheckCountryExist(int countryid)
        {
            if (_dbContext.Countries.Any(c => c.Id == countryid))
            {
                return true;
            }
            return false;
        }

        public bool CreateCountry(Country country)
        {
            _dbContext.Add(country);
            return Save();
            
        }

        public bool DeleteCountry(Country country)
        {
            _dbContext.Remove(country);
            return Save(); 
        }

        public ICollection<Country> GetCountries()
        {
            return _dbContext.Countries.OrderBy(c =>c.Id).ToList();
        }

        public Country GetCountry(int id)
        {
            return _dbContext.Countries.Where(c => c.Id == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return _dbContext.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromCountry(int countryId)
        {
            return _dbContext.Countries.Where(c => c.Id == countryId).Select(c => c.Owners).FirstOrDefault();

        }

        public bool Save()
        {
            var save = _dbContext.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _dbContext.Update(country);
            return Save();
        }
    }
}
