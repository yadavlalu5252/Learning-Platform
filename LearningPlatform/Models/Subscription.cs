using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatform.Models
{
    public class Subscription
    {
        [Key]
        public int sid { get; set; }
        public string stype { get; set; }

    

        public int amount { get; set; }
        public string status { get; set; }

 
    }
}
