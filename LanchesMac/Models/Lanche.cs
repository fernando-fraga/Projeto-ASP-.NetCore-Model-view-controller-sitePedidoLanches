using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanchesMac.Models
{
    [Table("Lanches")]
    public class Lanche
    {
        [Key]
        public int LancheId { get; set; }


        [Required(ErrorMessage ="O nome do lanche deve ser informado")]
        [Display(Name = "Nome do lanche")]
        [StringLength(80, MinimumLength = 10, ErrorMessage ="O {0} deve ter no mínimo {1}e no máximo {2} ")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição do lanche deve ser informado")]
        [Display(Name = "Descrição do lanche")]
        [StringLength(200, MinimumLength = 20, ErrorMessage = "O {0} deve ter no mínimo {1} e no máximo {2} ")]
        public string DescricaoCurta{ get; set;}


        [Required(ErrorMessage = "A descrição detalhada do lanche deve ser informado")]
        [Display(Name = "Descrição detalhada do lanche")]
        [StringLength(200, MinimumLength = 20, ErrorMessage = "O {0} deve ter no mínimo {1} e no máximo {2} ")]
        public string DescricaoDetalhada { get; set; }


        [Required(ErrorMessage = "Informe o preço do lanche")]
        [Display(Name = "Preço")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(1,999.99,ErrorMessage = "O preço deve estar entre 1 e 999,99")]
        public decimal Preco { get; set; }


        [Display(Name = "Caminho imagem nornal")]
        [StringLength(200, ErrorMessage = "O {0} deve ter no máximo {1} caractere ")]
        public string ImagemUrl { get; set; }

        [Display(Name = "Caminho imagem miniatura")]
        [StringLength(200, ErrorMessage = "O {0} deve ter no máximo {1} caractere ")] 
        public string ImagemThumbnailUrl { get; set;}

        [Display(Name = "Preferido?")]
        public bool IsLanchePreferido { get; set; }

        [Display(Name = "Estoque")]
        public bool EmEstoque { get; set; }

        [Display(Name ="Categorias")]
        public int CategoriaId { get; set; }

        public virtual Categoria Categoria { get; set; }


        }
  }

