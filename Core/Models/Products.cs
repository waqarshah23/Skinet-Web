using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Products
    {
        [Key,Required]
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }  = string.Empty;
    }
}
