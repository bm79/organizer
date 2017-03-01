using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Organizer.Controllers;
using Organizer.Models.Modify;
using Organizer.Models.View;
using Organizer.Repositories;
using Organizer.Test.Tools;
using Xunit;

namespace Organizer.Test.Unit.Controller
{
    public class TaskControllerTest 
    {
    
        [Fact]
        public void Get_MockViewRepository_AndGetAllTasks()
        {
            //assign
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            var viewRepository = new Mock<IViewRepository>();
            var modifyRepository = new Mock<IModifyRepository>();
            var logger = new Mock<ILogger<TaskController>>();
            IEnumerable<TaskView> taskList = DomainTools.GetTaskList(miniAutoFixture,10);
            
            viewRepository.Setup(a=>a.GetAllTasks()).Returns(taskList);
            TaskController taskController = new TaskController(viewRepository.Object,modifyRepository.Object,logger.Object);
           
            //action
            IActionResult actionResult = taskController.Get();

            //assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnTaskList = (IEnumerable<TaskView>)okResult.Value;
            Assert.Equal(taskList.Count(),returnTaskList.Count());
            viewRepository.Verify(a=>a.GetAllTasks(),Times.Once);
            taskList.ToList().ForEach(task=>
                {
                    Assert.Equal(returnTaskList.ToList().Count(
                        returnTask=>
                        task.Id == returnTask.Id
                        && task.Text == returnTask.Text
                        && task.Date == returnTask.Date
                        && task.Deleted == returnTask.Deleted
                        && task.Done == returnTask.Done
                        ),1);
                }
            );

        }
        [Fact]
         public void GetUserTask_MockViewRepository_AndGetAllUserTask()
        {
            //assign
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            int idUser = miniAutoFixture.CreateInt();
            var viewRepository = new Mock<IViewRepository>();
            var modifyRepository = new Mock<IModifyRepository>();
            var logger = new Mock<ILogger<TaskController>>();
            
            IEnumerable<TaskView> taskList = DomainTools.GetTaskList(miniAutoFixture,10);
            

            viewRepository.Setup(a=>a.GetAllUserTask(It.IsAny<int>())).Returns(taskList);
            TaskController taskController = new TaskController(viewRepository.Object,modifyRepository.Object,logger.Object);
           
            //action
            IActionResult actionResult = taskController.GetUserTask(idUser);

            //assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnTaskList = (IEnumerable<TaskView>)okResult.Value;
            Assert.Equal(taskList.Count(),returnTaskList.Count());
            viewRepository.Verify(a=>a.GetAllUserTask(It.Is<int>(val=>val==idUser)),Times.Once);
            taskList.ToList().ForEach(task=>
                {
                    Assert.Equal(returnTaskList.ToList().Count(
                        returnTask=>
                        task.Id == returnTask.Id
                        && task.Text == returnTask.Text
                        && task.Date == returnTask.Date
                        && task.Deleted == returnTask.Deleted
                        && task.Done == returnTask.Done
                        ),1);
                }
            );

        }

        [Fact]
         public void Get_MockViewRepository_AndGetTask()
        {
            //assign
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            
            var viewRepository = new Mock<IViewRepository>();
            var modifyRepository = new Mock<IModifyRepository>();
            var logger = new Mock<ILogger<TaskController>>();
           

            TaskView taskView = DomainTools.GetTask(miniAutoFixture);
            

            viewRepository.Setup(a=>a.GetTask(It.Is<int>(val=>val==taskView.Id))).Returns(taskView);
            TaskController taskController = new TaskController(viewRepository.Object,modifyRepository.Object,logger.Object);
           
            //action
            IActionResult actionResult = taskController.Get(taskView.Id);

            //assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnTask = Assert.IsType<TaskView>(okResult.Value);
            
            viewRepository.Verify(a=>a.GetTask(It.Is<int>(val=>val==taskView.Id)),Times.Once);
            Assert.NotNull(returnTask);
            Assert.Equal(taskView.Id,returnTask.Id);
            Assert.Equal(taskView.Text,returnTask.Text);
            Assert.Equal(taskView.Date,returnTask.Date);
            Assert.Equal(taskView.Deleted,returnTask.Deleted);
            Assert.Equal(taskView.Done,returnTask.Done);

        }

        [Fact]
        public void Post_MockModifyRepository_AndInsertTask()
        {
            //assign
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            
            int idUser = miniAutoFixture.CreateInt();
            var viewRepository = new Mock<IViewRepository>();
            var modifyRepository = new Mock<IModifyRepository>();
            var logger = new Mock<ILogger<TaskController>>();

            TaskModify taskModify = DomainTools.CreateTaskModify(miniAutoFixture);
            TaskView taskView = new TaskView 
            {
                Id = miniAutoFixture.CreateInt(),
                Text = taskModify.Text,
                Date = taskModify.Date,
                Deleted = false,
                Done = false
            };
            
            modifyRepository.Setup(a=>a.InsertTask(It.IsAny<int>(),It.IsAny<TaskModify>())).Returns<int,TaskModify>((a,b)=>taskView);

            TaskController taskController = new TaskController(viewRepository.Object,modifyRepository.Object,logger.Object);
            
            //action
            IActionResult actionResult = taskController.Post(idUser,taskModify);
            
            //assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnTask = Assert.IsType<TaskView>(okResult.Value);
            modifyRepository.Verify(a=>a.InsertTask(It.Is<int>(val=>val==idUser),It.Is<TaskModify>(val=>val==taskModify)),Times.Once);
            Assert.NotNull(returnTask);
            Assert.Equal(taskView.Id,returnTask.Id);
            Assert.Equal(taskModify.Text,returnTask.Text);
            Assert.Equal(taskModify.Date,returnTask.Date);
            Assert.Equal(taskView.Deleted,returnTask.Deleted);
            Assert.Equal(taskView.Done,returnTask.Done);
        
        }

        [Fact]
        public void Put_MockModifyRepository_AndUpdateTask()
        {
            //assign
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            int idTask = miniAutoFixture.CreateInt();
            var viewRepository = new Mock<IViewRepository>();
            var modifyRepository = new Mock<IModifyRepository>();
            var logger = new Mock<ILogger<TaskController>>();

            TaskModify taskModify = DomainTools.CreateTaskModify(miniAutoFixture);
            TaskView taskView = new TaskView 
            {
                Id = idTask,
                Text = taskModify.Text,
                Date = taskModify.Date,
                Deleted = miniAutoFixture.CreateBoolean(),
                Done = miniAutoFixture.CreateBoolean()
            };

            modifyRepository.Setup(a=>a.UpdateTask(It.IsAny<int>(),It.IsAny<TaskModify>())).Returns<int,TaskModify>((a,b)=>taskView);

            TaskController taskController = new TaskController(viewRepository.Object,modifyRepository.Object,logger.Object);
            
            //action
            IActionResult actionResult = taskController.Put(idTask,taskModify);   
            
            //assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnTask = Assert.IsType<TaskView>(okResult.Value);
            modifyRepository.Verify(a=>a.UpdateTask(It.Is<int>(val=>val==idTask),It.Is<TaskModify>(val=>val==taskModify)),Times.Once);
            Assert.NotNull(returnTask);
            Assert.Equal(idTask,returnTask.Id);
            Assert.Equal(taskModify.Text,returnTask.Text);
            Assert.Equal(taskModify.Date,returnTask.Date);
            Assert.Equal(taskView.Deleted,returnTask.Deleted);
            Assert.Equal(taskView.Done,returnTask.Done);
        }

        [Fact]
        public void Delete_MockModifyRepository_AndDeleteTask()
        {
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            int idTask = miniAutoFixture.CreateInt();
            var viewRepository = new Mock<IViewRepository>();
            var modifyRepository = new Mock<IModifyRepository>();
            var logger = new Mock<ILogger<TaskController>>();
             TaskView taskView = new TaskView 
            {
                Id = idTask,
                Text = miniAutoFixture.CreateString(),
                Date = miniAutoFixture.CreateDatetime(),
                Deleted = true,
                Done = miniAutoFixture.CreateBoolean()
            };
            modifyRepository.Setup(a=>a.DeleteTask(It.IsAny<int>())).Returns<int>(a=>taskView);
            TaskController taskController = new TaskController(viewRepository.Object,modifyRepository.Object,logger.Object);


            IActionResult actionResult = taskController.Delete(idTask);
            
            //assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnTask = Assert.IsType<TaskView>(okResult.Value);
            modifyRepository.Verify(a=>a.DeleteTask(It.Is<int>(val=>val==idTask)),Times.Once);
            Assert.NotNull(returnTask);
            Assert.Equal(idTask,returnTask.Id);
            Assert.Equal(taskView.Text,returnTask.Text);
            Assert.Equal(taskView.Date,returnTask.Date);
            Assert.Equal(true,returnTask.Deleted);
            Assert.Equal(taskView.Done,returnTask.Done);
        }

         [Fact]
        public void Done_MockModifyRepository_AndDoneTask()
        {
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            int idTask = miniAutoFixture.CreateInt();
            var viewRepository = new Mock<IViewRepository>();
            var modifyRepository = new Mock<IModifyRepository>();
            var logger = new Mock<ILogger<TaskController>>();
            bool done = true;
             TaskView taskView = new TaskView 
            {
                Id = idTask,
                Text = miniAutoFixture.CreateString(),
                Date = miniAutoFixture.CreateDatetime(),
                Deleted = miniAutoFixture.CreateBoolean(),
                Done = done
            };
            modifyRepository.Setup(a=>a.DoneTask(It.IsAny<int>(),It.IsAny<bool>())).Returns<int,bool>((a,b)=>taskView);
            TaskController taskController = new TaskController(viewRepository.Object,modifyRepository.Object,logger.Object);


            IActionResult actionResult = taskController.Done(idTask);
            
            //assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnTask = Assert.IsType<TaskView>(okResult.Value);
            modifyRepository.Verify(a=>a.DoneTask(It.Is<int>(val=>val==idTask),It.Is<bool>(val=>val==done)) ,Times.Once);
            Assert.NotNull(returnTask);
            Assert.Equal(idTask,returnTask.Id);
            Assert.Equal(taskView.Text,returnTask.Text);
            Assert.Equal(taskView.Date,returnTask.Date);
            Assert.Equal(taskView.Deleted,returnTask.Deleted);
            Assert.Equal(done,returnTask.Done);
        }

         public void UnDone_MockModifyRepository_AndDoneTask()
        {
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            int idTask = miniAutoFixture.CreateInt();
            var viewRepository = new Mock<IViewRepository>();
            var modifyRepository = new Mock<IModifyRepository>();
            var logger = new Mock<ILogger<TaskController>>();
            bool done = false;
            TaskView taskView = new TaskView 
            {
                Id = idTask,
                Text = miniAutoFixture.CreateString(),
                Date = miniAutoFixture.CreateDatetime(),
                Deleted = miniAutoFixture.CreateBoolean(),
                Done = done
            };
            modifyRepository.Setup(a=>a.DoneTask(It.IsAny<int>(),It.IsAny<bool>())).Returns<int,bool>((a,b)=>taskView);
            TaskController taskController = new TaskController(viewRepository.Object,modifyRepository.Object,logger.Object);


            IActionResult actionResult = taskController.UnDone(idTask);
            
            //assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnTask = Assert.IsType<TaskView>(okResult.Value);
            modifyRepository.Verify(a=>a.DoneTask(It.Is<int>(val=>val==idTask),It.Is<bool>(val=>val==done)) ,Times.Once);
            Assert.NotNull(returnTask);
            Assert.Equal(idTask,returnTask.Id);
            Assert.Equal(taskView.Text,returnTask.Text);
            Assert.Equal(taskView.Date,returnTask.Date);
            Assert.Equal(taskView.Deleted,returnTask.Deleted);
            Assert.Equal(done,returnTask.Done);
        }



    }
}