namespace Server.Models;

public class Movie
{
    public int id { get; set; }
    public string title { get; set; }
    public int release_year { get; set; }
    public int duration { get; set; }
    public string description { get; set; }
    public string poster_url { get; set; }
    public string director { get; set; }
    public string actors { get; set; }
    public string URL_video  { get; set; }
    public string genre { get; set; }
    public string country { get; set; }
}