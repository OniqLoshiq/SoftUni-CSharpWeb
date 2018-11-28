using System.ComponentModel.DataAnnotations;

namespace Eventures.Web.ViewModels
{
    public class CreateOrderBindingModel
    {
        public string EventId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Tickets")]
        public int Tickets { get; set; }
    }
}
