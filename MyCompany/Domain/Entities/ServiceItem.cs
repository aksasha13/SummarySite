using System.ComponentModel.DataAnnotations;

namespace MyCompany.Domain.Entities
{
    public class ServiceItem:EntityBase
    {
        [Required(ErrorMessage = "Fill the name of the project")]
        [Display(Name = "Project name")]
        public override string Title { get; set; }

        [Display(Name = "Short description of the project")]
        public override string Subtitle { get; set; }

        [Display(Name = "Full description of the project")]
        public override string Text { get; set; }
    }
}
