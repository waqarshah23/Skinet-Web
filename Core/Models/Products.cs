using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Products: BaseEntity
    {
        //[Key,Required]
        //public new int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }  = string.Empty;
        [Required, StringLength(50)]
        public string Description { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Required]
        public string PictureUrl { get; set; } = string.Empty;
        public virtual ProductType? ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public virtual ProductBrand? ProductBrand { get; set; }
        public int ProductBrandId { get; set; }

    }
}
