using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : Task
{
    private Func<NPC, TaskStatus> fn;

    // Construtor que recebe uma função como argumento
    public Action(Func<NPC, TaskStatus> fn)
    {
        this.fn = fn;
    }

    // Sobrescreve o método Execute para chamar a função fornecida
    public override TaskStatus Execute(NPC npc)
    {
        return fn(npc);
    }
}
