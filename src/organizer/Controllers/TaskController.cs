using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Organizer.Models.Modify;
using Organizer.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Organizer.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : Controller
    {

        IViewRepository _viewRepository;
        IModifyRepository _modifyRepository;
         ILogger<TaskController> _logger;
        public TaskController(IViewRepository viewReposistory , IModifyRepository modifyRepository, ILogger<TaskController> logger)
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
                return Ok(_viewRepository.GetAllTasks());
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
                return Ok(_viewRepository.GetTask(id));
             } catch(Exception ex)
             {
                 _logger.LogError("Error in get",ex);
                return BadRequest();   
             }
        }


          // GET api/values/5
        [HttpGet("user/{id}")]
        public IActionResult GetUserTask(int id)
        {
            try {
                return Ok(_viewRepository.GetAllUserTask(id));
            } catch(Exception ex) {
                 _logger.LogError("Error in get",ex);
                return BadRequest();   
             }
        }

        // POST api/values
        [HttpPost("user/{id}")]
        public IActionResult Post(int id , [FromBody]TaskModify value)
        {
            try {
            if (ModelState.IsValid)
            {
                 return Ok(_modifyRepository.InsertTask(id,value));   
            } else
            {
                _logger.LogError("Error in post",ModelState);
                return BadRequest("");
            }

            } catch(Exception ex)
            {
                 _logger.LogError("Error in post",ex);
                return BadRequest();
            }
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TaskModify value)
        {
            try {
            if (ModelState.IsValid)
            {
                 return Ok(_modifyRepository.UpdateTask(id,value));   
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
                 return Ok(_modifyRepository.DeleteTask(id));
             } catch(Exception ex)
             {
                  _logger.LogError("Error in delete",ex);
                return BadRequest();
             }
        }

        [HttpPut("done/{id}")]
        public IActionResult Done(int id)
        {
         try {
            return Ok(_modifyRepository.DoneTask(id,true));   
           

            } catch(Exception ex)
            {
                 _logger.LogError("Error in put",ex);
                return BadRequest();
            }
        }

        [HttpPut("undone/{id}")]
        public IActionResult UnDone(int id)
        {
         try {
            return Ok(_modifyRepository.DoneTask(id,false));   
           

            } catch(Exception ex)
            {
                 _logger.LogError("Error in put",ex);
                return BadRequest();
            }
        }
    }
}
