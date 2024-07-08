using Microsoft.AspNetCore.Mvc;
using WebAPITest.Dto;
using WebAPITest.Extension;
using WebAPITest.Interface;
using WebAPITest.Models;
using WebAPITest.Repository;

namespace WebAPITest.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _icountryRepository;

        public CountryController(ICountryRepository countryRepository) {
            _icountryRepository = countryRepository;
        }

        [HttpGet]
        public IActionResult GetCountriesList()
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countries = _icountryRepository.GetCountries().Select(c => ToDtoExtension.CountryMapToDto(c)).ToList();

            return Ok(countries);
        }


        [HttpGet ("CountryId={id}")]
        public IActionResult GetCountry(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_icountryRepository.CheckCountryExist(id))
            {
                return NotFound();
            }
            var country = _icountryRepository.GetCountry(id);

            return Ok(ToDtoExtension.CountryMapToDto(country));
        }

        [HttpGet("GetCountryOfAnOwner/OwnerId={id}")]
        public IActionResult GetCountryByOwner(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            var country = _icountryRepository.GetCountryByOwner(id);

            return Ok(ToDtoExtension.CountryMapToDto(country));
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
        {
            if (countryCreate == null)
                return BadRequest(ModelState);

            var country = _icountryRepository.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = ToDtoExtension.CountryDtoMap(countryCreate);

            if (!_icountryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto updatedCountry)
        {
            if (updatedCountry == null)
                return BadRequest(ModelState);

            if (countryId != updatedCountry.Id)
                return BadRequest(ModelState);

            if (!_icountryRepository.CheckCountryExist(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var countryMap = ToDtoExtension.CountryDtoMap(updatedCountry);

            if (!_icountryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong updating category");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Updated to "+ updatedCountry.Name);
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_icountryRepository.CheckCountryExist(countryId))
            {
                return NotFound();
            }

            var countryToDelete = _icountryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_icountryRepository.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return Ok("Successfully Deleted ");
        }
    }
}
