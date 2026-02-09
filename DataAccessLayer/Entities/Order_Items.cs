using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Order_Items
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Order_Item_ID { get; set; }

        public int Order_ID { get; set; }
        [Foreign key (nameof (Order_Item_ID))]
        public Orders Order { get; set; }

        public int Product_ID { get; set; }
        [Foreign key (nameof(Product_ID))]
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal Price_at_Purchase { get; set; }

    }
}
