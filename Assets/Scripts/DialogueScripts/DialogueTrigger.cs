using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(messages, actors);
    }

}



[System.Serializable]
public class Message
{
    public int actorId;
    public string message;
    public bool isUnique;
    public int uniqueId;
}

[System.Serializable]
public class Actor
{
    public string name;
    public Sprite sprite;
}