using System.ComponentModel.DataAnnotations;

namespace VehicleManagement.Models
{
    public class VehicleViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string VehicleModel { get; set; }

        [Range (1,1000,ErrorMessage = "Should be greated than or equal to 1")]
        public int Seat { get; set; }

        [Required]
        public string Color { get; set; }
    }
}
