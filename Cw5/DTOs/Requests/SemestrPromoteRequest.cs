using System.ComponentModel.DataAnnotations;

namespace Cw5.DTOs.Requests
{
    public class SemestrPromoteRequest
    {
        [Required]
        public string Studies { get; set; }
        [Required]
        public int Semester { get; set; }
    }
}
