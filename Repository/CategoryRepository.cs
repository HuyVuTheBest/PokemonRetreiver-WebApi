using Microsoft.EntityFrameworkCore;
using WebAPITest.DB;
using WebAPITest.Dto;
using WebAPITest.Interface;
using WebAPITest.Models;

namespace WebAPITest.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbConnect _dbContext;
        public CategoryRepository(DbConnect dbContext)
        {
            _dbContext = dbContext;
        }



        public bool CategoryExist(int id)
        {
            if(_dbContext.Categories.Any(c => c.Id == id))
            {
                return true;
            }
            return false;
        }

        public bool CreateCategory(Category category)
        {
            _dbContext.Categories.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _dbContext.Categories.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _dbContext.Categories.OrderBy(c => c.Id).ToList();
        }

        public Category GetCategory(int id)
        {       
                return _dbContext.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonsByCategory(int id)
        {
            return _dbContext.PokemonCategories.Where(c => c.CategoryId == id).Select(c => c.Pokemon).ToList();
        }

        public bool Save()
        {
            var save = _dbContext.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _dbContext.Categories.Update(category);
            return Save();
        }
    }
}
