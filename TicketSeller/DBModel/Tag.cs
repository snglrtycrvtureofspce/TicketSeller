using System.ComponentModel.DataAnnotations;

namespace TicketSeller.DBModel;

public class Tag
{
    [Key]
    public int Id { get; set; }
    
    public string TagName { get; set; }

    public override string ToString() => TagName;
    
}