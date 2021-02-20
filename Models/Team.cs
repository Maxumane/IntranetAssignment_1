using System.ComponentModel.DataAnnotations;

namespace consumeHockeyAPI.Models
{
    public class Team
    {
        public int id { get; set; }

        [Required]
        public string name { get; set; }
    }
}