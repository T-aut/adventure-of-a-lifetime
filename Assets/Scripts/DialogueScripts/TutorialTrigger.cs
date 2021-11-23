using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    private DialogueTrigger dialogueTrigger;
    // Start is called before the first frame update
    void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
        dialogueTrigger.StartDialogue();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
