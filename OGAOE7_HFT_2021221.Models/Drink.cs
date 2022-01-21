using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OGAOE7_HFT_2021221.Models
{
    /// <summary>
    /// This class models drinks that can be ordered at our Italian restaurant.
    /// </summary>
    [Table("Drinks")]
    public class Drink : IComparable<Drink>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public bool Promotional { get; set; }

        // MAIN DATA
        //Unfortunately, this getter doesn't work on the Model level because lazy loading prevents getting Drink and Pizza objects, triggers a NullReferenceException. It will be implemented through logic.
        //[NotMapped]
        //[JsonIgnore]
        //public string MainData => $"[{Name}] : Price - {Price} HUF : Promotional - {Promotional} : Number of orders - {Orders.Count}";

        // NAVIGATION PROPERTY

        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<PromoOrder> Orders { get; set; }

        public Drink()
        {
            Orders = new HashSet<PromoOrder>();
        }

        public override bool Equals(object obj)
        {
            if (obj is Drink)
                return (obj as Drink).Name == this.Name;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int CompareTo(Drink other)
        {
            return string.Compare(this.Name, other.Name);              
        }
    }
}
