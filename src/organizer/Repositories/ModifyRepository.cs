using Microsoft.Extensions.Logging;
using Organizer.Context;
using Organizer.Models.Modify;
using Organizer.Models.View;
using Organizer.Extensions;
using System.Linq;


namespace Organizer.Repositories
{
    public class ModifyRepository: IModifyRepository
    {
        private OrganizerContext _organizerContext ;

        private ILogger<ModifyRepository> _logger;
        public ModifyRepository(OrganizerContext organizerContext,ILogger<ModifyRepository> logger)
        {
            _organizerContext = organizerContext;
            _logger=logger;
        }

        public UserView InsertUser(UserModify userModify)
        {
            User user = new User { Deleted = false};
            user.SaveUserModify(userModify);
            _organizerContext.User.Add(user);
            _organizerContext.SaveChanges();
            return _organizerContext.User.Where(a=>a.Id==user.Id).Select(a=>a.MapUserToView()).Single();
        }

        public UserView DeleteUser(int id)
        {
            User user = _organizerContext.User.Where(a=>a.Id==id).Single();
            _organizerContext.Attach(user);
             user.Deleted = true;
            _organizerContext.SaveChanges();
            return _organizerContext.User.Where(a=>a.Id==user.Id).Select(a=>a.MapUserToView()).Single();
        }

        public UserView UpdateUser(int id, UserModify userModify)
        {
            User user = _organizerContext.User.Where(a=>a.Id==id).Single();
            _organizerContext.Attach(user);
            user.SaveUserModify(userModify);
            _organizerContext.SaveChanges();
            return _organizerContext.User.Where(a=>a.Id==user.Id).Select(a=>a.MapUserToView()).Single();
        }

        public TaskView InsertTask(int idUser, TaskModify taskModify)
        {
            User user = _organizerContext.User.Where(a=>a.Id==idUser).Single();
            Task task = new Task{ User = user , Done = false, Deleted = false };
            task.SaveTaskModify(taskModify);
            _organizerContext.Tasks.Add(task);
            _organizerContext.SaveChanges();
            return _organizerContext.Tasks.Where(a=>a.Id==task.Id).Select(a=>a.MapTaskToView()).Single();
        }

        public TaskView UpdateTask(int id , TaskModify taskModify)
        {
            Task task = _organizerContext.Tasks.Where(a=>a.Id==id).Single();
             _organizerContext.Attach(task);
            task.SaveTaskModify(taskModify);
             _organizerContext.SaveChanges();
             return _organizerContext.Tasks.Where(a=>a.Id==task.Id).Select(a=>a.MapTaskToView()).Single();
        }

        public TaskView DeleteTask(int id)
        {
             Task task = _organizerContext.Tasks.Where(a=>a.Id==id).Single();
             _organizerContext.Attach(task);
             task.Deleted = true;
             _organizerContext.SaveChanges();
              return _organizerContext.Tasks.Where(a=>a.Id==task.Id).Select(a=>a.MapTaskToView()).Single();
        }

        public TaskView DoneTask(int id, bool doneTask)
        {
             Task task = _organizerContext.Tasks.Where(a=>a.Id==id).Single();
             _organizerContext.Attach(task);
             task.Done = doneTask;
             _organizerContext.SaveChanges();
              return _organizerContext.Tasks.Where(a=>a.Id==task.Id).Select(a=>a.MapTaskToView()).Single();
        }
               
    }
}