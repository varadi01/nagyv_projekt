using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic.CompilerServices;

namespace nagyv_projekt.Entities;

[Table("lending")]
public class Lending
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id {get; set;}
    
    [Required]
    public Guid ReaderId {get; set;}
    
    [Required]
    public Guid BookId {get; set;}
    
    [Required] 
    public DateOnly LendingDate {get; set;}
    
    [Required] 
    public DateOnly ReturnDate {get; set;}
}