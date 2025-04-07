using System;
using System.Collections.Generic;

namespace OutfitPlaner_Applcation.Models;

public partial class Clothing
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Color { get; set; } = null!;

    public string Style { get; set; } = null!;

    public string Material { get; set; } = null!;

    public string Season { get; set; } = null!;

    public int? Condition { get; set; }

    public DateTime? AddedAt { get; set; }

    public virtual ICollection<ClothingCapsule> ClothingCapsules { get; set; } = new List<ClothingCapsule>();

    public virtual ICollection<ClothingLook> ClothingLooks { get; set; } = new List<ClothingLook>();

    public virtual User IdUserNavigation { get; set; } = null!;
    public string UserId { get; internal set; }
}
