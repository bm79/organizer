using Microsoft.EntityFrameworkCore;

namespace Organizer.Context {
public class OrganizerContext:DbContext {

    public OrganizerContext(){}

    public OrganizerContext(DbContextOptions<OrganizerContext> options):base(options){}

    public virtual DbSet<User> User { get; set; }
    public virtual DbSet<Task> Tasks { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Filename=./organizer.db");
        }
    }
}
}