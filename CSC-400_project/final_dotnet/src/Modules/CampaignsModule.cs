using System.ComponentModel.DataAnnotations;

public class Campaign
{
    [Key]

    public string? Id { get; set; }
    public string? title { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? city { get; set; }
    public string? deadline { get; set; }
    public string? Category { get; set; }
    public string? fundgoal { get; set; }
    public List<string>? sociallinks { get; set; }
    public List<string>? images { get; set; }
    public string? ownerId { get; set; }
    public Users? owner { get; set; }
    public string? mainImage { get; set; }
}