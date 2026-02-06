using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Preferences
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pref_ID { get; set; }

        [ForeignKey(nameof(Customer))]
        public int Customer_ID { get; set; }
       
        [ForeignKey(nameof(Product))]
        public int Product_ID { get; set; }
        public string Notes { get; set; }
    }
}
