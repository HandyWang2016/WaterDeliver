using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CustomerAccessory
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int SendBuckets { get; set; }
        public int ReceiveEmptyBuckets { get; set; }
        public int WaterDispenser { get; set; }
        public int WaterHolder { get; set; }
        public int PushPump { get; set; }
    }
}
