using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todolist.Contexts;
using todolist.Models;

namespace todolist.Controllers
{
    public class TarefaController : Controller
    {
        // Declaração do campo _context, que será usado para interagir com o banco de dados
        private readonly todolistContext _context;

        // Construtor do controlador, que recebe o contexto do banco de dados e o inicializa
        public TarefaController(todolistContext context)
        {
            _context = context;
        }

        // Ação para exibir a página inicial com um formulário para cadastro de tarefas
        public async Task<IActionResult> Index()
        {
            var tarefa = new Tarefa();  // Cria um objeto de tarefa vazio

            // Preenche os ViewBags com dados de Usuários, Prioridades e Status para os dropdowns
            ViewBag.Usuarios = _context.Usuarios.ToList();
            ViewBag.Prioridades = _context.Prioridades.ToList();
            ViewBag.Status = _context.Statuses.ToList();

            return View(tarefa);  // Retorna a view com o objeto de tarefa vazio
        }

        // Ação para listar as tarefas, agrupando-as por status
        public async Task<IActionResult> ListarTarefas()
        {
            // Carrega as tarefas, incluindo suas entidades relacionadas (Prioridade, Status e Usuário)
            var tarefas = await _context.Tarefas
                .Include(t => t.IdPrioridadeNavigation)
                .Include(t => t.IdStatusNavigation)
                .Include(t => t.IdUsuarioNavigation)
                .Where(t => t.IdStatusNavigation != null) // Filtra as tarefas com status
                .ToListAsync();

            // Agrupa as tarefas por status
            var tarefasAgrupadas = new
            {
                AFazer = tarefas.Where(t => t.IdStatusNavigation.IdStatus == 1).ToList(),
                EmProgresso = tarefas.Where(t => t.IdStatusNavigation.IdStatus == 2).ToList(),
                Concluido = tarefas.Where(t => t.IdStatusNavigation.IdStatus == 3).ToList()
            };

            // Preenche o ViewBag com as tarefas agrupadas
            ViewBag.TarefasAgrupadas = tarefasAgrupadas;

            // Retorna a view "Gerenciamento" com as tarefas agrupadas
            return View("Gerenciamento");
        }

        // Ação HTTP POST para criar uma nova tarefa
        [HttpPost]
        public async Task<IActionResult> Create(Tarefa tarefa)
        {
            if (ModelState.IsValid)  // Verifica se o modelo recebido é válido
            {
                // Define o status da tarefa como 1 (AFazer) caso não tenha sido atribuído
                if (tarefa.IdStatus == null)
                {
                    tarefa.IdStatus = 1;
                    tarefa.DataCadastro = DateOnly.FromDateTime(DateTime.Now);
                }

                _context.Add(tarefa);  // Adiciona a tarefa ao contexto do banco de dados
                await _context.SaveChangesAsync();  // Salva as alterações no banco de dados

                // Exibe uma mensagem de sucesso utilizando TempData
                TempData["MensagemSucesso"] = "Tarefa cadastrada com sucesso!";

                return RedirectToAction("Index");  // Redireciona para a página "Index"
            }

            // Preenche os ViewBags novamente caso o modelo seja inválido
            ViewBag.Usuarios = _context.Usuarios.ToList();
            ViewBag.Prioridades = _context.Prioridades.ToList();
            ViewBag.Status = _context.Statuses.ToList();

            return View(tarefa);  // Retorna a view com os dados da tarefa
        }

        // Ação HTTP POST para excluir uma tarefa com base no ID
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);  // Encontra a tarefa pelo ID
            if (tarefa != null)  // Se a tarefa for encontrada
            {
                _context.Tarefas.Remove(tarefa);  // Remove a tarefa do contexto
                await _context.SaveChangesAsync();  // Salva as alterações no banco de dados
            }
            return RedirectToAction("ListarTarefas");  // Redireciona para a lista de tarefas
        }

        // Ação HTTP GET para editar uma tarefa existente
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Busca a tarefa no banco de dados com as navegações para Prioridade, Status e Usuário
            var tarefa = await _context.Tarefas
                .Include(t => t.IdPrioridadeNavigation)
                .Include(t => t.IdStatusNavigation)
                .Include(t => t.IdUsuarioNavigation)
                .FirstOrDefaultAsync(t => t.IdTarefa == id);

            // Se a tarefa não for encontrada, exibe uma mensagem de erro
            if (tarefa == null)
            {
                TempData["Erro"] = "Tarefa não encontrada!";
                return RedirectToAction("ListarTarefas");
            }

            // Preenche os ViewBags com os dados necessários para os dropdowns
            ViewBag.Usuarios = _context.Usuarios.ToList();
            ViewBag.Prioridades = _context.Prioridades.ToList();
            ViewBag.Status = _context.Statuses.ToList();

            // Retorna a view com a tarefa preenchida
            return View("Index", tarefa);
        }

        // Ação HTTP POST para atualizar uma tarefa existente
        [HttpPost]
        public async Task<IActionResult> Update(Tarefa tarefa)
        {
            if (ModelState.IsValid)  // Verifica se o modelo é válido
            {
                // Carrega a tarefa original do banco de dados sem rastrear mudanças
                var tarefaOriginal = await _context.Tarefas.AsNoTracking().FirstOrDefaultAsync(t => t.IdTarefa == tarefa.IdTarefa);

                // Verifica se a tarefa foi encontrada
                if (tarefaOriginal == null)
                {
                    TempData["Erro"] = "Tarefa não encontrada!";
                    return RedirectToAction("ListarTarefas");
                }

                // Mantém o status original da tarefa
                tarefa.IdStatus = tarefaOriginal.IdStatus;

                _context.Update(tarefa);  // Atualiza a tarefa no contexto
                await _context.SaveChangesAsync();  // Salva as alterações no banco de dados

                TempData["Mensagem"] = "Tarefa atualizada com sucesso!";  // Exibe mensagem de sucesso
                return RedirectToAction("ListarTarefas");  // Redireciona para a lista de tarefas
            }

            // Preenche os ViewBags novamente caso o modelo seja inválido
            ViewBag.Usuarios = _context.Usuarios.ToList();
            ViewBag.Prioridades = _context.Prioridades.ToList();
            ViewBag.Status = _context.Statuses.ToList();

            return View("Cadastro", tarefa);  // Retorna a view com a tarefa preenchida
        }

        // Ação HTTP GET para criar uma nova tarefa
        [HttpGet]
        public IActionResult Create()
        {
            var tarefa = new Tarefa();  // Cria um novo objeto de tarefa
                                        // Preenche os ViewBags com dados para os dropdowns
            ViewBag.Usuarios = _context.Usuarios.ToList();
            ViewBag.Prioridades = _context.Prioridades.ToList();
            ViewBag.Status = _context.Statuses.ToList();

            return View(tarefa);  // Retorna a view com a tarefa vazia
        }

        // Ação HTTP POST para alterar o status de uma tarefa
        [HttpPost]
        public async Task<IActionResult> AlterarStatus(int id, int novoStatus)
        {
            // Busca a tarefa pelo ID
            var tarefa = await _context.Tarefas.FindAsync(id);

            // Se a tarefa não for encontrada, exibe uma mensagem de erro
            if (tarefa == null)
            {
                TempData["Erro"] = "Tarefa não encontrada!";
                return RedirectToAction("ListarTarefas");
            }

            // Atualiza o status da tarefa
            tarefa.IdStatus = novoStatus;

            // Salva as alterações no banco de dados
            _context.Tarefas.Update(tarefa);
            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Status da tarefa atualizado com sucesso!";  // Mensagem de sucesso

            return RedirectToAction("ListarTarefas");  // Redireciona para a lista de tarefas
        }
    }
}
