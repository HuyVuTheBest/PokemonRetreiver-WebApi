using Microsoft.AspNetCore.Mvc;
using WebAPITest.DB;
using WebAPITest.Dto;
using WebAPITest.Extension;
using WebAPITest.Interface;
using WebAPITest.Models;
using WebAPITest.Repository;


namespace WebAPITest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerrepository _Iownerrepository;
        private readonly ICountryRepository _countryRepository;
        public OwnerController(IOwnerrepository Iownerrepository, ICountryRepository countryRepository) 
        {
            _Iownerrepository= Iownerrepository;
            _countryRepository= countryRepository;  
        }
        [HttpGet]
        public IActionResult GetOwners()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var OwnerList = _Iownerrepository.GetOwners().Select(o=> ToDtoExtension.OwnerMapToDto(o)).ToList();

            return Ok(OwnerList);
        }

        [HttpGet("{ownerId}")]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_Iownerrepository.OwnerExists(ownerId))
            {
                return NotFound();
            }
            var owner = ToDtoExtension.OwnerMapToDto(_Iownerrepository.GetOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        [HttpGet("GetPokemonByOwner/{ownerId}")]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!_Iownerrepository.OwnerExists(ownerId))
            {
                return NotFound();
            }
            var pokemon =_Iownerrepository.GetPokemonByOwner(ownerId).Select(pokemon => ToDtoExtension.PokemonMapToDto(pokemon)).ToList();

           

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (pokemon.Count == 0)
            {
                return Ok("This owner has no Pokemon");
            }
            return Ok(pokemon);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto ownerCreate)
        {
            if (ownerCreate == null)
                return BadRequest(ModelState);

            var owners = _Iownerrepository.GetOwners()
                .Where(c => c.LastName.Trim().ToUpper() == ownerCreate.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (owners != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = ToDtoExtension.OwnerDtoMap(ownerCreate);

            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_Iownerrepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto updatedOwner)
        {
            if (updatedOwner == null)
                return BadRequest(ModelState);

            if (ownerId != updatedOwner.Id)
                return BadRequest(ModelState);

            if (!_Iownerrepository.OwnerExists(ownerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();


            var ownerMap = ToDtoExtension.OwnerDtoMap(updatedOwner);

            if (!_Iownerrepository.UpdateOwner(ownerMap) )
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Updated");
        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOwner(int ownerId)
        {
            if (!_Iownerrepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            var ownerToDelete = _Iownerrepository.GetOwner(ownerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_Iownerrepository.DeleteOwner(ownerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return Ok("Successfully Deleted");
        }
    }
}
