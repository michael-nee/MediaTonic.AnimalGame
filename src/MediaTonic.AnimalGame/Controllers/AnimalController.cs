using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediaTonic.AnimalGame.API.Attributes;
using MediaTonic.AnimalGame.API.Models;
using MediaTonic.AnimalGame.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediaTonic.AnimalGame.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ValidateModel]
    public class AnimalController : ControllerBase
    {
        private readonly IMapper _mapper;

        public AnimalController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public ActionResult<AnimalViewModel> Get(Guid id)
        {
            var animal = Animal.GetAnimal(id);

            if (animal == null) return NotFound();

            var viewModel = _mapper.Map<AnimalViewModel>(animal);

            return Ok(viewModel);
        }

        //Not Needed
        [HttpGet("GetByIdUserId")]
        public ActionResult<AnimalViewModel> GetByIdUserId([RequiredFromQuery]Guid id, [RequiredFromQuery]Guid userId)
        {
            var user = Models.User.GetUserById(userId);

            if (user == null) return NotFound("No user found for given userId");

            var animal = Animal.GetAnimal(id, userId);

            if (animal == null) return NotFound("No animal found for given id");

            var viewModel = _mapper.Map<AnimalViewModel>(animal);

            return Ok(viewModel);
        }

        //Not Needed
        [HttpGet("GetByIdUserName")]
        public ActionResult<AnimalViewModel> GetByIdUserName([RequiredFromQuery]Guid id, [RequiredFromQuery]string userName)
        {
            var user = Models.User.GetUserByUserName(userName);

            if (user == null) return NotFound("No user found for given userName");

            var animal = Animal.GetAnimal(id, userName);

            if (animal == null) return NotFound("No animal found for given id");

            var viewModel = _mapper.Map<AnimalViewModel>(animal);

            return Ok(viewModel);
        }

        [HttpPost]
        public ActionResult Post([FromBody] CreateAnimalViewModel viewModel)
        {
            var user = Models.User.GetUserByUserName(viewModel.UserId);
            if (user == null) return NotFound("No user for userId provided");

            var animal = Animal.CreateAnimal(viewModel.AnimalType, viewModel.Name, user);

            if (animal == null)
                return BadRequest("Can't create animal");

            var animalViewModel = _mapper.Map<AnimalViewModel>(animal);

            return CreatedAtAction("Get", new { id = animalViewModel.Id }, animalViewModel);
        }

        [HttpPut("Feed")]
        public ActionResult Feed([Required]Guid id)
        {
            var animal = Animal.GetAnimal(id);

            if (animal == null)
                return NotFound();

            var hungriness = animal.FeedAnimal();

            return Ok(hungriness);
        }

        [HttpPut("Pet")]
        public ActionResult Pet([Required]Guid id)
        {
            var animal = Animal.GetAnimal(id);

            if (animal == null)
                return NotFound();

            var happiness = animal.PetAnimal();

            return Ok(happiness);
        }
    }
}