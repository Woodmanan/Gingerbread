using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public TMP_Text text;

    int c = 0;

    private void Start()
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.StartDialogue(dialogue);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            Debug.Log(c);
            if (c == dialogue.sentences.Length)
            {
                dialogueManager.DisplayNextSentence();
                gameObject.SetActive(false);


            }

            


            else
            {
                dialogueManager.DisplayNextSentence();
                c++;
            }
        }
    }
}
    
