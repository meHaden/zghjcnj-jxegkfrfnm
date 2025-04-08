// Models/CapsuleWardrobe.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OutfitPlaner_Applcation.Models
{
    public partial class CapsuleWardrobe
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? Style { get; set; }

        [StringLength(50)]
        public string? Season { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int IdUser { get; set; }

        //public virtual ICollection<Clothing> Items { get; set; } = new List<Clothing>();

        public virtual ICollection<ClothingCapsule> ClothingCapsules { get; set; } = new List<ClothingCapsule>();
        public virtual User IdUserNavigation { get; set; } = null!;
    }
}