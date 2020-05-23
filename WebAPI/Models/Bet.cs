using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Bet
    {
        [Required]
        [Range(1,30, ErrorMessage = "Il valore deve essere compreso tra 1 e 30")]
        public int first { get; set; }
        [Required]
        [Range(1, 30, ErrorMessage = "Il valore deve essere compreso tra 1 e 30")]
        public int second { get; set; }
        [Required]
        [Range(1, 30, ErrorMessage = "Il valore deve essere compreso tra 1 e 30")]
        public int third { get; set; }
        [Required]
        [Range(1, 30, ErrorMessage = "Il valore deve essere compreso tra 1 e 30")]
        public int fourth { get; set; }
        [Required]
        [Range(1, 30, ErrorMessage = "Il valore deve essere compreso tra 1 e 30")]
        public int fifth { get; set; }

        public string username { get; set; }

        public List<int> betNumbers { get; set; } = new List<int>();
    }

    public class Winning
    {
        public int betsCount { get; set; }

        public int numbersHit { get; set; }
    }
}
