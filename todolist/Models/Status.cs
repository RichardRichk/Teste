using System;
using System.Collections.Generic;

namespace todolist.Models;

public partial class Status
{
    public int IdStatus { get; set; }

    public string Status1 { get; set; } = null!;

    public virtual ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
}
