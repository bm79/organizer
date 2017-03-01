using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Organizer.Context
{
public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id {get; set;}
    public string Name { get; set;}
    public string Surname { get; set;}

    public bool Deleted { get; set;}
    public List<Task> Tasks {get;set;}
}
}