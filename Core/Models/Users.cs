using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("Users")]
    public class Users
    {
        [Key, Required]
        public int Id { get; set; }
        [StringLength(50), Required(AllowEmptyStrings = false)]
        public string First_Name { get; set; } = "";
        [StringLength(50), Required(AllowEmptyStrings = false)]
        public string Last_Name { get; set; } = "";
        [StringLength(50), Required(AllowEmptyStrings = false)]
        public string Middle_Name { get; set; } = "";
        [StringLength(50)]
        public string Pay_System_id { get; set; } = "";

        public bool Is_Active { get; set; }

        public string Type { get; set; } = "";

        [StringLength(50), Required(AllowEmptyStrings = false)]
        public string Password { get; set; } = "";
        [StringLength(50), Required(AllowEmptyStrings = false)]
        public string Email_Address { get; set; } = "";
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }


    }
}

