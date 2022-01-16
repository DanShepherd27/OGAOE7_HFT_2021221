using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Models
{
    /// <summary>
    /// This class models a promotional order where you can combine your pizza with a drink and you will get a discount.
    /// </summary>
    [Table("Orders")]
    public class PromoOrder : IComparable<PromoOrder>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime TimeOfOrder { get; set; }

        public int TotalPrice { get { return (Pizza.Price + Drink.Price) - (Pizza.Price + Drink.Price) * DiscountPercentage / 100; } }

        public int DiscountPercentage { get; set; }

        //FOREIGN KEY

        [Required]
        [ForeignKey(nameof(Pizza))]
        public string PizzaName { get; set; }

        [Required]
        [ForeignKey(nameof(Drink))]
        public string DrinkName { get; set; }

        // NAVIGATION PROPERTY

        [NotMapped]
        public virtual Pizza Pizza { get; set; }

        [NotMapped]
        public virtual Drink Drink { get; set; }

        public int CompareTo(PromoOrder other)
        {
            if (this.Id < other.Id)
                return -1;
            else if (this.Id == other.Id)
                return 0;
            else
                return 1;
        }

        public override bool Equals(object obj)
        {
            if (obj is PromoOrder)
                return (obj as PromoOrder).Id == this.Id;
            else
                return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
