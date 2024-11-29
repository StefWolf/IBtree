using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Task
{
    private List<Task> tasks;

   // Construtor para inicializar a sequ�ncia de tarefas
        public Sequence(List<Task> tasks)
        {
            this.tasks = tasks;
        }

        // Sobrescreve o m�todo Execute para executar as tarefas em sequ�ncia
        public override TaskStatus Execute(NPC npc)
        {
            foreach (var task in tasks)
            {
                var status = task.Execute(npc);
                if (status != TaskStatus.SUCCESS)
                {
                    return status;
                }
            }
            return TaskStatus.SUCCESS;
        }
    
}
