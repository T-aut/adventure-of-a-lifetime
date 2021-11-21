using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    
    public UnityEngine.UI.Image actorImage;

    public Text actorName;
    private Vector3 initialPosition;
    private Vector3 currentPosition;
    public Text messageText;
    public PlayerMovement playerMovement;
    public Animator playerAnimator;
    public RectTransform backgroundBox;
    private Message[] currentMessages;
    private Actor[] currentActors;
    private bool isMessageSpecial;
    private int specialMessageId;
    private int activeMessage = 0;
    public static bool isActive = false;

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;
        Debug.Log("Started conversation! Loaded messages: " + messages.Length);
        DisplayMessage();
        backgroundBox.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo();
        playerMovement._isControlEnabled = false;

    }

    void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.message;
        isMessageSpecial = messageToDisplay.isUnique;
        specialMessageId = messageToDisplay.uniqueId;
        //isMessageSpecial = messageToDisplay.isSpecial;
        // specialMessageId = messageToDisplay.specialType;
        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
        AnimateTextColor();
    }

    public void NextMessage()
    {
        activeMessage++;
        Debug.Log(activeMessage);
        Debug.Log(currentMessages.Length);
        if (activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            Debug.Log("Conversation ended");
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            isActive = false;
            playerMovement._isControlEnabled = true;
        }
    }

    void AnimateTextColor()
    {
        LeanTween.alphaText(messageText.rectTransform, 0, 0);
        LeanTween.alphaText(messageText.rectTransform, 1, 0.5f);
    }

    // Start is called before the first frame update
    void Start()
    {

        backgroundBox.transform.localScale = Vector3.zero;
        initialPosition = GameObject.FindWithTag("Player").transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) playerMovement._isControlEnabled = false;
        currentPosition = GameObject.FindWithTag("Player").transform.position;
        if (Input.GetKeyDown(KeyCode.Return) && isActive && isMessageSpecial == false)
        {
            NextMessage();
        }
        else if (isActive && isMessageSpecial)
        {
            if (specialMessageId == 1)
            {
                playerMovement._isControlEnabled = true;
                Debug.Log(Vector2.Distance(initialPosition, currentPosition));
                if (Vector2.Distance(initialPosition, currentPosition) > 3f)
                {
                    NextMessage();
                    playerMovement._isControlEnabled = false;

                }
            }

            else if (specialMessageId == 2)
            {
                playerMovement._isControlEnabled = true;
                if (playerAnimator.GetBool("IsAttacking"))
                {
                    NextMessage();
                    playerMovement._isControlEnabled = false;
                }
            }
            else if (specialMessageId == 3)
            {
                playerMovement._isControlEnabled = true;
                if (playerAnimator.GetBool("IsCasting"))
                {
                    NextMessage();
                    playerMovement._isControlEnabled = false;
                }
            }

        }
    }
    
}