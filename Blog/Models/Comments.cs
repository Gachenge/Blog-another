using Blog.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Comment
{
    [Key]
    public int Id { get; set; }

    [MaxLength(200)]
    [Required]
    public string Body { get; set; }

    // Foreign key property
    public int PostId { get; set; }
    // Foreign key property
    public string AuthorId { get; set; }

    // Navigation property
    [ForeignKey("AuthorId")]
    public virtual ApplicationUser Author { get; set; }

    // Navigation property
    [ForeignKey("PostId")]
    public virtual Post Post { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
