using System;

namespace BoggleWebservice.Models
{
    public class PlayerScore
    {
        public Guid BoggleBoxId { get; set; }
        public string PlayerName { get; set; }
        public int Score { get; set; }
    }
}