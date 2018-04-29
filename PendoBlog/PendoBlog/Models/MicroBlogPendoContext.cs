using Microsoft.EntityFrameworkCore;

namespace PendoBlog.Models {

    public class MicroBlogPendoContext : DbContext {

        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<PostVote> PostVote { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<TopPost> TopPost { get; set; }

        public MicroBlogPendoContext(DbContextOptions<MicroBlogPendoContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Post>(entity => {
                
                entity.HasKey(c => c.PostId);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");
                
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.UserId);
                //  .OnDelete(DeleteBehavior.ClientSetNull);

            });

            modelBuilder.Entity<PostVote>(entity => {
                entity.HasKey(e => new { e.PostId, e.UserId });
            });

            modelBuilder.Entity<User>(entity => {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Passsword)
                    .IsRequired()
                    .HasMaxLength(30);
            });



        }
}
}
