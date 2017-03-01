using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer.Context
{
public class Task
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get;set;}

    public DateTime Date { get; set;}

    public string Text {get; set;}

    public bool Done { get; set;}

    public bool Deleted { get; set;}

    public User User { get; set;}
}
}