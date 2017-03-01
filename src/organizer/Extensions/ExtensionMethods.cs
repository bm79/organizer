namespace Organizer.Extensions {
    public static class ExtensionMethods
    {
        public static Organizer.Models.View.UserView MapUserToView(this Organizer.Context.User user)
        {
            return new Organizer.Models.View.UserView {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Deleted = user.Deleted
            };
        }

        public static void SaveUserModify(this Organizer.Context.User user , Organizer.Models.Modify.UserModify userModify)
        {
            user.Name = userModify.Name;
            user.Surname = userModify.Surname;
        }


        public static void SaveTaskModify(this Organizer.Context.Task task , Organizer.Models.Modify.TaskModify taskModify)
        {
            task.Date = taskModify.Date;
            task.Text = taskModify.Text;
        }

        public static Organizer.Models.View.TaskView MapTaskToView(this Organizer.Context.Task task)
        {
            return new Organizer.Models.View.TaskView {
                Id = task.Id,
                Date = task.Date,
                Text = task.Text,
                Done = task.Done,
                Deleted = task.Deleted
            };
        }
    }
}