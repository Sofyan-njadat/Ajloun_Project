using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ajloun_Project.Models;

public class CraftOrder
{
    [Key]
    public int OrderId { get; set; }

    [Required]
    public int CraftId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int Quantity { get; set; }

    public DateTime? OrderDate { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Pending";

    // العلاقات
    [ForeignKey("CraftId")]
    public virtual Handicraft Craft { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }
}
