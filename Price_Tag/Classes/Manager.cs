using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Price_Tag.Classes
{
    internal class Manager
    {
        public static Frame MainFrame { get; set; }

        public class Settings
        {
            public string CompanyName { get; set; }
            public string FileStreamString { get; set; }
        }
    }
}
