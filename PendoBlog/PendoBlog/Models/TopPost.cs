namespace PendoBlog.Models {
    using System;
    using System.ComponentModel.DataAnnotations;

    //  SELECT p.PostId, p.Title, p.[CreationDate], p.VoteUpNumber from viewTopPosts p
    public class TopPost {
        [Key]
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public int VoteUpNumber { get; set; }

    }
}