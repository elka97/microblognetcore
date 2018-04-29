using System;

namespace PendoBlog.Models {
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Post {
        [Key]
        public int PostId { get; set; }

        [Required]
        [ForeignKey("FK_Post_User_UserId")]
        public int UserId { get; set; }

        [Required]
        [MinLength(5)]
        public string Content { get; set; }

        [Required]
        [MinLength(5)]
        public string Title { get; set; }

        public DateTime CreationDate { get; set; }

        [NotMapped]
        public User User { get; set; }
    }
}