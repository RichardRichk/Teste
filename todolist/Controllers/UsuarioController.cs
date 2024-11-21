using Microsoft.AspNetCore.Mvc;
using todolist.Contexts;
using todolist.Models;

namespace todolist.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly todolistContext _context;

        public UsuarioController(todolistContext context)
        {
            _context = context;
        }

        // Ação para exibir a página inicial do controlador
        public IActionResult Index()
        {
            // Retorna a view associada à ação Index
            return View();
        }

        // Ação HTTP POST para criar um novo usuário no banco de dados
        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            // Verifica se o modelo recebido é válido (se todos os dados necessários estão corretos)
            if (ModelState.IsValid)
            {
                // Adiciona o usuário ao contexto (preparando para ser salvo no banco de dados)
                _context.Add(usuario);

                // Salva as alterações no banco de dados de forma assíncrona
                await _context.SaveChangesAsync();

                // Exibe uma mensagem de sucesso usando TempData, que será acessada na próxima requisição
                TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";

                // Redireciona o usuário para a ação "Index" após a criação bem-sucedida
                return RedirectToAction("Index");
            }

            // Se o modelo não for válido, retorna a view "Index" novamente, permitindo ao usuário corrigir os dados
            return View("Index");
        }
    }
}
