namespace JoyCase.Data;

public partial class Role
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
