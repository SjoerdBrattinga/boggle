using System;
using System.Collections.Generic;

namespace BoggleWebservice.Models
{
    public class BoggleBox
    {
        public Guid BoggleBoxId { get; set; } 
        public List<List<Die>> Dice { get; set; }
    }
}