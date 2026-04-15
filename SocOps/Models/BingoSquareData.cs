namespace SocOps.Models;

public class BingoSquareData
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsMarked { get; set; }
    public bool IsFreeSpace { get; set; }
    public string? MarkedByPlayerId { get; set; } // Track which player marked this square
}
