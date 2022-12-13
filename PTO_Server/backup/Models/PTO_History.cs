using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTO_Server.Models
{
    [Table("PTO_History")]
    public class PTO_History
    {
        [Key, Required]
        public int Id { get; set; }
        [Display(Name = "Users")]
        public virtual int Employee_Id { get; set; }
        [ForeignKey("Employee_Id")]
        public virtual Users? User { get; set; }
        [Display(Name = "PTO_Types")]
        public virtual int PTO_Id { get; set; }
        [ForeignKey("PTO_Id")]
        public virtual PTO_Types? PTO_Type { get; set; }
        public DateTime Start_date { get; set; }
        
        public DateTime End_Date { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "";

        public int Approved_By { get; set; }
        [Required(AllowEmptyStrings = false),]
        public string Notes { get; set; } = "";
    }
}
