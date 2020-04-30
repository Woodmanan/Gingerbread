using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject[] nexts;
    public GameObject[] others;

    public Text text;

    int c = 0;

    void OnMouseDown()
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        Debug.Log(c);
        if (c == dialogue.sentences.Length)
        {
            dialogueManager.DisplayNextSentence();
            gameObject.SetActive(false);
            foreach (GameObject next in nexts)
            {
                if (next != null)
                {
                    next.SetActive(true);
                }

            }

            foreach (GameObject other in others)
            {
                if (other != null)
                {
                    other.SetActive(false);
                }

            }
        }

        else if (c == 0)
        {
            dialogueManager.StartDialogue(dialogue);
            c++;
        }


        else
        {
            dialogueManager.DisplayNextSentence();
            c++;
        }
    }

}
