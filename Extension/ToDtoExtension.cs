using WebAPITest.Dto;
using WebAPITest.Models;

namespace WebAPITest.Extension
{
    public static class ToDtoExtension
    {
        public static PokemonDto PokemonMapToDto(this Pokemon pokemon)
        {
            if (pokemon == null)
                return null;

            return new PokemonDto
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                BirthDate = pokemon.BirthDate
            };
        }
        public static Pokemon PokemonDtoMap(this PokemonDto pokemon)
        {
            if (pokemon == null)
                return null;

            return new Pokemon
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                BirthDate = pokemon.BirthDate,
                Reviews = new List<Review>(),
                PokemonOwners = new List<PokemonOwner>(),
                PokemonCategories = new List<PokemonCategory>()
            };
        }


        public static CategoryDto CategoryMapToDto(this Category category)
        {
            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name

            };
        }
        public static Category CategoryDtoMap(this CategoryDto category)
        {
            if (category == null)
                return null;

            return new Category
            {
                Id = category.Id,
                Name = category.Name,
                PokemonCategories = new List<PokemonCategory>()

            };
        }



        public static CountryDto CountryMapToDto(this Country country)
        {
            if (country == null)
                return null;

            return new CountryDto
            {
                Id = country.Id,
                Name = country.Name

            };
        }
        public static Country CountryDtoMap(this CountryDto country)
        {
            if (country == null)
                return null;

            return new Country
            {
                Id = country.Id,
                Name = country.Name,
                Owners = new List<Owner>(),
            };
        }

        public static OwnerDto OwnerMapToDto(this Owner country)
        {
            if (country == null)
                return null;

            return new OwnerDto
            {
                Id = country.Id,
                FirstName = country.FirstName,
                LastName = country.LastName,
                Gym= country.Gym
            };
        }
        public static Owner OwnerDtoMap(this OwnerDto owner)
        {
            if (owner == null)
                return null;

            return new Owner
            {
                Id = owner.Id,
                FirstName = owner.FirstName,
                LastName = owner.LastName,
                Gym = owner.Gym,
                Country = null ,
                PokemonOwners = null
            };
        }

        public static Review ReviewDtoMap(this ReviewDto country)
        {
            if (country == null)
                return null;

            return new Review
            {
                Id = country.Id,
                Title = country.Title,
                Text = country.Text,
                Rating = country.Rating,
                Reviewer =null,
                Pokemon = null
            };
        }
        public static ReviewDto ReviewMapToDto(this Review country)
        {
            if (country == null)
                return null;

            return new ReviewDto
            {
                Id = country.Id,
                Title = country.Title,
                Text = country.Text,
                Rating = country.Rating,
            };
        }


        public static Reviewer ReviewerDtoMap(this ReviewerDto country)
        {
            if (country == null)
                return null;

            return new Reviewer
            {
                Id = country.Id,
                FirstName = country.FirstName,
                LastName = country.LastName,
                Reviews = null,
                
            };
        }
        public static ReviewerDto ReviewerMapToDto(this Reviewer country)
        {
            if (country == null)
                return null;

            return new ReviewerDto
            {
                Id = country.Id,
                FirstName = country.FirstName,
                LastName = country.LastName,
               
            };
        }
    }
}
