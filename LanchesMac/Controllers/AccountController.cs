using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager) {

            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Login(string returnUrl) {
            return View(new LoginViewModel() 
            {
                ReturnUrl = returnUrl
            });
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM) {
            if (!ModelState.IsValid) {
                return View(loginVM);
            }

            var user = await _userManager.FindByNameAsync(loginVM.UserName); //localiza o usuario pelo meno nome para checar se 
                                                                             //ja possui registro na tabela de usuario 

            if (user != null)    //Se o usuario foi diferente de Nulo 
            {

                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                //Usa com base o usuario e senha para logar.
                //O primeiro 'false' diz respeito a persistir o Cookie quando a sessão encerrar
                //E o segundo 'false' se o Login falhar, não iremos bloquear o acesso do usuario

                if (result.Succeeded) {
                    if (string.IsNullOrEmpty(loginVM.ReturnUrl)) {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(loginVM.ReturnUrl);
                }
            }


            ModelState.AddModelError("", "Falha ao realizar login");
            return View(loginVM);
        }


        [HttpGet]
        public IActionResult Register() 
        {
            
            return View(); 
        
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginViewModel registroVM ) 
        {
            if (ModelState.IsValid) 
            {
                var user = new IdentityUser(){ UserName = registroVM.UserName };

                var result = await _userManager.CreateAsync(user, registroVM.Password); //resultado do login utilizando as variaveis user e registroVM.Password
                            //utilizamos o método      AWAIT    quando vamos referenciar um método assincrono 
               
                if(result.Succeeded) 
                {

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    await _userManager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("Login", "Account");   //redireciona para a Action Login da Controlloer "ACCOUNT" 
                }
                else 
                {
                    this.ModelState.AddModelError("Registro", "Falha ao registrar o usuário");
                }
            }

            return View(registroVM);
        }



        [HttpPost]
        public async Task<IActionResult> Logout() {

            HttpContext.Session.Clear();                //Limpa os valores da session
            HttpContext.User = null;                    //Zera o valor atribuido ao usuario

            await _signInManager.SignOutAsync();        // Metodo de deslogar 
            return RedirectToAction("Index", "Home");   //Redirecionar para a Home do Controlloer HOME
        }


        public IActionResult AccessDenied() 
        {
            return View();  
        }
    }
}
