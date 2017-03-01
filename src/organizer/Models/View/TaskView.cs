using Organizer.Models.Modify;

namespace Organizer.Models.View
{
    public class TaskView:TaskModify {
    public int Id {get;set;}
     public bool Done { get; set;}

    public bool Deleted { get; set;}

 }
}