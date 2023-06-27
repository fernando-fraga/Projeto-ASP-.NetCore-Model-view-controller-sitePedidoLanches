using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanchesMac.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        [Key] //por usar o nome da variavel com o nome da Class seguido de Id, o MVC já associa que será uma Key, mas nesse caso colocamos apenas para melhor visualizacao
        public int CategoriaId { get; set; }

        [StringLength(100,ErrorMessage ="O tamanho máximo é 100 caracteres")]
        [Required(ErrorMessage ="Informe o nome da categoria")]
        [Display(Name = "Nome")]
        public string CategoriaNome { get; set; }

        [StringLength(200, ErrorMessage = "O tamanho máximo é 200 caracteres")]
        [Required(ErrorMessage = "Informe a descricao da categoria")]
        [Display(Name = "Descricao")]
        public string Descricao { get; set;}

        public List<Lanche> Lanches { get; set;}

    }
}
