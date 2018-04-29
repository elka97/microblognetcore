using System.Collections.Generic;

namespace PendoBlog.Models {
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User {
        public User() {
            Post = new HashSet<Post>();
        }
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(3)]
        public string Passsword { get; set; }

        [NotMapped]
        public ICollection<Post> Post { get; set; }
    }
}
