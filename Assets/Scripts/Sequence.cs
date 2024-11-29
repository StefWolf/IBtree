using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Task
{
    private List<Task> tasks;

   // Construtor para inicializar a sequência de tarefas
        public Sequence(List<Task> tasks)
        {
            this.tasks = tasks;
        }

        // Sobrescreve o método Execute para executar as tarefas em sequência
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
