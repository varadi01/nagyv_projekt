using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic.CompilerServices;

namespace nagyv_projekt.Entities;

public class Lending
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid id {get; set;}
    
    [Required]
    public Guid ReaderId {get; set;}
    
    [Required]
    public Guid BookId {get; set;}
    
    [Required] //TODO
    public DateOnly LendingDate {get; set;}
    
    [Required] //TODO
    public DateOnly ReturnDate {get; set;}
}