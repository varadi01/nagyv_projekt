using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nagyv_projekt.Entities;

[Table("books")]
public class Books
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BookID { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Author { get; set; }
    
    [Required]
    public string Publisher { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Year { get; set; }
}