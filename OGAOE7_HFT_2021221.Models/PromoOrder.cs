using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [Required]
        public DateTime TimeOfOrder { get; set; }

        //Unfortunately, this getter doesn't work on the Model level because lazy loading prevents getting Drink and Pizza objects, triggers a NullReferenceException. It will be implemented through logic.
        //[NotMapped]
        //[JsonIgnore]
        //public int TotalPrice { get { return (this.Pizza.Price + this.Drink.Price) - (this.Pizza.Price + this.Drink.Price) * DiscountPercentage / 100; } }

        [Required]
        public int DiscountPercentage { get; set; }

        //FOREIGN KEY

        [Required]
        [ForeignKey(nameof(Pizza))]
        public int PizzaId { get; set; }

        [Required]
        [ForeignKey(nameof(Drink))]
        public int DrinkId { get; set; }

        // MAIN DATA
        //Unfortunately, this getter doesn't work on the Model level because lazy loading prevents getting Drink and Pizza objects, triggers a NullReferenceException. It will be implemented through logic.
        //[NotMapped]
        //[JsonIgnore]
        //public string MainData => $"[{Id}] : {PizzaName} ({Pizza.Price} HUF) : {DrinkName} ({Drink.Price} HUF) : Discount - {DiscountPercentage}% : Total - {TotalPrice} : Time of order - {TimeOfOrder.ToString("G", CultureInfo.CurrentCulture)}";

        // NAVIGATION PROPERTY
        [NotMapped]
        [JsonIgnore]
        public virtual Pizza Pizza { get; set; }

        [NotMapped]
        [JsonIgnore]
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
