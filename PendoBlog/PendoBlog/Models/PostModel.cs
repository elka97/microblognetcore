namespace PendoBlog.Models {
    using System.ComponentModel.DataAnnotations;

    public class PostModel {
        [Required]
        [MinLength(5)]
        public string Text { get; set; }
    }
}
