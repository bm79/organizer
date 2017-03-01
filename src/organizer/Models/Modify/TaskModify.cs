using System;
using System.ComponentModel.DataAnnotations;

namespace Organizer.Models.Modify { 
 public class TaskModify {


    [Required]
    public DateTime Date { get; set;}

    [Required]
    [MaxLength(255)]
    public string Text {get; set;}

   

 }
}