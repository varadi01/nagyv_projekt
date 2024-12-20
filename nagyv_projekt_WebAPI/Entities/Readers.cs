using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nagyv_projekt.Entities;

[Table("readers")]
public class Readers
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ReaderId { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    [Required]
    [Range(typeof(DateOnly), "1900-01-01", "3000-01-01")] 
    public DateOnly BirthDate { get; set; }
    
    
    public virtual ICollection<Lending> Lendings { get; set; }
}