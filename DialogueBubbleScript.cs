using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueBubbleScript : MonoBehaviour
{

    Transform catPos;

    Vector3 bubbleScale;
    Vector3 textScale;

    Vector3 LeftDialoguePos = new Vector3(-2.7f, -5.4f, 90f);
    Vector3 RightDialoguePos = new Vector3(2.7f, -5.4f, 90f);

    Text dialogueText;
    Image dialogueBubbleImg;

    
    private void Start()
    {
        Debug.Log(transform.position);

        catPos = GameObject.Find("Cat").transform;

        dialogueBubbleImg = transform.GetComponent<Image>();
        dialogueText = transform.GetChild(0).GetComponent<Text>();

        bubbleScale = transform.localScale;
        textScale = dialogueText.transform.localScale;


        dialogueBubbleImg.enabled = false;
        dialogueText.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {

       
            MakeDialogueBubbleFaceCentre();
            PositionDialogueBubble();


        if (!catPos.gameObject.activeSelf)
            Destroy(gameObject);

    }

    private void MakeDialogueBubbleFaceCentre()
    {

        if (catPos.position.x < 0.0f ) {

            transform.localScale = bubbleScale;

            dialogueText.transform.localScale = new Vector3(textScale.x, textScale.y, textScale.z);

        }

        else if (catPos.position.x > 0.0f)
        {

            transform.localScale = new Vector3(-bubbleScale.x, bubbleScale.y, bubbleScale.z);

            dialogueText.transform.localScale = new Vector3(-textScale.x, textScale.y, textScale.z); 

        }

    }

    private void PositionDialogueBubble() {

        if (catPos.position.y == -8)
        {
            if (catPos.position.x < 0.0f)
                transform.position = LeftDialoguePos;           

            else if (catPos.position.x > 0.0f)
                transform.position = RightDialoguePos;
            
        } else
        {

            transform.position = catPos.position;

        }

    }



}
