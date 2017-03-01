using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Organizer.Controllers;
using Organizer.Models.View;
using Organizer.Repositories;
using Organizer.Test.Tools;
using Xunit;
using System.Linq;
using Organizer.Models.Modify;

namespace Organizer.Test.Unit.Controller
{
    public class UserControllerTest 
    {
        [Fact]
        public void Get_MockUserRepository_AndGetAllUsers()
        {
            //assign
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            IEnumerable<UserView> userList = DomainTools.GetUserList(miniAutoFixture,10);
            var viewRepository = new Mock<IViewRepository>();
            var modifyRepository = new Mock<IModifyRepository>();
            var logger = new Mock<ILogger<UserController>>();


            viewRepository.Setup(a=>a.GetAllUsers()).Returns(userList);
            UserController userController = new UserController(viewRepository.Object,modifyRepository.Object,logger.Object);

            //action
            IActionResult actionResult = userController.Get();
            
            //assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
           
            
            var returnUserList = (IEnumerable<UserView>)okResult.Value;

            Assert.Equal(userList.Count(),returnUserList.Count());
            userList.ToList().ForEach(user=>
                {
                    Assert.Equal(returnUserList.ToList()
                    .Count
                    (
                        returnUser => 
                            user.Id == returnUser.Id 
                            && user.Name == returnUser.Name
                            && user.Surname == returnUser.Surname 
                            && user.Deleted == returnUser.Deleted    
                        ),
                    1);
                }
            );
        
        }

        [Fact]
        public void Get_MockUserRepository_AndGetUser()
        {
            //assign
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            UserView user = DomainTools.GetUser(miniAutoFixture);

            var viewRepository = new Mock<IViewRepository>();
            var modifyRepository = new Mock<IModifyRepository>();
            var logger = new Mock<ILogger<UserController>>();
            viewRepository.Setup(a=>a.GetUser(It.Is<int>(b=>b==user.Id))).Returns(user);
            UserController userController = new UserController(viewRepository.Object,modifyRepository.Object,logger.Object);
            
            //action
            IActionResult actionResult = userController.Get(user.Id);

            // assert
            viewRepository.Verify(a=>a.GetUser(It.Is<int>(b=>b==user.Id)),Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnUser = Assert.IsType<UserView>(okResult.Value);
            Assert.NotNull(returnUser);
            Assert.Equal(user.Id , returnUser.Id);
            Assert.Equal(user.Name,returnUser.Name);
            Assert.Equal(user.Surname,returnUser.Surname);
            Assert.Equal(user.Deleted,returnUser.Deleted);
        
        }

        [Fact]
        public void Post_MockUserRepository_AndInsertUser()
        {
            //assign
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            UserModify userModify = DomainTools.CreateUserModify(miniAutoFixture);
            UserView userView = new UserView
             {
                Id = miniAutoFixture.CreateInt(),
                Name = userModify.Name,
                Surname = userModify.Surname,
                Deleted = false

            };

            var viewRepository = new Mock<IViewRepository>();
            var modifyRepository = new Mock<IModifyRepository>();
            var logger = new Mock<ILogger<UserController>>();
            modifyRepository.Setup(a=>a.InsertUser(It.IsAny<UserModify>())).Returns<UserModify>(a=>userView);
            UserController userController = new UserController(viewRepository.Object,modifyRepository.Object,logger.Object);
            
            
            //action
            IActionResult actionResult = userController.Post(userModify);

            //assert
            modifyRepository.Verify(a=>a.InsertUser(It.Is<UserModify>(val=>val.Name == userModify.Name && val.Surname == userModify.Surname)),Times.Once);
      
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnUser = Assert.IsType<UserView>(okResult.Value);
             Assert.NotNull(returnUser);
            Assert.Equal(userModify.Name,returnUser.Name);
            Assert.Equal(userModify.Surname,returnUser.Surname);
            Assert.Equal(userView.Id,returnUser.Id);
            Assert.Equal(userView.Deleted,returnUser.Deleted);
        
        }

        [Fact]
        public void Put_MockUserRepository_AndUpdateUser()
        {
            //assign
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            int idUser = miniAutoFixture.CreateInt();
            miniAutoFixture.Initialize();
            UserModify userModify = DomainTools.CreateUserModify(miniAutoFixture);
            UserView userView = new UserView
             {
                Id = idUser,
                Name = userModify.Name,
                Surname = userModify.Surname,
                Deleted = miniAutoFixture.CreateBoolean()
            };
            var viewRepository = new Mock<IViewRepository>();
            var modifyRepository = new Mock<IModifyRepository>();
            var logger = new Mock<ILogger<UserController>>();

            modifyRepository.Setup(a=>a.UpdateUser(It.IsAny<int>(),It.IsAny<UserModify>())).Returns<int,UserModify>((a,b)=>userView);
            UserController userController = new UserController(viewRepository.Object,modifyRepository.Object,logger.Object);
            
            
            //action
            IActionResult actionResult = userController.Put(idUser,userModify);

            //assert
            modifyRepository.Verify
            (
            a=>
                a.UpdateUser
                (
                It.Is<int>(val=>val==idUser),
                It.Is<UserModify>
                (val=>val.Name == userModify.Name && val.Surname == userModify.Surname)
                )
                ,Times.Once
            );
      
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnUser = Assert.IsType<UserView>(okResult.Value);
             Assert.NotNull(returnUser);
            Assert.Equal(userModify.Name,returnUser.Name);
            Assert.Equal(userModify.Surname,returnUser.Surname);
            Assert.Equal(idUser,returnUser.Id);
            Assert.Equal(userView.Deleted,returnUser.Deleted);

        }

        [Fact]
        public void Delete_MockUserRepository_AndDeleteUser()
        {
            //assign
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            int idUser = miniAutoFixture.CreateInt();
            miniAutoFixture.Initialize();
            
            UserView userView = new UserView
             {
                Id = idUser,
                Name = miniAutoFixture.CreateString(),
                Surname = miniAutoFixture.CreateString(),
                Deleted = true
            };
            var viewRepository = new Mock<IViewRepository>();
            var modifyRepository = new Mock<IModifyRepository>();
            var logger = new Mock<ILogger<UserController>>();

            modifyRepository.Setup(a=>a.DeleteUser(It.IsAny<int>())).Returns<int>(a=>userView);
            UserController userController = new UserController(viewRepository.Object,modifyRepository.Object,logger.Object);
            
            
            //action
            IActionResult actionResult = userController.Delete(idUser);

            //assert
            modifyRepository.Verify(a=>a.DeleteUser(It.Is<int>(val=>val==idUser)),Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnUser = Assert.IsType<UserView>(okResult.Value);
            Assert.NotNull(returnUser);
            Assert.Equal(userView.Name,returnUser.Name);
            Assert.Equal(userView.Surname,returnUser.Surname);
            Assert.Equal(idUser,returnUser.Id);
            Assert.Equal(true,returnUser.Deleted);
        }

        
    }
}