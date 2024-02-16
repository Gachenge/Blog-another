using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Blog.Models;
using System.Collections.Generic;
using System;


public class Post
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    [Required]
    public string Title { get; set; }

    [MaxLength(1000)]
    [Required]
    public string Body { get; set; }

    // Foreign key property
    public string AuthorId { get; set; }

    // Navigation property
    [ForeignKey("AuthorId")]
    public virtual ApplicationUser Author { get; set; }

    // Navigation property
    public virtual ICollection<Comment> Comments { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }                                                                                                                                                                    
}
