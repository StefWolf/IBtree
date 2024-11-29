using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskStatus
{
    RUNNING = 1,
    SUCCESS = 2,
    FAILURE = 3
}

public abstract class Task
{
    /// <summary>
    /// Executa a tarefa, retornando um dos poss�veis status.
    /// Por padr�o, se a subclasse n�o reescrever, retorna FAILURE.
    /// </summary>
    /// <param name="npc">Objeto do tipo GameObject</param>
    /// <returns>Status da Tarefa</returns>
    public virtual TaskStatus Execute(NPC npc)
    {
        return TaskStatus.FAILURE;
    }
}
