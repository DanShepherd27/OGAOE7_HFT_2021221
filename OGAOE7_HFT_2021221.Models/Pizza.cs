using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OGAOE7_HFT_2021221.Models
{
    /// <summary>
    /// This class models pizza types on the menu of our Italian restaurant.
    /// </summary>
    [Table("Pizzas")]
    public class Pizza : IComparable<Pizza>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        public bool Promotional { get { return Price >= 2000; } }

        // MAIN DATA
        //Unfortunately, this getter doesn't work on the Model level because lazy loading prevents getting Drink and Pizza objects, triggers a NullReferenceException. It will be implemented through logic.
        //[NotMapped]
        //[JsonIgnore]
        //public string MainData => $"[{Name}] : Price - {Price} HUF : Promotional - {Promotional} : Number of orders - {Orders.Count}";

        // NAVIGATION PROPERTY

        [NotMapped]
        [JsonIgnore]
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
