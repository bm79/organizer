using System.Collections.Generic;
using Organizer.Models.View;

namespace Organizer.Repositories {
    public interface IViewRepository {
        IEnumerable<UserView> GetAllUsers();
        UserView GetUser(int id);
        IEnumerable<TaskView> GetAllTasks();
        IEnumerable<TaskView> GetAllUserTask(int idUser);
        TaskView GetTask(int id);
        
    }
}