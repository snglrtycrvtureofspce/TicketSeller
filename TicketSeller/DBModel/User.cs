using System.ComponentModel.DataAnnotations;

namespace TicketSeller.DBModel;

public class User
{  
    [Key] 
    [MaxLength(450)]
    public string Id { get; set; }

    public byte[] HashPassword { get; set; }

    public string UserName { get; set; }
    
    public UsersRole Role { get; set; }
}