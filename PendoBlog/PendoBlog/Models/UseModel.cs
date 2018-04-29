namespace PendoBlog.Models {
    using System.ComponentModel.DataAnnotations;

    public class UseModel {
       
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        public string Passsword { get; set; }

    }
}
