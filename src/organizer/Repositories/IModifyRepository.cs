using Organizer.Models.Modify;
using Organizer.Models.View;

namespace Organizer.Repositories {
    public interface IModifyRepository {
        UserView InsertUser(UserModify userModify);
        UserView DeleteUser(int id);
        UserView UpdateUser(int id , UserModify userModify);
        TaskView InsertTask(int idUser , TaskModify taskModify);
        TaskView UpdateTask(int id , TaskModify taskModify);
        TaskView DeleteTask(int id);
        TaskView DoneTask(int id , bool doneTask);
        
    }
}