using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Job_Scheduler_webAPI.Models
{
    public class Countries
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CountryID { get; set; }

        public string CountryName { get; set; }

        public string TwoCharCountryCode { get; set; }

        public string ThreeCharCountryCode { get; set; }
    }
}
