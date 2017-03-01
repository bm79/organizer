using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Organizer.Context;
using Organizer.Models.View;
using Organizer.Test.Tools;
using Xunit;
using System.Linq;
using Organizer.Models.Modify;

namespace Organizer.Test.Integration {
    public class TaskIntegrationTest:IntegrationTest {
        [Fact]  
        public void GetTask_RunServerAddUserListAndTask_ReturnTaskListJson()
        {
          DoIntegrationTest(async (client,dbContextOptions) =>
            {
                //Arrange
                MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
                miniAutoFixture.Initialize();
                List<User> userList = null;

                List<Task> taskList = DbSetTools.AddExampleTasksToDatabase(dbContextOptions,miniAutoFixture,out userList);
                var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/task");

                //Action
                var response = await client.SendAsync(request);

                //Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var content = await response.Content.ReadAsStringAsync();
                
                List<TaskView> taskViewList = JsonConvert.DeserializeObject<List<TaskView>>(content);
                Assert.Equal(taskList.Count,taskViewList.Count);
                
                taskList.ForEach(
                    task=>
                    {
                        Assert.Equal(taskViewList
                        .Count
                        (
                            taskView=>
                                task.Id == taskView.Id
                                && task.Text == taskView.Text
                                && task.Date == taskView.Date
                                && task.Deleted == taskView.Deleted
                                && task.Done == taskView.Done   
                        ),
                       1);
                    }
               );

                }
            );
        }
        
        [Fact]
        public void GetUserTask_RunServerAddUserListAndTask_ReturnTaskListJson()
        {
            DoIntegrationTest(async (client,dbContextOptions) =>
            {
             //Arrange
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            List<User> userList = null;
            List<Task> taskList = DbSetTools.AddExampleTasksToDatabase(dbContextOptions,miniAutoFixture,out userList);
            User userExample = userList[0];
            List<Task> userTaskList = taskList.Where(a=>a.User==userExample).ToList();
            var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("/api/task/user/{0}",userExample.Id));
            
            //Action
            var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
                
            List<TaskView> taskViewList = JsonConvert.DeserializeObject<List<TaskView>>(content);

            Assert.Equal(userTaskList.Count(),taskViewList.Count());
            userTaskList.ForEach(
                    task=>
                    {
                        Assert.Equal(taskViewList
                        .Count
                        (
                            taskView=>
                                task.Id == taskView.Id
                                && task.Text == taskView.Text
                                && task.Date == taskView.Date
                                && task.Deleted == taskView.Deleted
                                && task.Done == taskView.Done   
                        ),
                       1);
                    }
               );
            
            });
        }

        [Fact]
        public void PostTaskRunServerAddUserListAndTask_ReturnTaskAdd()
        {
             DoIntegrationTest(async (client,dbContextOptions) =>
            {
            //Arrange
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            List<User> userList = null;
            List<Task> taskList = DbSetTools.AddExampleTasksToDatabase(dbContextOptions,miniAutoFixture,out userList);
            User userExample = userList[0];
            TaskModify taskModify = DomainTools.CreateTaskModify(miniAutoFixture);

            //Action
            var response = await client.PostAsJsonAsync(string.Format("/api/task/user/{0}",userExample.Id),taskModify);
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            TaskView taskView = JsonConvert.DeserializeObject<TaskView>(content);
            Assert.Equal(taskModify.Text,taskView.Text);
            Assert.Equal(taskModify.Date,taskView.Date);
            Assert.Equal(false,taskView.Deleted);
            Assert.Equal(false,taskView.Done);
            Assert.Equal(true,taskView.Id>0);
            
         });
        }


        [Fact]
        public void PutTaskRunServerAddUserListAndTask_ReturnTaskAdd()
        {
             DoIntegrationTest(async (client,dbContextOptions) =>
            {
            //Arrange
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            List<User> userList = null;
            List<Task> taskList = DbSetTools.AddExampleTasksToDatabase(dbContextOptions,miniAutoFixture,out userList);
            Task taskExample = taskList[0];
            TaskModify taskModify = DomainTools.CreateTaskModify(miniAutoFixture);

            //Action
            var response = await client.PutAsJsonAsync(string.Format("/api/task/{0}",taskExample.Id),taskModify);
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            TaskView taskView = JsonConvert.DeserializeObject<TaskView>(content);
            Assert.Equal(taskModify.Text,taskView.Text);
            Assert.Equal(taskModify.Date,taskView.Date);
            Assert.Equal(taskExample.Deleted,taskView.Deleted);
            Assert.Equal(taskExample.Done,taskView.Done);
            Assert.Equal(taskExample.Id,taskView.Id);
            
         });
        }


        [Fact]
        public void DeleteTask_RunServerAddUserListAndTask_ReturnTaskAdd()
        {
             DoIntegrationTest(async (client,dbContextOptions) =>
            {
            //Arrange
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            List<User> userList = null;
            List<Task> taskList = DbSetTools.AddExampleTasksToDatabase(dbContextOptions,miniAutoFixture,out userList);
            Task taskExample = taskList[0];
            
            //Action
            var response = await client.DeleteAsync(string.Format("/api/task/{0}",taskExample.Id));
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            TaskView taskView = JsonConvert.DeserializeObject<TaskView>(content);
            Assert.Equal(taskExample.Text,taskView.Text);
            Assert.Equal(taskExample.Date,taskView.Date);
            Assert.Equal(true,taskView.Deleted);
            Assert.Equal(taskExample.Done,taskView.Done);
            Assert.Equal(taskExample.Id,taskView.Id);
            
         });
        }

        [Fact]
        public void DoneTask_RunServerAddUserListAndTask_ReturnTaskDone()
        {
             DoIntegrationTest(async (client,dbContextOptions) =>
            {
            //Arrange
            bool done = true;
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            List<User> userList = null;
            List<Task> taskList = DbSetTools.AddExampleTasksToDatabase(dbContextOptions,miniAutoFixture,out userList);
            Task taskExample = taskList[0];
            
            //Action
            var response = await client.PutAsync(string.Format("/api/task/done/{0}",taskExample.Id),null);
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            TaskView taskView = JsonConvert.DeserializeObject<TaskView>(content);
            Assert.Equal(taskExample.Text,taskView.Text);
            Assert.Equal(taskExample.Date,taskView.Date);
            Assert.Equal(taskExample.Deleted,taskView.Deleted);
            Assert.Equal(done,taskView.Done);
            Assert.Equal(taskExample.Id,taskView.Id);
            
         });
        }

        [Fact]
        public void UnDoneTask_RunServerAddUserListAndTask_ReturnTaskDone()
        {
             DoIntegrationTest(async (client,dbContextOptions) =>
            {
            //Arrange
            bool done = false;
            MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
            miniAutoFixture.Initialize();
            List<User> userList = null;
            List<Task> taskList = DbSetTools.AddExampleTasksToDatabase(dbContextOptions,miniAutoFixture,out userList);
            Task taskExample = taskList[0];
            
            //Action
            var response = await client.PutAsync(string.Format("/api/task/undone/{0}",taskExample.Id),null);
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            TaskView taskView = JsonConvert.DeserializeObject<TaskView>(content);
            Assert.Equal(taskExample.Text,taskView.Text);
            Assert.Equal(taskExample.Date,taskView.Date);
            Assert.Equal(taskExample.Deleted,taskView.Deleted);
            Assert.Equal(done,taskView.Done);
            Assert.Equal(taskExample.Id,taskView.Id);
            
         });
        }


    }
}