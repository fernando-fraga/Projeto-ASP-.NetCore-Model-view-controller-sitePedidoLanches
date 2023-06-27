using Microsoft.EntityFrameworkCore.Migrations; 

#nullable disable

namespace LanchesMac.Migrations
{
    /// <inheritdoc />
    public partial class PopularCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Categorias(CategoriaNome, Descricao)" + 
                                   "VALUES('Normal','Lanche feitos com ingredientes ordinarios')");

            migrationBuilder.Sql("INSERT INTO Categorias(CategoriaNome, Descricao)" +
                                   "VALUES('Natural','Lanche feitos com ingredientes naturais e organicos para deixar voce mais saudável')");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categorias");
        }
    }
}
