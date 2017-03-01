using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Organizer.Models.Modify;
using Organizer.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Organizer.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        ILogger<UserController> _logger;
        IViewRepository _viewRepository;
        IModifyRepository _modifyRepository;

        public UserController(IViewRepository viewReposistory , IModifyRepository modifyRepository,ILogger<UserController> logger)
        {
            _viewRepository = viewReposistory;
            _modifyRepository = modifyRepository;
            _logger = logger;
        }
        
        
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            try {
                return Ok(_viewRepository.GetAllUsers());
            } catch(Exception ex)
            {
                  _logger.LogError("Error in get",ex);
                return BadRequest();   
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
             try {
                return Ok(_viewRepository.GetUser(id));
            } catch(Exception ex)
            {
                  _logger.LogError("Error in get",ex);
                return BadRequest();   
            }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]UserModify value)
        {
         try {
            if (ModelState.IsValid)
            {
                 return Ok(_modifyRepository.InsertUser(value));   
            } else
            {
                _logger.LogError("Error in post",ModelState);
                return BadRequest();
            }

            } catch(Exception ex)
            {
                 _logger.LogError("Error in post",ex);
                return BadRequest();
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UserModify value)
        {
            try {
            if (ModelState.IsValid)
            {
                 return Ok(_modifyRepository.UpdateUser(id,value));   
            } else
            {
                _logger.LogError("Error in put",ModelState);
                return BadRequest("");
            }

            } catch(Exception ex)
            {
                 _logger.LogError("Error in put",ex);
                return BadRequest();
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try {
                return Ok(_modifyRepository.DeleteUser(id));
            } catch(Exception ex)
            {
                  _logger.LogError("Error in get",ex);
                return BadRequest();   
            }
        }
    }
}
