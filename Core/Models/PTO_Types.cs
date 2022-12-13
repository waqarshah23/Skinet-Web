using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("PTO_Types")]
    public class PTO_Types
    {
        [Key, Required]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false), StringLength(15)]
        public string Short_Name { get; set; } = "";
        [Required(AllowEmptyStrings = false), StringLength(50)]
        public string Description { get; set; } = "";
        [Required(AllowEmptyStrings = false), StringLength(2)]
        public string Abbreviation { get; set; } = "";
        public int Color { get; set; }

        public bool Is_Request {get; set;}
    }
}
