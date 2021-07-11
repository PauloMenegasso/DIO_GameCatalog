using System.ComponentModel.DataAnnotations;

namespace Projeto_DIO___Catálogo_de_Jogos_com_DotNet.imputModel
{
    public class GameImputModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Game name should have between 3 and 100 characters")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Producer name should have between 3 and 100 characters")]
        public string Producer { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Price should be between 1 and 1000 reais")]
        public double Price { get; set; }
    }
}
