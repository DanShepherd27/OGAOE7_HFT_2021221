using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OGAOE7_HFT_2021221.Models
{
    /// <summary>
    /// This class models pizza types on the menu of our Italian restaurant.
    /// </summary>
    [Table("Pizzas")]
    public class Pizza : IComparable<Pizza>
    {
        [Key]
        [Required]
        public string Name { get; set; }

        public int Price { get; set; }

        public bool Promotional { get { return Price >= 2000; } }

        // NAVIGATION PROPERTY

        [NotMapped]
        public virtual ICollection<PromoOrder> Orders { get; set; }

        public Pizza()
        {
            Orders = new HashSet<PromoOrder>();
        }

        public override bool Equals(object obj)
        {
            if (obj is Pizza)
                return (obj as Pizza).Name == this.Name;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int CompareTo(Pizza other)
        {
            return string.Compare(this.Name, other.Name);
        }
    }
}
