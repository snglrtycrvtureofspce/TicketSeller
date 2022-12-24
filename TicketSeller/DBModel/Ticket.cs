using System;
using System.ComponentModel.DataAnnotations;

namespace TicketSeller.DBModel;

public class Ticket
{
    [Key] 
    public int Id { get; set; }
    
    public virtual User SoldUser { get; set; }
    
    public bool IsSold { get; set; }

    public virtual User BockedUser { get; set; }
    
    public bool IsBocked { get; set; }
    
    public string TicketName { get; set; }
    
    public DateTime DateOfConcert { get; set; }

    public string ConcertPlace { get; set; }
    
    public virtual Tag Tag { get; set; }
}