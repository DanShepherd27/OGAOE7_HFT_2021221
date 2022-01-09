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
    /// This class models drinks that can be ordered at our Italian restaurant.
    /// </summary>
    [Table("Drinks")]
    public class Drink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public bool Promotional { get; set; }

        // NAVIGATION PROPERTY

        [NotMapped]
        public virtual ICollection<PromoOrder> Orders { get; set; }

        public Drink()
        {
            Orders = new HashSet<PromoOrder>();
        }
    }
}
