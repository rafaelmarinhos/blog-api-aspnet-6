using blog_api_aspnet_6.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace blog_api_aspnet_6.Data.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Title);
            builder.Property(x => x.Summary);
            builder.Property(x => x.Body);
            builder.Property(x => x.Slug);

            builder.Property(x => x.LastUpdateDate)
                .IsRequired()
                .HasColumnName("LastUpdateDate")
                .HasColumnType("SMALLDATETIME")
                .HasMaxLength(60)
                .HasDefaultValue(DateTime.Now.ToUniversalTime());

            builder.HasIndex(x => x.Slug, "IX_Post_Slug")
                .IsUnique();

            builder.HasOne(x => x.Author)
                .WithMany(x => x.Posts);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Posts);

            builder
                .HasMany(x => x.Tags)
                .WithMany(x => x.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostTag",
                    post => post
                        .HasOne<Tag>()
                        .WithMany()
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_PostRole_PostId")
                        .OnDelete(DeleteBehavior.Cascade),
                    tag => tag
                        .HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_PostTag_TagId")
                        .OnDelete(DeleteBehavior.Cascade));
        }
    }
}
