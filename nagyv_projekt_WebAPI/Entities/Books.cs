using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nagyv_projekt.Entities;

public class Books
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid AccessionNumber { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Author { get; set; }
    
    [Required]
    public string Publisher { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)] //TODO 
    public int Year { get; set; }
}