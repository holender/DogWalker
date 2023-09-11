using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data
{
    public class Walker
       {
           public int Id { get; set; }

           [Required]
           [StringLength(200)]
           public string? Name { get; set; }

           [StringLength(4000)]
           public string? Bio { get; set; }

           [StringLength(1000)]
           public string? WebSite { get; set; }

           public ICollection<SessionWalker> SessionWalkers { get; set; } = 
               new List<SessionWalker>();
       }
   }