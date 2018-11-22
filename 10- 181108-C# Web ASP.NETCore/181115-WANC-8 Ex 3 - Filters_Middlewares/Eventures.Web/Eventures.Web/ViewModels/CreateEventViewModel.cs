using System;
using System.ComponentModel.DataAnnotations;

namespace Eventures.Web.ViewModels
{
    public class CreateEventViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Place")]
        [RegularExpression(@"^[A-Z][a-zA-Z]+,[ ]?[A-Z][a-zA-Z].*$", ErrorMessage = "Invalid City and Country Format")]
        public string Place { get; set; }

        [Required]
        [Display(Name = "Start")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = @"{dd-MMM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }

        [Required]
        [Display(Name = "End")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = @"{dd-MMM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime End { get; set; }

        [Required]
        [Display(Name = "TotalTickets")]
        [Range(0, int.MaxValue)]
        public int TotalTickets { get; set; }

        [Required]
        [Display(Name = "PricePerTicket")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal TicketPrice { get; set; }
    }
}
