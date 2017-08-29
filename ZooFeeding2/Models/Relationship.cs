using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZooFeeding2.Models
{
    public class Relationship
    {
        public int RelationshipId { get; set; }
        public int AnimalId { get; set; }
        public int FoodId { get; set; }

        public virtual Animal Animal { get; set; }
        public virtual Food Food { get; set; }
    }
}