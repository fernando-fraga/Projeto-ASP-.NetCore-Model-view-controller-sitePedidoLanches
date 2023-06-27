using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class CarrinhoCompraController : Controller
    {
        private readonly ILancheRepository _lancheRepository;
        private readonly CarrinhoCompra _carrinhoCompra;

        public CarrinhoCompraController(ILancheRepository lancheRepository,
            CarrinhoCompra carrinhoCompra) {
            _lancheRepository = lancheRepository;
            _carrinhoCompra = carrinhoCompra;
        }

        public IActionResult Index() 
        {

            var itens = _carrinhoCompra.GetCarrinhoCompraItens();
            _carrinhoCompra.CarrinhoCompraItems = itens;

            var carrinhoCompraVM = new CarrinhoCompraViewModel 
            {
                CarrinhoCompra = _carrinhoCompra,
                CarrinhoCompraTotal = _carrinhoCompra.GetCarrinhoCompraTotal() 
            };

            return View(carrinhoCompraVM);
        }

        [Authorize]
        //Metodo ação para adicionar o item selecionado no Carrinho
        public IActionResult AdicionarItemNoCarrinhoCompra (int lancheId) 
        {
                                   //Acessa o repositorio de lanches, na tabela de Lanches
            var lancheSelecionado = _lancheRepository.Lanches.
                FirstOrDefault(p => p.LancheId == lancheId);   //Metodo FirstOrDefault retorna o primeiro item que atende satisfaça a condição

            if (lancheSelecionado != null) 
            {
                _carrinhoCompra.AdicionarAoCarrinho(lancheSelecionado);
            }
            return RedirectToAction("Index");
        }


        [Authorize]
        //Metodo ação para remover o item selecionado no Carrinho
        public IActionResult RemoverItemDoCarrinhoCompra (int lancheId) 
        {
            //Acessa o repositorio de lanches, na tabela de Lanches
            var lancheSelecionado = _lancheRepository.Lanches.
                FirstOrDefault(p => p.LancheId == lancheId);   //Metodo FirstOrDefault retorna o primeiro item que atende satisfaça a condição

            if (lancheSelecionado != null) {
                _carrinhoCompra.RemoverDoCarrinho(lancheSelecionado);
            }
            return RedirectToAction("Index");
        }


    }
}
