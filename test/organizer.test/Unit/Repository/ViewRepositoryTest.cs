using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Organizer.Context;
using Organizer.Models.View;
using Organizer.Repositories;
using Xunit;
using Organizer.Test.Tools;


namespace Organizer.Test.Unit.Repository 
{
    public class ViewRepositoryTest 
    {
        [Fact]
        public void GetAllUsers_MockDbSetUserAndContext_AndGetAllUsers()
        {
            //assign
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            List<User> usersCollection = DbSetTools.CreateUserList(1,100,miniAutoFixture);
            IQueryable<User> users = usersCollection.AsQueryable(); 
            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns( users.GetEnumerator());
            
            var mockContext = new Mock<OrganizerContext>();
            mockContext.Setup(c => c.User).Returns(mockDbSet.Object);
            var logger = new Mock<ILogger<ViewRepository>>();
            ViewRepository viewRepository = new ViewRepository(mockContext.Object,logger.Object);
            
            //action
            IEnumerable<UserView> userList =  viewRepository.GetAllUsers();

            //assert
            Assert.Equal(users.Count(),userList.Count());
             users.ToList().ForEach(user=>
             {
                 
                    Assert.Equal
                    (
                        userList.Count
                            ( 
                                 userView => 
                                 userView.Id == user.Id  
                                 && userView.Name == user.Name 
                                 && userView.Surname == user.Surname
                                 && userView.Deleted == user.Deleted
                       
                            ),
                        1
                    );
            }
           );

             
        }

        public void GetUser_MockDbSetUserAndContext_AndGetUser()
        {
            //assign
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            
            User userSpecified = DbSetTools.CreateUser(101,miniAutoFixture);
            List<User> usersCollection = 
                DbSetTools.CreateUserList(1,100,miniAutoFixture)
                .Concat(new User[]{userSpecified}).ToList();
            

            IQueryable<User> users = usersCollection.AsQueryable(); 
            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns( users.GetEnumerator());
            
            var mockContext = new Mock<OrganizerContext>();
            mockContext.Setup(c => c.User).Returns(mockDbSet.Object);
            var logger = new Mock<ILogger<ViewRepository>>();
            ViewRepository viewRepository = new ViewRepository(mockContext.Object,logger.Object);
            
            //action
            UserView userView = viewRepository.GetUser(userSpecified.Id);

            //assert
            Assert.Equal(userSpecified.Id, userView.Id);  
            Assert.Equal(userSpecified.Name , userView.Name);
            Assert.Equal(userSpecified.Surname , userView.Surname); 
            Assert.Equal(userSpecified.Deleted , userView.Deleted);
                                   
        }


        [Fact]
        public void GetAllTasks_MockDbSetTasksAndContext_AndGetAllTasks()
        {
            //assign
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();

            User user1 = DbSetTools.CreateUser(1,miniAutoFixture);
            User user2 = DbSetTools.CreateUser(1,miniAutoFixture);

            List<Task> tasksCollection = DbSetTools.CreateTaskList(1,user1,10,miniAutoFixture)
            .Concat(DbSetTools.CreateTaskList(11,user2,20,miniAutoFixture))
            .ToList();
          
            IQueryable<Task> tasks = tasksCollection.AsQueryable();

            var mockDbSet = new Mock<DbSet<Task>>();
            mockDbSet.As<IQueryable<Task>>().Setup(m => m.Provider).Returns(tasks.Provider);
            mockDbSet.As<IQueryable<Task>>().Setup(m => m.Expression).Returns(tasks.Expression);
            mockDbSet.As<IQueryable<Task>>().Setup(m => m.ElementType).Returns(tasks.ElementType);
            mockDbSet.As<IQueryable<Task>>().Setup(m => m.GetEnumerator()).Returns(tasks.GetEnumerator());
            
            var mockContext = new Mock<OrganizerContext>();
            mockContext.Setup(c => c.Tasks).Returns(mockDbSet.Object);
            var logger = new Mock<ILogger<ViewRepository>>();
            ViewRepository viewRepository = new ViewRepository(mockContext.Object,logger.Object);
            
            //action
            IEnumerable<TaskView> taskList = viewRepository.GetAllTasks();    
            
            //assert
            Assert.Equal(tasks.Count(),taskList.Count());
            tasks.ToList().ForEach(task=>
             {
                 
                    Assert.Equal
                    (
                        taskList.Count
                            ( 
                                 taskView => 
                                 taskView.Id == task.Id
                                 && taskView.Text == task.Text
                                 && taskView.Date == task.Date
                                 && taskView.Deleted == task.Deleted
                                 && taskView.Done == task.Done
                            ),
                        1
                    );
            }
           );
        }

        [Fact]
        public void GetAllTasks_MockDbSetTasksAndContext_AndGetAllUserTask()
        {
            //assign
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
            var logger = new Mock<ILogger<ViewRepository>>();
            ViewRepository viewRepository = new ViewRepository(mockContext.Object,logger.Object);
            
            //action
            IEnumerable<TaskView> taskList = viewRepository.GetAllUserTask(userSpecified.Id);
            
            //assign
            IEnumerable<Task> tasksUser = tasks.Where(a=>a.User.Id == userSpecified.Id);
            Assert.Equal(tasksUser.Count() ,taskList.Count());
            tasksUser.ToList().ForEach(task=>
             {
                 
                    Assert.Equal
                    (
                        taskList.Count
                            ( 
                                 taskView => 
                                 taskView.Id == task.Id
                                 && taskView.Text == task.Text
                                 && taskView.Date == task.Date
                                 && taskView.Deleted == task.Deleted
                                 && taskView.Done == task.Done
                            ),
                        1
                    );
            }
           );
        }

        [Fact]
        public void GetAllTasks_MockDbSetTasksAndContext_AndGetTask()
        {
             //assign
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();

            User user = DbSetTools.CreateUser(11,miniAutoFixture);
            Task taskSpecified = DbSetTools.CreateTask(11,user,miniAutoFixture);

            List<Task> tasksCollection = DbSetTools.CreateTaskList(1,user,10,miniAutoFixture)
            .Concat(new Task[]{taskSpecified})
            .ToList();


            IQueryable<Task> tasks = tasksCollection.AsQueryable();

            var mockDbSet = new Mock<DbSet<Task>>();
            mockDbSet.As<IQueryable<Task>>().Setup(m => m.Provider).Returns(tasks.Provider);
            mockDbSet.As<IQueryable<Task>>().Setup(m => m.Expression).Returns(tasks.Expression);
            mockDbSet.As<IQueryable<Task>>().Setup(m => m.ElementType).Returns(tasks.ElementType);
            mockDbSet.As<IQueryable<Task>>().Setup(m => m.GetEnumerator()).Returns(tasks.GetEnumerator());
            
            var mockContext = new Mock<OrganizerContext>();
            mockContext.Setup(c => c.Tasks).Returns(mockDbSet.Object);
            var logger = new Mock<ILogger<ViewRepository>>();
            ViewRepository viewRepository = new ViewRepository(mockContext.Object,logger.Object);


            // action
            TaskView taskView = viewRepository.GetTask(taskSpecified.Id);

            // assign
            Assert.Equal(taskSpecified.Id, taskView.Id);  
            Assert.Equal(taskSpecified.Text, taskView.Text);
            Assert.Equal(taskSpecified.Date,taskView.Date);
            Assert.Equal(taskSpecified.Deleted, taskView.Deleted);
            Assert.Equal(taskSpecified.Done, taskView.Done);        
            
        }
        
        
    
    }
}