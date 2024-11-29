using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectController : MonoBehaviour
{
    Transform feedbackCurrent;
    Transform inventario;
    public Sprite key;
    public int keysCount = 0;
    private void Start()
    {
        feedbackCurrent = transform.GetChild(1).GetChild(0);
        inventario = transform.GetChild(1).GetChild(1);
    }

    public int GetKeysCount()
    {
        return keysCount;
    }

    public void SetKeysCount()
    {
        keysCount--;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                feedbackCurrent.GetChild(1).GetComponent<TextMeshProUGUI>().text = collision.gameObject.name + " collected!";
                FindObjectOfType<FadeController>().FadeInForFadeOut(2f);

                int slotsCount = inventario.childCount;
                for (int ii = 0; ii < slotsCount; ii++)
                {
                    Transform slot = inventario.GetChild(ii);
                    if (!slot.GetChild(0).gameObject.activeSelf)
                    {
                        slot.GetChild(0).GetComponent<Image>().sprite = key;
                        slot.GetChild(0).gameObject.SetActive(true);
                        keysCount++;
                        Destroy(collision.gameObject);
                        break;
                    }
                }
            }
        }
    }
}
