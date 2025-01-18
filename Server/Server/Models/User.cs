namespace Server.Models;

public class User
{
    //create fields id,login,password
    public int Id { get; set; }
    public string Login { get; set; }
    
    public string Password { get; set; }
    public string Email { get; set; }
}