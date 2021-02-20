using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace consumeHockeyAPI.Models
{
    public class PlayerDetail
    {
        
        public int id { get; set; }

        [Required]
        public Team currentTeam {get; set; }

        [Required]
        public string firstName { get; set; }
        public string lastName { get; set; }

        [Required]
        public int currentAge { get; set; }

        [Required]
        public string nationality { get; set; }

        [Required]
        public Position primaryPosition { get; set; }
    }
}