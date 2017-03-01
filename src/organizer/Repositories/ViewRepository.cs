using System.Collections.Generic;
using Organizer.Context;
using System.Linq;
using Organizer.Extensions;
using Microsoft.Extensions.Logging;

namespace Organizer.Repositories
{
    public class ViewRepository:IViewRepository {
        private OrganizerContext _organizerContext; 

        private ILogger<IViewRepository> _logger;
        public ViewRepository(OrganizerContext organizerContext , ILogger<ViewRepository> logger)
        {
            _organizerContext=organizerContext;
            _logger = logger;
        }

        public IEnumerable<Models.View.TaskView> GetAllTasks()
        {
            return _organizerContext.Tasks.Select(a=>a.MapTaskToView()).AsEnumerable();
        }

        public IEnumerable<Models.View.UserView> GetAllUsers()
        {
           return _organizerContext.User.Select(a=>a.MapUserToView()).AsEnumerable();
        }

        public IEnumerable<Models.View.TaskView> GetAllUserTask(int idUser)
        {
            User user = _organizerContext.User.Where(a=>a.Id==idUser).Single();
            return _organizerContext.Tasks.Where(a=>a.User==user).Select(a=>a.MapTaskToView()).AsEnumerable();
        }

        public Models.View.TaskView GetTask(int id)
        {
            return _organizerContext.Tasks.Where(a=>a.Id==id).Select(a=>a.MapTaskToView()).Single();
        }

        public Models.View.UserView GetUser(int id)
        {
            return _organizerContext.User.Where(a=>a.Id==id).Select(a=>a.MapUserToView()).Single();
        }

        
    }
}