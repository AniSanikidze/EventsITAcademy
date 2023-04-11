using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EventsITAcademy.Application.Events.Requests
{
    public class EventRequestModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Title should not be empty and should consists maximum 100 symbols")]
        public string Title { get; set; }
        [Required]
        [StringLength(350,ErrorMessage = "Description should not be empty and should consists maximum 350 symbols")]
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime FinishDate { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int NumberOfTickets { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
