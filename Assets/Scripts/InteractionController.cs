using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    Transform feedbackCurrent;
    Transform inventario;
    private void Start()
    {
        feedbackCurrent = GameObject.FindWithTag("Player").transform.GetChild(1).GetChild(0);
        inventario = GameObject.FindWithTag("Player").transform.GetChild(1).GetChild(1);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int keys = GameObject.FindWithTag("Player").GetComponent<CollectController>().GetKeysCount();
            if (Input.GetKeyDown(KeyCode.E) && keys > 0)
            {
                GameObject.FindWithTag("Player").GetComponent<CollectController>().SetKeysCount();
                GetComponent<Animator>().SetBool("IsOpened", true);
                feedbackCurrent.GetChild(1).GetComponent<TextMeshProUGUI>().text = gameObject.name + " opened!";
                FindObjectOfType<FadeController>().FadeInForFadeOut(2f);
                FindFirstObjectByType<GameController>().SetChestCount();

                int slotsCount = inventario.childCount;
                for (int ii = 0; ii < slotsCount; ii++)
                {
                    Transform slot = inventario.GetChild(ii);
                    if (slot.GetChild(0).gameObject.activeSelf)
                    {
                        slot.GetChild(0).GetComponent<Image>().sprite = null;
                        slot.GetChild(0).gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }
}
