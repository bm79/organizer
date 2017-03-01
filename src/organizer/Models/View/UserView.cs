using Organizer.Models.Modify;

namespace Organizer.Models.View
{
    public class UserView:UserModify {
        public int Id { get; set;}

         public bool Deleted { get; set;}
       
    }
}