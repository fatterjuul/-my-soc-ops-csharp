namespace SocOps.Models;

public class Team
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "Team";
    public List<Player> Players { get; set; } = new();
}
