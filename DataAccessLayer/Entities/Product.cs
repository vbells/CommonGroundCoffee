using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Product_ID { get; set; }
        public string Product_Name { get; set;}
        public string Description { get; set; }  
        public string Product_Type { get; set; }
        public decimal Unit_Price { get; set; }
        public int Quantity_Available { get; set; }

        public string ImageUrl { get; set; } = string.Empty;


    }
}
