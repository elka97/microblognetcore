namespace PendoBlog.Models {
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PostVote {
        [Key]
        [ForeignKey("FK_PostVote_Post_PostId")]
        public int PostId { get; set; }
        [Key]
        [ForeignKey("FK_PostVote_User_UserId")]
        public int UserId { get; set; }

        [Range(0, 1)]
        public bool VoteUp { get; set; }

        [Range(0, 1)]
        public bool VoteDown { get; set; }
    }
}