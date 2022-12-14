using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class ProductBrand: BaseEntity
    {
        [Required, StringLength(100)]
        public string name { get; set; } = string.Empty
    }
}