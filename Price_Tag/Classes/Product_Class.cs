using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Price_Tag.Classes
{
    public class Product_Class
    {
        public class Product
        {
            public int ID { get; set; }
            public string ProductName { get; set; }
            public string ProductCost { get; set; }
            public string ProductType { get; set; }
            public string ProductBarcode { get; set; }
        }
        public static List<Product> ProductsList { get; set; }
    }
}
