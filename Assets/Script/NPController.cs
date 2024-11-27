using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class NPController : MonoBehaviour
{

    private Sequence vigiarZona;

    private Sequence perseguirPlayer;

    public NPC npc = new NPC();

    void Start()
    {
        GerarArvoreDeComportamento();

    }

    void Update()
    {
        if (vigiarZona.Execute(npc) == TaskStatus.SUCCESS)
        {
            if (perseguirPlayer.Execute(npc) == TaskStatus.SUCCESS)
            {
                Debug.Log("--- GAME OVER ---");
            }
        }
    }

    void GerarArvoreDeComportamento()
    {
        Action leaf1 = new Action(MoverNoLimiteDaZona);
        Condition leaf2 = new Condition(EstaNoPontoDeVisao);
        Action leaf3 = new Action(LocalizarPlayer);

        Action leaf4 = new Action(MoverAteOPlayer);
        Condition leaf5 = new Condition(ChegouNoPlayer);
        Action leaf6 = new Action(CapturarPlayer);

        List<Task> listSequence1 = new List<Task>();
        List<Task> listSequence2 = new List<Task>();

        listSequence1.Add(leaf1);
        listSequence1.Add(leaf2);
        listSequence1.Add(leaf3);
        listSequence2.Add(leaf4);
        listSequence2.Add(leaf5);
        listSequence2.Add(leaf6);


        vigiarZona = new Sequence(listSequence1);
        perseguirPlayer = new Sequence(listSequence2);
    }

    TaskStatus MoverNoLimiteDaZona(NPC npc)
    {
        Debug.Log("Movendo no limite da zona...");
        return TaskStatus.SUCCESS;
    }

    bool EstaNoPontoDeVisao(NPC npc)
    {
        Debug.Log("Ops! O player está no foco de visão");
        return true;
    }

    TaskStatus LocalizarPlayer(NPC npc)
    {
        Debug.Log("Localizando o player...");
        return TaskStatus.SUCCESS;
    }

    TaskStatus MoverAteOPlayer(NPC npc)
    {
        Debug.Log("Perseguindo o player...");
        return TaskStatus.SUCCESS;
    }

    bool ChegouNoPlayer(NPC npc)
    {
        Debug.Log("Cheguei no Player!");
        return true;
    }

    TaskStatus CapturarPlayer(NPC npc)
    {
        Debug.Log("Capturei ele!");
        return TaskStatus.SUCCESS;
    }
}

