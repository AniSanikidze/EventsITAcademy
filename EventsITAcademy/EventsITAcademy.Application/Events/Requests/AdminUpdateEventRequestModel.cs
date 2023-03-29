using System.ComponentModel.DataAnnotations;

namespace EventsITAcademy.Application.Events.Requests
{
    public class AdminUpdateEventRequestModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Title should not be empty and should consists maximum 100 symbols")]
        public string Title { get; set; }
        [Required]
        [StringLength(350, ErrorMessage = "Description should not be empty and should consists maximum 350 symbols")]
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime FinishDate { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int NumberOfTickets { get; set; }
        [Required]
        public int ModificationPeriod { get; set; }
        [Required]
        public int ReservationPeriod { get; set; }
    }
}
