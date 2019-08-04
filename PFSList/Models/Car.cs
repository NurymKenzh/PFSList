using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PFSList.Models
{
    public class Car
    {
        public int Id { get; set; }
        public int ProducerId { get; set; }
        public Producer Producer { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
    }

    public class Producer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}