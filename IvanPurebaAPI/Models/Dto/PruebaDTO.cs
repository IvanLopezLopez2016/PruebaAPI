using System.ComponentModel.DataAnnotations;

namespace IvanPurebaAPI.Models.Dto
{
    public class PruebaDTO
    {
        
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? Nombre { get; set; }   
        public double decimales { get; set; }

        public string? ImgUrl { get; set; }
    }
}
