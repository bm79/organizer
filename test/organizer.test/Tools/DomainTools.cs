using System.Collections.Generic;
using System.Linq;
using Organizer.Models.Modify;
using Organizer.Models.View;

namespace Organizer.Test.Tools
{
    public class DomainTools 
    {
        public static TaskView GetTask(MiniAutoFixture miniAutoFixture)
        {
            return new TaskView()
            {
                Id = miniAutoFixture.CreateInt(),
                Text = miniAutoFixture.CreateString(),
                Date = miniAutoFixture.CreateDatetime(),
                Done = miniAutoFixture.CreateBoolean(),
                Deleted = miniAutoFixture.CreateBoolean()
            };
        }

        public static IEnumerable<TaskView> GetTaskList(MiniAutoFixture miniAutoFixture,int taskSize)
        {
           return Enumerable.Range(1,taskSize).Select(a=> new TaskView()
            {
                Id = a,
                Text = miniAutoFixture.CreateString(),
                Date = miniAutoFixture.CreateDatetime(),
                Done = miniAutoFixture.CreateBoolean(),
                Deleted = miniAutoFixture.CreateBoolean()
            }).ToList();
        }



        public static UserView GetUser(MiniAutoFixture miniAutoFixture)
        {

            return new UserView()
            {
                Id = miniAutoFixture.CreateInt(),
                Name = miniAutoFixture.CreateString(),
                Surname = miniAutoFixture.CreateString(),
                Deleted = miniAutoFixture.CreateBoolean()
            };
        }

        public static IEnumerable<UserView> GetUserList(MiniAutoFixture miniAutoFixture , int userSize)
        {
            return Enumerable.Range(1,userSize).Select(a=>new UserView{
                Id = a,
                Name = miniAutoFixture.CreateString(),
                Surname = miniAutoFixture.CreateString(),
                Deleted = miniAutoFixture.CreateBoolean()
            }).ToList();
        }


        public static UserModify CreateUserModify(MiniAutoFixture miniAutoFixture)
        {
            return new UserModify(){
                Name =  miniAutoFixture.CreateString(30),
                Surname = miniAutoFixture.CreateString(50)
            };
        }

        public static TaskModify CreateTaskModify(MiniAutoFixture miniAutoFixture)
        {
            return new TaskModify()
            {
                Text = miniAutoFixture.CreateString(255),
                Date = miniAutoFixture.CreateDatetime()
            };
        }
    }

}