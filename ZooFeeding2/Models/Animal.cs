using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZooFeeding2.Models
{
    public class Animal
    {
        public int AnimalId { get; set; }
        public string AnimalName { get; set; }

        public virtual ICollection<Relationship> Relationships { get; set; }
    }
}