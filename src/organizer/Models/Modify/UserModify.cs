using System.ComponentModel.DataAnnotations;

namespace Organizer.Models.Modify
{
    public class UserModify {
       
        [Required]
        [MaxLength(30)]
        public string Name { get;set;}

        [MaxLength(50)]
        [Required]
        public string Surname {get; set;}

       

       
    }
}