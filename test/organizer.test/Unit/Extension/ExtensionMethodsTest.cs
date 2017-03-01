using Organizer.Context;
using Organizer.Extensions;
using Organizer.Models.Modify;
using Organizer.Models.View;
using Xunit;
using Organizer.Test.Tools;

namespace Organizer.Test.Unit.Extension {
    public class ExtensionMethodTest
    {
        [Fact]
        public void MapUserToView_WhenSetUser_ReturnUserView()
        {
            //assign
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            int id = miniAutoFixture.CreateInt();

            User user = DbSetTools.CreateUser( id , miniAutoFixture);
            
            //action
            UserView userView = ExtensionMethods.MapUserToView(user);

            //assert
            Assert.Equal(user.Id,userView.Id);
            Assert.Equal(user.Name,userView.Name);
            Assert.Equal(user.Surname,userView.Surname);
            Assert.Equal(user.Deleted,userView.Deleted);
        }

        [Fact]
        public void MapTaskToView_WhenSetTask_ReturnTaskView()
        {
            //assign
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
           
            int id = miniAutoFixture.CreateInt();

            Task task = DbSetTools.CreateTask(id,null,miniAutoFixture);

            //action
            TaskView taskView = ExtensionMethods.MapTaskToView(task);

            //assert
            Assert.Equal(task.Id,taskView.Id);
            Assert.Equal(task.Date,taskView.Date);
            Assert.Equal(task.Text,taskView.Text);
            Assert.Equal(task.Done,taskView.Done);
            Assert.Equal(task.Deleted,taskView.Deleted);

        }

        [Fact]
        public void SaveUserModify_WhenSetUserModify_ChangeUserWithoutChangeAnythingElse()
        {
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            
            int id = miniAutoFixture.CreateInt();
            bool deleted = miniAutoFixture.CreateBoolean();
            User user = new User {
                Id = id,
                Deleted = deleted,
            };

            UserModify userModify = new UserModify {
                Name = miniAutoFixture.CreateString(),
                Surname = miniAutoFixture.CreateString()
            };

            // action
            ExtensionMethods.SaveUserModify(user,userModify);

            // assert
            Assert.Equal(id,user.Id);
            Assert.Equal(userModify.Name , user.Name);
            Assert.Equal(userModify.Surname,user.Surname);
            Assert.Equal(deleted,user.Deleted);
        
        }

        [Fact]
        public void SaveTaskModify_WhenSetTaskModify_ChangeTaskWithoutChangeAnythingElse()
        {
            //arrange
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            int id = miniAutoFixture.CreateInt();
            bool deleted = miniAutoFixture.CreateBoolean();
            bool done = miniAutoFixture.CreateBoolean();

            Task task = new Task
             {
                Id = id ,
                Done = done ,
                Deleted = deleted
            };

            TaskModify taskModify = new TaskModify
            {
                Date = miniAutoFixture.CreateDatetime(),
                Text = miniAutoFixture.CreateString()
            };
            
            //action
            ExtensionMethods.SaveTaskModify(task,taskModify);

            //assert
            Assert.Equal(id,task.Id);
            Assert.Equal(taskModify.Date , task.Date);
            Assert.Equal(taskModify.Text, task.Text);
            Assert.Equal(done,task.Done);
            Assert.Equal(deleted, task.Deleted);
        }
    }
}