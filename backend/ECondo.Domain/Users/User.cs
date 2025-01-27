namespace ECondo.Domain.Users;
public class User
{
    public Guid Id { get; set; }
    public string SecurityStamp { get; set; } = Guid.NewGuid().ToString();

    public string Username { get; set; } = null!;
    public string NormalizedUsername { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string NormalizedEmail { get; set; } = null!;
    public bool EmailConfirmed { get; set; }

    public string PasswordHash { get; set; } = null!;

    public Guid ConcurrencyStamp { get; set; } = Guid.NewGuid();

    public DateTimeOffset? LockoutEnd { get; set; }
    public int AccessFailedCount { get; set; } = 0;
    public bool LockoutEnabled { get; set; }
}


/* (Jordan Dimitrov): AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
   *                               *     _
          /\     *            ___.       /  `)
      *  //\\    /\          ///\\      / /
        ///\\\  //\\/\      ////\\\    / /     /\
       ////\\\\///\\/\\.-~~-.///\\\\  / /     //\\
      /////\\\\///\\/         `\\\\\\/ /     ///\\
     //////\\\\// /            `\\\\/ /     ////\\
    ///////\\\\\//               `~` /\    /////\\
   ////////\\\\\/      ,_____,   ,-~ \\\__//////\\\
   ////////\\\\/  /~|  |/////|  |\\\\\\\\@//jro/\\
   //<           / /|__|/////|__|///////~|~/////\\
   
   ~~~     ~~   ` ~   ..   ~  ~    .     ~` `   '.
   ~ _  -  -~.    .'   .`  ~ .,    '.    ~~ .  '.
 */
