using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class NPController : MonoBehaviour
{

    private void Start()
    {
        var leaf1 = new Action(MoverNoLimiteDaZona);
        var leaf2 = new Condition(EstaNoPontoDeVisao);
        var leaf3 = new Action(LocalizarPlayer);
        // var selection2 = new Action(MoverAteOPlayer);
        var leaf4 = new Condition(ChegouNoPlayer);
        var leaf5 = new Action(CapturarPlayer);
    }

    void VigiarAZona()
    {
        
    }

    TaskStatus MoverNoLimiteDaZona(NPC npc)
    {
        return TaskStatus.SUCCESS;
    }

    bool EstaNoPontoDeVisao(NPC npc)
    {
        return true;
    }

    TaskStatus LocalizarPlayer(NPC npc)
    {
        return TaskStatus.SUCCESS;
    }

    TaskStatus MoverAteOPlayer(NPC npc)
    {
        return TaskStatus.SUCCESS;
    }

    bool ChegouNoPlayer(NPC npc)
    {
        return true;
    }

    TaskStatus CapturarPlayer(NPC npc)
    {
        return TaskStatus.SUCCESS;
    }
}

