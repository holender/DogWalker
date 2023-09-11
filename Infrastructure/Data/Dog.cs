using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data
{
    public class Dog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string? Name { get; set; }

        [Required]
        [StringLength(200)]
        public string? Breed { get; set; }

        [Required]
        [StringLength(200)]
        public string? UserName { get; set; }

        [StringLength(256)] public ICollection<Owner> Owners { get; set; } 
            = new List<Owner>();

        public ICollection<SessionDog> SessionDogs { get; set; } =
            new List<SessionDog>();
    }
}