using AutoMapper;
using MediaTonic.AnimalGame.API.Attributes;
using MediaTonic.AnimalGame.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace MediaTonic.AnimalGame.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        public UserController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public ActionResult<UserViewModel> Get(Guid id)
        {
            var user = Models.User.GetUserById(id);

            if (user == null) return NotFound();

            var viewModel = _mapper.Map<UserViewModel>(user);

            return Ok(viewModel);
        }

        [HttpGet("GetByUserName")]
        public ActionResult<UserViewModel> GetByUserName([Required]string userName)
        {
            var user = Models.User.GetUserByUserName(userName);

            if (user == null) return NotFound();

            var viewModel = _mapper.Map<UserViewModel>(user);

            return Ok(viewModel);
        }

        [HttpPost]
        public ActionResult Post([FromBody]CreateUserViewModel viewModel)
        {
            var user = Models.User.GetUserByUserName(viewModel.UserId);

            if (user != null) return Conflict(viewModel.UserId + " already exists.");

            user = Models.User.CreateUser(viewModel.Name, viewModel.UserId);

            var userViewModel = _mapper.Map<UserViewModel>(user);

            return CreatedAtAction("Get", new { id = user.Id }, userViewModel);
        }
    }
}