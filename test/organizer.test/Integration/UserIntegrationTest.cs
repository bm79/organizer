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
    public class UserIntegrationTest:IntegrationTest {
        
        [Fact]  
        public void GetAllUser_RunServerAddUserList_ReturnUserListJson()
        {
          DoIntegrationTest(async (client,dbContextOptions) =>
            {
                //Arrange
                MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
                miniAutoFixture.Initialize();
                              
                List<User> userList = DbSetTools.AddExampleUsersToDatabase(dbContextOptions,miniAutoFixture);
                var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/user");

                //Action
                var response = await client.SendAsync(request);

                //Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var content = await response.Content.ReadAsStringAsync();
                
                List<UserView> userViewList = JsonConvert.DeserializeObject<List<UserView>>(content);
                Assert.Equal(userList.Count,userViewList.Count);
                userList.ForEach(
                    user=>
                    {
                        Assert.Equal(userViewList
                        .Count
                        (
                            userView => 
                            user.Id == userView.Id 
                            && user.Name == userView.Name
                            && user.Surname == userView.Surname 
                            && user.Deleted == userView.Deleted    
                        ),
                       1);
                    }
               );

                }
            );
        }

    
        [Fact]
        public void GetUser_RunServerAddUserList_GetUser()
        {
             DoIntegrationTest(async (client,dbContextOptions) =>
            {
                //Arrange
                MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
                miniAutoFixture.Initialize();
                List<User> userList = DbSetTools.AddExampleUsersToDatabase(dbContextOptions,miniAutoFixture);
                User userExample = userList[0];
                UserModify userModify = DomainTools.CreateUserModify(miniAutoFixture);
                var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("/api/user/{0}",userExample.Id));

                //Action
                var response = await client.SendAsync(request);

                //Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var content = await response.Content.ReadAsStringAsync();
                UserView userView = JsonConvert.DeserializeObject<UserView>(content);
                
                Assert.Equal(userExample.Id,userView.Id); 
                Assert.Equal(userExample.Name,userView.Name);
                Assert.Equal(userExample.Surname,userView.Surname);     
                Assert.Equal(userExample.Deleted,userView.Deleted);     

            });
        }


        [Fact]
        public void PostUser_RunServerAddUserList_GetAddUser()
        {
             DoIntegrationTest(async (client,dbContextOptions) =>
            {
                //Arrange
                MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
                miniAutoFixture.Initialize();
                List<User> userList = DbSetTools.AddExampleUsersToDatabase(dbContextOptions,miniAutoFixture);
                UserModify userModify = DomainTools.CreateUserModify(miniAutoFixture);
                
                //Action
                var response = await client.PostAsJsonAsync("/api/user",userModify);
              

                //Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var content = await response.Content.ReadAsStringAsync();
                UserView userView = JsonConvert.DeserializeObject<UserView>(content);
                Assert.Equal(userModify.Name,userView.Name);
                Assert.Equal(userModify.Surname,userView.Surname);
                Assert.Equal(false,userView.Deleted);
                Assert.Equal(true,userView.Id>0);
            });
        }

        [Fact]
        public void PutUser_RunServerAddUserList_GetUpdateUser()
        {
             DoIntegrationTest(async (client,dbContextOptions) =>
            {
                //Arrange
                MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
                miniAutoFixture.Initialize();
                List<User> userList = DbSetTools.AddExampleUsersToDatabase(dbContextOptions,miniAutoFixture);
                User userExample = userList[0];

                UserModify userModify = DomainTools.CreateUserModify(miniAutoFixture);
                
                //Action
                var response = await client.PutAsJsonAsync(string.Format("/api/user/{0}",userExample.Id) ,userModify);
              

                //Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var content = await response.Content.ReadAsStringAsync();
                UserView userView = JsonConvert.DeserializeObject<UserView>(content);
                Assert.Equal(userModify.Name,userView.Name);
                Assert.Equal(userModify.Surname,userView.Surname);
                Assert.Equal(userExample.Deleted,userView.Deleted);
                Assert.Equal(userExample.Id,userView.Id);
            });
        }
        [Fact]
         public void DeleteUser_RunServerAddUserList_GetDeleteUser()
        {
             DoIntegrationTest(async (client,dbContextOptions) =>
            {
                //Arrange
                MiniAutoFixture miniAutoFixture = new MiniAutoFixture();
                miniAutoFixture.Initialize();
                List<User> userList = DbSetTools.AddExampleUsersToDatabase(dbContextOptions,miniAutoFixture);
                User userExample = userList[0];

               
                
                //Action
                var response = await client.DeleteAsync(string.Format("/api/user/{0}",userExample.Id));
              

                //Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var content = await response.Content.ReadAsStringAsync();
                UserView userView = JsonConvert.DeserializeObject<UserView>(content);
                Assert.Equal(userExample.Name,userView.Name);
                Assert.Equal(userExample.Surname,userView.Surname);
                Assert.Equal(true,userView.Deleted);
                Assert.Equal(userExample.Id,userView.Id);
            });
        }


    }
}