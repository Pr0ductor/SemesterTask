namespace Server.Models;

public class MoviesDataVm
{
    public List<Movie> movies { get; set; }
    public bool IsAuthorized { get; set; }
    public string Login { get; set; }
    
}