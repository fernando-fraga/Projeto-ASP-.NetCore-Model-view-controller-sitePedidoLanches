using LanchesMac.Context;
using Microsoft.EntityFrameworkCore;

namespace LanchesMac.Models
{
    public class CarrinhoCompra
    {

        private readonly AppDbContext _context;

        public CarrinhoCompra(AppDbContext context) {
            _context = context;
        }

        public string CarrinhoCompraId { get; set; }

        public List<CarrinhoCompraItem> CarrinhoCompraItems { get; set; }


        //Método para criar o carrinho de compras
        public static CarrinhoCompra GetCarrinho(IServiceProvider services) {
            //Define uma sessão
            ISession session =
                services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            //obtem um servico do tipo do nosso contexto
            var context = services.GetService<AppDbContext>();

            //Obtem ou gera o Id do carrinho    
            //A função ?? verifica se é nulo, se for diferente de nulo executa o que está antes
            // se for nulo, executa o que está na sua frente
            string carrinhoId = session.GetString("CarrinhoId") ?? Guid.NewGuid().ToString();

            //atribui o ID do carrinho na sessão
            session.SetString("CarrinhoId", carrinhoId);


            //retorna o carrinho com o contexto e o Id atribuido ou obtido
            return new CarrinhoCompra(context) {
                CarrinhoCompraId = carrinhoId
            };
        }



        // Método para Adicionar o itens ao carrinho
        public void AdicionarAoCarrinho(Lanche lanche) {


            //SingleOrDefault retorna um unico elemente que satisfaça a condição
            var carrinhoCompraItem = _context.CarrinhoCompraItens.SingleOrDefault(
                s => s.Lanche.LancheId == lanche.LancheId &&
                s.CarrinhoCompraId == CarrinhoCompraId);

            if (carrinhoCompraItem == null) {

                carrinhoCompraItem = new CarrinhoCompraItem {
                    CarrinhoCompraId = CarrinhoCompraId,
                    Lanche = lanche,
                    Quantidade = 1
                };
                _context.CarrinhoCompraItens.Add(carrinhoCompraItem);
            }
            else 
            {
                carrinhoCompraItem.Quantidade++;
            }
            _context.SaveChanges();
        }

        //Método para remover os itens do carrinho
        public int RemoverDoCarrinho(Lanche lanche) {

            var carrinhoCompraItem = _context.CarrinhoCompraItens.SingleOrDefault(
                s => s.Lanche.LancheId == lanche.LancheId &&
                s.CarrinhoCompraId == CarrinhoCompraId);

            var quantidadeLocal = 0;

            if(carrinhoCompraItem != null) 
            {
                if (carrinhoCompraItem.Quantidade > 1) 
                {
                    carrinhoCompraItem.Quantidade--;
                    quantidadeLocal = carrinhoCompraItem.Quantidade;
                }        
                else 
                {
                    _context.CarrinhoCompraItens.Remove(carrinhoCompraItem);
                }
            }
            _context.SaveChanges();
            return quantidadeLocal;
        }



        //Método que retorna a lista com os itens dos Carrinho
        public List<CarrinhoCompraItem> GetCarrinhoCompraItens() 
        {
            return CarrinhoCompraItems ??
                (CarrinhoCompraItems = 
                _context.CarrinhoCompraItens
                .Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
                .Include(s => s.Lanche)
                .ToList());
        }



        //Método que remove todos os itens de um carrinho
        public void LimparCarrinho() 
        {
            var carrinhoItens = _context.CarrinhoCompraItens
                                .Where(carrinho => carrinho.CarrinhoCompraId == CarrinhoCompraId);

            _context.CarrinhoCompraItens.RemoveRange(carrinhoItens);
            _context.SaveChanges();
        }


        //Método para gerar o valor total do Carrinho

        public decimal GetCarrinhoCompraTotal() {

            var total = _context.CarrinhoCompraItens.Where(c => c.CarrinhoCompraId == CarrinhoCompraId).Select(c => c.Lanche.Preco * c.Quantidade).Sum();
            return total;
        }

    }
}
