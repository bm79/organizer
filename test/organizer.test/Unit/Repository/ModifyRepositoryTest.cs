using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Organizer.Context;
using Organizer.Models.Modify;
using Organizer.Models.View;
using Organizer.Repositories;
using Organizer.Test.Tools;
using Xunit;

namespace Organizer.Test.Unit.Repository 
{
    public class ModifyRepositoryTest
    {
        [Fact]
        public void InsertUser_MockDbSetUser_AndGetInsertUser()
        {
            //assign
            int newIdUser = 11;
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            List<User> usersCollection  = DbSetTools.CreateUserList(1,10,miniAutoFixture);
            IQueryable<User> users = usersCollection.AsQueryable(); 
            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns( users.GetEnumerator());
            mockDbSet.Setup(m=>m.Add(It.IsAny<User>()))
                    .Callback<User>(a=> 
                    {
                        a.Id= newIdUser ; 
                        usersCollection.Add(a);
                    });

            var mockContext = new Mock<OrganizerContext>();
            mockContext.Setup(c => c.User).Returns(mockDbSet.Object);
            var logger = new Mock<ILogger<ModifyRepository>>();
            ModifyRepository modifyRepository = new ModifyRepository(mockContext.Object,logger.Object);
            UserModify userModify = DomainTools.CreateUserModify(miniAutoFixture);
            
            //action
            UserView userView = modifyRepository.InsertUser(userModify);
            
            //assert
            mockDbSet.Verify
            (
                a=> a.Add
                (
                    It.Is<User>
                    (
                
                        user => user.Name == userModify.Name 
                        && user.Surname == userModify.Surname
                        && !user.Deleted
                        && user.Id == newIdUser
                    )
                ),
                Times.Once
            );
            mockContext.Verify(a=>a.SaveChanges(),Times.Once);


            Assert.Equal(userModify.Name , userView.Name);
            Assert.Equal(userModify.Surname,userView.Surname);
            Assert.Equal(false,userView.Deleted);
            Assert.Equal(newIdUser,userView.Id);
        }

        [Fact]
        public void ModifytUser_MockDbSetUser_AndGetUpdateUser()
        {
            //assign
            int idUserUpdate = 1;
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            List<User> usersCollection  = DbSetTools.CreateUserList(1,10,miniAutoFixture);
            IQueryable<User> users = usersCollection.AsQueryable(); 
             UserModify userModify = DomainTools.CreateUserModify(miniAutoFixture);
            
            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns( users.GetEnumerator());
           

            var mockContext = new Mock<OrganizerContext>();
            mockContext.Setup(c => c.User).Returns(mockDbSet.Object);
            var logger = new Mock<ILogger<ModifyRepository>>();
            ModifyRepository modifyRepository = new ModifyRepository(mockContext.Object,logger.Object);
           
            
            //action
            UserView userView = modifyRepository.UpdateUser(idUserUpdate,userModify);
            
            //assert
            User userModified = usersCollection.Where(a=>a.Id==idUserUpdate).Single();
            
            mockContext.Verify(a=>a.Attach(It.Is<User>(user=>userModified == user)),Times.Once);
            mockContext.Verify(a=>a.SaveChanges(),Times.Once);
            Assert.Equal(userModify.Name , userView.Name);
            Assert.Equal(userModify.Surname,userView.Surname);
            Assert.Equal(userModified.Deleted,userView.Deleted);
            Assert.Equal(idUserUpdate,userView.Id);
        }



        [Fact]
        public void DeleteUser_MockDbSetUser_AndGetDeleteUser()
        {
            //assign
            int idUserDeleted = 1;
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            List<User> usersCollection  = DbSetTools.CreateUserList(1,10,miniAutoFixture);
            IQueryable<User> users = usersCollection.AsQueryable(); 
            
            
            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns( users.GetEnumerator());
           

            var mockContext = new Mock<OrganizerContext>();
            mockContext.Setup(c => c.User).Returns(mockDbSet.Object);
            var logger = new Mock<ILogger<ModifyRepository>>();
            ModifyRepository modifyRepository = new ModifyRepository(mockContext.Object,logger.Object);
           
            
            //action
            UserView userView = modifyRepository.DeleteUser(idUserDeleted);
            
            //assert
            User userModified = usersCollection.Where(a=>a.Id==idUserDeleted).Single();
            
            mockContext.Verify(a=>a.Attach(It.Is<User>(user=>userModified == user)),Times.Once);
            mockContext.Verify(a=>a.SaveChanges(),Times.Once);
            Assert.Equal(userModified.Name , userView.Name);
            Assert.Equal(userModified.Surname,userView.Surname);
            Assert.Equal(true,userView.Deleted);
            Assert.Equal(1,userView.Id);
        }
        
        [Fact]
        public void InsertTask_MockDbSetUserDbSetTask_AndGetInsertTask()
        {
         
             //assign
            int newIdTask = 41;
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();

            User userSpecified = DbSetTools.CreateUser(1,miniAutoFixture);
            User user2 = DbSetTools.CreateUser(2,miniAutoFixture);
            User user3 = DbSetTools.CreateUser(3,miniAutoFixture);
             List<Task> tasksCollection = DbSetTools.CreateTaskList(1,userSpecified,20,miniAutoFixture)
            .Concat(DbSetTools.CreateTaskList(21,user2,10,miniAutoFixture))
            .Concat(DbSetTools.CreateTaskList(31,user3,10,miniAutoFixture))     
            .ToList();

            IQueryable<Task> tasks = tasksCollection.AsQueryable();
            List<User> userCollection = new User[]{userSpecified , user2,user3}.ToList();
            IQueryable<User> users = userCollection.AsQueryable();

            var mockDbSetUser = new Mock<DbSet<User>>();
            var mockDbSetTask = new Mock<DbSet<Task>>();

            mockDbSetUser.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSetUser.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSetUser.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSetUser.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns( users.GetEnumerator());

            mockDbSetTask.As<IQueryable<Task>>().Setup(m => m.Provider).Returns(tasks.Provider);
            mockDbSetTask.As<IQueryable<Task>>().Setup(m => m.Expression).Returns(tasks.Expression);
            mockDbSetTask.As<IQueryable<Task>>().Setup(m => m.ElementType).Returns(tasks.ElementType);
            mockDbSetTask.As<IQueryable<Task>>().Setup(m => m.GetEnumerator()).Returns(tasks.GetEnumerator());
            
            var mockContext = new Mock<OrganizerContext>();
            mockContext.Setup(c=>c.User).Returns(mockDbSetUser.Object);
            mockContext.Setup(c => c.Tasks).Returns(mockDbSetTask.Object);
            mockDbSetTask.Setup(a=>a.Add(It.IsAny<Task>()))
            .Callback<Task>(
            task=>{
                task.Id = newIdTask;
                tasksCollection.Add(task);
            });
            
            var logger = new Mock<ILogger<ModifyRepository>>();
            ModifyRepository modifyRepository = new ModifyRepository(mockContext.Object,logger.Object);
            TaskModify taskModify = DomainTools.CreateTaskModify(miniAutoFixture);


            // action
            TaskView taskView = modifyRepository.InsertTask(userSpecified.Id,taskModify);
           
            //assert
            mockDbSetTask.Verify
            (
                a=> a.Add
                (
                    It.Is<Task>
                    (
                        task =>
                        task.Id == newIdTask
                        && task.Date == task.Date 
                        && !task.Deleted
                        && !task.Done
                        && task.Text == taskModify.Text
                        && task.User == userSpecified
                    )
                ),
                Times.Once
            );
            
            mockContext.Verify(a=>a.SaveChanges(),Times.Once);
            
            Assert.Equal(taskModify.Text , taskView.Text);
            Assert.Equal(taskModify.Date , taskView.Date);
            Assert.Equal(false,taskView.Done);
            Assert.Equal(false,taskView.Deleted);
            Assert.Equal(newIdTask , taskView.Id);

        }

        [Fact]
          public void UpdateTask_MockDbSetTask_AndGetUpdateTask()
         {
         
             //assign
            int idTaskModify = 1;
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            User user = DbSetTools.CreateUser(1,miniAutoFixture);
            List<Task> tasksCollection = DbSetTools.CreateTaskList(1,user,10,miniAutoFixture);
            IQueryable<Task> tasks = tasksCollection.AsQueryable();
            var mockDbSetTask = new Mock<DbSet<Task>>();
            mockDbSetTask.As<IQueryable<Task>>().Setup(m => m.Provider).Returns(tasks.Provider);
            mockDbSetTask.As<IQueryable<Task>>().Setup(m => m.Expression).Returns(tasks.Expression);
            mockDbSetTask.As<IQueryable<Task>>().Setup(m => m.ElementType).Returns(tasks.ElementType);
            mockDbSetTask.As<IQueryable<Task>>().Setup(m => m.GetEnumerator()).Returns(tasks.GetEnumerator());
            
            var mockContext = new Mock<OrganizerContext>();
            mockContext.Setup(c => c.Tasks).Returns(mockDbSetTask.Object);
            var logger = new Mock<ILogger<ModifyRepository>>();
            ModifyRepository modifyRepository = new ModifyRepository(mockContext.Object,logger.Object);
            TaskModify taskModify = DomainTools.CreateTaskModify(miniAutoFixture);
            
            //action
            TaskView taskView = modifyRepository.UpdateTask(idTaskModify,taskModify);

            //assert
            Task taskModified = tasksCollection.Where(a=>a.Id == idTaskModify).Single();
            mockContext.Verify(a=>a.Attach(It.Is<Task>(task=>task==taskModified)),Times.Once);
            mockContext.Verify(a=>a.SaveChanges(),Times.Once);

            Assert.Equal(taskModify.Date,taskView.Date);
            Assert.Equal(taskModify.Text,taskView.Text);
            Assert.Equal(taskModified.Deleted,taskView.Deleted);
            Assert.Equal(taskModified.Done,taskView.Done);
            Assert.Equal(idTaskModify,taskView.Id);
         }

         [Fact]
         public void DeleteTask_MockDbSetTask_AndGetDeleteTask()
         {
         
             //assign
            int idTaskModify = 1;
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            User user = DbSetTools.CreateUser(1,miniAutoFixture);
            List<Task> tasksCollection = DbSetTools.CreateTaskList(1,user,10,miniAutoFixture);
            IQueryable<Task> tasks = tasksCollection.AsQueryable();
            var mockDbSetTask = new Mock<DbSet<Task>>();
            mockDbSetTask.As<IQueryable<Task>>().Setup(m => m.Provider).Returns(tasks.Provider);
            mockDbSetTask.As<IQueryable<Task>>().Setup(m => m.Expression).Returns(tasks.Expression);
            mockDbSetTask.As<IQueryable<Task>>().Setup(m => m.ElementType).Returns(tasks.ElementType);
            mockDbSetTask.As<IQueryable<Task>>().Setup(m => m.GetEnumerator()).Returns(tasks.GetEnumerator());
            
            var mockContext = new Mock<OrganizerContext>();
            mockContext.Setup(c => c.Tasks).Returns(mockDbSetTask.Object);
            var logger = new Mock<ILogger<ModifyRepository>>();
            ModifyRepository modifyRepository = new ModifyRepository(mockContext.Object,logger.Object);
         
            
            //action
            TaskView taskView = modifyRepository.DeleteTask(idTaskModify);

            //assert
            Task taskModified = tasksCollection.Where(a=>a.Id == idTaskModify).Single();
            mockContext.Verify(a=>a.Attach(It.Is<Task>(task=>task==taskModified)),Times.Once);
            mockContext.Verify(a=>a.SaveChanges(),Times.Once);

            Assert.Equal(taskModified.Date,taskView.Date);
            Assert.Equal(taskModified.Text,taskView.Text);
            Assert.Equal(true,taskView.Deleted);
            Assert.Equal(taskModified.Done,taskView.Done);
            Assert.Equal(idTaskModify,taskView.Id);
         }

         [Fact]
        public void DoneTask_MockDbSetTask_AndGetDoneTask()
         {
         
             //assign
            int idTaskModify = 1;
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            User user = DbSetTools.CreateUser(1,miniAutoFixture);
            List<Task> tasksCollection = DbSetTools.CreateTaskList(1,user,10,miniAutoFixture);
            IQueryable<Task> tasks = tasksCollection.AsQueryable();
            var mockDbSetTask = new Mock<DbSet<Task>>();
            mockDbSetTask.As<IQueryable<Task>>().Setup(m => m.Provider).Returns(tasks.Provider);
            mockDbSetTask.As<IQueryable<Task>>().Setup(m => m.Expression).Returns(tasks.Expression);
            mockDbSetTask.As<IQueryable<Task>>().Setup(m => m.ElementType).Returns(tasks.ElementType);
            mockDbSetTask.As<IQueryable<Task>>().Setup(m => m.GetEnumerator()).Returns(tasks.GetEnumerator());
            
            var mockContext = new Mock<OrganizerContext>();
            mockContext.Setup(c => c.Tasks).Returns(mockDbSetTask.Object);
            var logger = new Mock<ILogger<ModifyRepository>>();
            ModifyRepository modifyRepository = new ModifyRepository(mockContext.Object,logger.Object);
            bool done = miniAutoFixture.CreateBoolean();
            
            //action
            TaskView taskView = modifyRepository.DoneTask(idTaskModify,done);

            //assert
            Task taskModified = tasksCollection.Where(a=>a.Id == idTaskModify).Single();
            mockContext.Verify(a=>a.Attach(It.Is<Task>(task=>task==taskModified)),Times.Once);
            mockContext.Verify(a=>a.SaveChanges(),Times.Once);

            Assert.Equal(taskModified.Date,taskView.Date);
            Assert.Equal(taskModified.Text,taskView.Text);
            Assert.Equal(taskModified.Deleted,taskView.Deleted);
            Assert.Equal(done,taskView.Done);
            Assert.Equal(idTaskModify,taskView.Id);
         }



    }
}