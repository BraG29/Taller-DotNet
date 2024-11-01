using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quality_Management.Model;

public class Office
{
    [Required]
    [Key]
    public string OfficeId { get; set; }
    public int PositionsAmount { get; set; }
    [NotMapped]
    public int ClientsInQueue { get; set; } = 0;
    [NotMapped] 
    public int FreePositions { get; set; } = 0;
    
    // public Office(string officeId, int positionsAmount)
    // {
    //     OfficeId = officeId;
    //     PositionsAmount = positionsAmount;
    // }
}