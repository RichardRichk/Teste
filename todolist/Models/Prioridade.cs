using System;
using System.Collections.Generic;

namespace todolist.Models;

public partial class Prioridade
{
    public int IdPrioridade { get; set; }

    public string Titulo { get; set; } = null!;

    public virtual ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
}
