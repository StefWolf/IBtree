using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : Task
{
    private Func<NPC, bool> fn;

    // Construtor que recebe uma função de condição como argumento
    public Condition(Func<NPC, bool> fn)
    {
        this.fn = fn;
    }

    // Sobrescreve o método Execute para verificar a condição
    public override TaskStatus Execute(NPC npc)
    {
        return fn(npc) ? TaskStatus.SUCCESS : TaskStatus.FAILURE;
    }
}

