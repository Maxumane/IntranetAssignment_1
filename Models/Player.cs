using System.ComponentModel.DataAnnotations;

namespace consumeHockeyAPI.Models
{
    public class Player
    {
        public string playerId { get; set; }

        [Required]
        public string currentTeam {get; set; }

        [Required]
        public string fullName { get; set; }

        [Required]
        public string age { get; set; }

        [Required]
        public string nationality { get; set; }

        [Required]
        public string primaryPosition { get; set; }
    }
}