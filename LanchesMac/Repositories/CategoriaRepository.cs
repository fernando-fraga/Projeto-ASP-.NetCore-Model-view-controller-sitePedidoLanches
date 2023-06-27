using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;

namespace LanchesMac.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        //Variavel Privada de apenas leitura do tipo "AppDbContext" chamada _context 
        //depois cliquei em cima de context para criar o Construtor abaixo
        private readonly AppDbContext _context;


        //esse é o construtor
        public CategoriaRepository(AppDbContext context) {
            _context = context;
        }

        public IEnumerable<Categoria> Categorias => _context.Categorias;


    }
}
