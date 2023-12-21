using ShoeStore.Models;
using System.ComponentModel.DataAnnotations;

namespace ShoeStore.ModelViews
{
    public class CheckOutVM
    {

        public int CustomerId { get; set; }
		[Required(ErrorMessage = "Please enter Full Name")]
        public string? FullName { get; set; }
        public string? Email { get; set; }
        [Required(ErrorMessage = "Please enter Phone number")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Shipping Address is required")]
        public string? Address { get; set; }
        public string? Size {  get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }

        public string? Note { get; set; }
    }
}
