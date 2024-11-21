using System;
using System.Collections.Generic;

namespace todolist.Models;

public partial class Tarefa
{
    public int IdTarefa { get; set; }

    public string? Descricao { get; set; }

    public string? Setor { get; set; }

    public DateOnly? DataCadastro { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdPrioridade { get; set; }

    public int? IdStatus { get; set; }

    public virtual Prioridade? IdPrioridadeNavigation { get; set; }

    public virtual Status? IdStatusNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
