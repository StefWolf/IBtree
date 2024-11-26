using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : Task
{
    private Func<NPC, bool> fn;

    // Construtor que recebe uma fun��o de condi��o como argumento
    public Condition(Func<NPC, bool> fn)
    {
        this.fn = fn;
    }

    // Sobrescreve o m�todo Execute para verificar a condi��o
    public override TaskStatus Execute(NPC npc)
    {
        return fn(npc) ? TaskStatus.SUCCESS : TaskStatus.FAILURE;
    }
}

