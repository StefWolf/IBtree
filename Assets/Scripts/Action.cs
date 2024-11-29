using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : Task
{
    private Func<NPC, TaskStatus> fn;

    // Construtor que recebe uma fun��o como argumento
    public Action(Func<NPC, TaskStatus> fn)
    {
        this.fn = fn;
    }

    // Sobrescreve o m�todo Execute para chamar a fun��o fornecida
    public override TaskStatus Execute(NPC npc)
    {
        return fn(npc);
    }
}
