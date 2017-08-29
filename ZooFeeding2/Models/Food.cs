using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZooFeeding2.Models
{
    public class Food
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }

        public virtual ICollection<Relationship> Relationships { get; set; }
    }
}