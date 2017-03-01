using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Organizer.Context;

namespace Organizer.Test.Tools
{
    public class DbSetTools 
    {
        public static User CreateUser(int id , MiniAutoFixture miniAutoFixtue)
        {
            return new User {
                Id = id,
                Name = miniAutoFixtue.CreateString(),
                Surname = miniAutoFixtue.CreateString(),
                Deleted = miniAutoFixtue.CreateBoolean()
            };
        }

       
        public static Task CreateTask(int id , User user,MiniAutoFixture miniAutoFixtue)
        {
            
            return new Task {
                Id = id,
                Date = miniAutoFixtue.CreateDatetime(),
                Text = miniAutoFixtue.CreateString(),
                Done = miniAutoFixtue.CreateBoolean(),
                User = user,
                Deleted = miniAutoFixtue.CreateBoolean()
            };
        }

        public static List<User> CreateUserList(int startUserId , int userSize, MiniAutoFixture miniAutoFixtue)
        {
            return Enumerable.Range(startUserId,userSize).Select(a=>CreateUser(a,miniAutoFixtue)).ToList();
        }

        public static List<Task> CreateTaskList(int startTaskId , User user , int taskSize, MiniAutoFixture miniAutoFixtue)
        {
           return
                Enumerable
                .Range(startTaskId,taskSize)
                .Select(a=>CreateTask(a,user,miniAutoFixtue))
                .ToList();
              
        }

        public static List<User> AddExampleUsersToDatabase(DbContextOptions<OrganizerContext> dbContextOptions , MiniAutoFixture miniAutoFixture)
        {
            
             using (OrganizerContext organizerContext = new OrganizerContext(dbContextOptions))
             {

                List<User> userList = Enumerable.Range(1,10).Select(a=>CreateUser(0,miniAutoFixture)).ToList();
                userList.ForEach(user=>organizerContext.Add(user)); 
                organizerContext.SaveChanges();
                return userList;
             }
        }

        public static List<Task> AddExampleTasksToDatabase(DbContextOptions<OrganizerContext> dbContextOptions , MiniAutoFixture miniAutoFixture,out List<User> userList)
        {
            using (OrganizerContext organizerContext = new OrganizerContext(dbContextOptions))
            {
                 userList = new List<User>();
                 User user1 = DbSetTools.CreateUser(0,miniAutoFixture);
                 organizerContext.Add(user1);
                 userList.Add(user1);

                 User user2 = DbSetTools.CreateUser(0,miniAutoFixture);
                 organizerContext.Add(user2); 
                 userList.Add(user2);

                 User user3 = DbSetTools.CreateUser(0,miniAutoFixture);
                 organizerContext.Add(user3); 
                 userList.Add(user3);
                
                 List<Task> taskList = Enumerable
                .Range(1,10)
                .Select(a=>CreateTask(0,user1,miniAutoFixture))
                .ToList()
                .Concat(
                    Enumerable.Range(11,10)
                    .Select(a=>CreateTask(0,user2,miniAutoFixture))
                    .ToList()
                )
                .Concat(
                    Enumerable.Range(21,10)
                    .Select(a=>CreateTask(0,user3,miniAutoFixture))
                    .ToList()
                )
                .ToList();



                 taskList.ForEach(task=>organizerContext.Add(task));
                 organizerContext.SaveChanges();
                 return taskList;
            }
        }
       
    }
}