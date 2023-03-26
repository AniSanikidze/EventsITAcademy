using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventsITAcademy.MVC.Models
{
    public class AssignRoleViewModel
    {
        public string UserId { get; set; }

        public string SelectedRole { get; set; }
        public SelectList Roles { get; set; }
    }
}
