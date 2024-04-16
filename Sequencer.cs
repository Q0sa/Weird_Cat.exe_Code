using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using static PopUpHolder;

public class Sequencer : MonoBehaviour
{
    [SerializeField] public float CatSizeIncrease = 1.5f;
    [SerializeField] public float MaxScaleOfCat = 17f;

    [System.Serializable] public class EventEntry {

        [SerializeField] public float delayFromPrevious;
        [SerializeField] public enum EventType { CatSpeech, CatAction, QuestionOrGarbageCollect, CreditsAndQuitGame };
        [SerializeField] public EventType eventType = EventType.CatSpeech;

        
        [SerializeField] public CatSpeech[] CatSpeechSelected;
        [SerializeField] public CatAction[] CatActionSelected;
        [SerializeField] public QuestionOrGarbageCollectOption[] QuestionOrGarbageCollectSelected;

        [HideInInspector] public enum STATE { WaitingToStart, Running, Complete };
        [HideInInspector] public STATE currentState = STATE.WaitingToStart;

    }

    [System.Serializable] public class CatSpeech{

        [SerializeField] public enum CatDialogueType { Custom, NameSpeech, FileCountSpeech }
        [SerializeField] public CatDialogueType DialogueType = CatDialogueType.Custom;

        [SerializeField] public string dialogue;
        [SerializeField] public bool isInvisible;
        [SerializeField] public Vector2 position = new Vector2(0, -8);
    }

    [System.Serializable] public class CatAction{
    
        [SerializeField] public enum SelectedAction { ClearText, WalkLeft, WalkRight, EnterFiles, ExitFiles, Disentegrate };
        [SerializeField] public SelectedAction selectedAction = SelectedAction.WalkLeft;
    }

    [System.Serializable] public class QuestionOrGarbageCollectOption
    {

        [SerializeField] public bool IncreaseSizeOnNo = false;
        [SerializeField] public bool FinalCatQuestion = false;

        [SerializeField] public TypeOfButtons button_type;
        [SerializeField] public TypeOfIcon icon_type;

        [SerializeField] public string title;
        [SerializeField] public string content;

    }


    [SerializeField] public List<EventEntry> sequenceList = new List<EventEntry>();



    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    public IntPtr windowHandle;

    private PopUpHolder popUpHolder;
    private CatScript catScript;

    private int eventIndex = 0;
    public void Awake()
    {

        windowHandle = GetActiveWindow();
       
    }

    public void Start() {

        popUpHolder = gameObject.GetComponent<PopUpHolder>();
        catScript = GameObject.Find("Cat").GetComponent<CatScript>();

    }

    public void Update() {

        RunSequencer();
        Debug.Log(eventIndex);
    
    }

    void RunSequencer() {

        try { 

            if (sequenceList[eventIndex] != null) {

                switch (sequenceList[eventIndex].currentState) {

                    case EventEntry.STATE.WaitingToStart:

                        StartCoroutine(PlayEvent(sequenceList[eventIndex]));
                        sequenceList[eventIndex].currentState = EventEntry.STATE.Running;

                        break;

                    case EventEntry.STATE.Running:
                        break;

                    case EventEntry.STATE.Complete:

                        sequenceList[eventIndex].currentState = EventEntry.STATE.Complete;
                        eventIndex++;

                        break;

                    default:
                        break;
                }

                if (catScript.isActionComplete && sequenceList[eventIndex].eventType == EventEntry.EventType.CatAction) {

                    sequenceList[eventIndex].currentState = EventEntry.STATE.Complete;
                    catScript.isActionComplete = false;

                }

            }
        }
        catch  {


        }



    }


    IEnumerator PlayEvent(EventEntry entry) {

        yield return new WaitForSecondsRealtime(entry.delayFromPrevious);

        switch (entry.eventType)
        {
            case EventEntry.EventType.CatSpeech:
                
                CatSpeak(entry);

                sequenceList[eventIndex].currentState = EventEntry.STATE.Complete;

                break;
        
            case EventEntry.EventType.CatAction:

                PerfromAction(entry);

                break;
        
            case EventEntry.EventType.QuestionOrGarbageCollect:
        
                ASK_AGAIN:

                yield return new WaitForSecondsRealtime(.5f);

                int ID = popUpHolder.CreatePopUp(windowHandle, 
                                        entry.QuestionOrGarbageCollectSelected[0].button_type,
                                        entry.QuestionOrGarbageCollectSelected[0].icon_type, 
                                        entry.QuestionOrGarbageCollectSelected[0].title, 
                                        entry.QuestionOrGarbageCollectSelected[0].content);


                if (entry.QuestionOrGarbageCollectSelected[0].button_type == TypeOfButtons.YESNO_Button) {

                    if (entry.QuestionOrGarbageCollectSelected[0].IncreaseSizeOnNo)
                    {
                        if (ID == 6)
                        {
                            if (catScript.gameObject.transform.position.x < 0)
                                catScript.gameObject.transform.localScale = Vector3.one;
                            else
                                catScript.gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
                           
                        }

                        //no 
                        if (ID == 7)
                        {

                            if (catScript.gameObject.transform.localScale.x <= MaxScaleOfCat)
                            {
                                catScript.gameObject.transform.localScale += new Vector3(CatSizeIncrease, CatSizeIncrease);

                                if (catScript.gameObject.transform.localScale.x >= 11.5) {

                                    catScript.gameObject.transform.position += new Vector3(-.5f, 0f);

                                }

                            }

                            goto ASK_AGAIN;

                        }
                    }

                    else if (entry.QuestionOrGarbageCollectSelected[0].FinalCatQuestion) {

                        //yes
                        if (ID == 6)
                            catScript.DisplayText("...What? YOU WOULDN’T DARE.", false, new Vector2(0, -8));


                        //no 
                        if (ID == 7)
                            catScript.DisplayText("See?! Please don’t delete me!", false, new Vector2(0, -8));


                    }

                }

                sequenceList[eventIndex].currentState = EventEntry.STATE.Complete;
                break;

            case EventEntry.EventType.CreditsAndQuitGame:

                popUpHolder.CreatePopUp(windowHandle, TypeOfButtons.OK_Button, TypeOfIcon.NONE,
                                           "Created By",
                                           "William Melander, Shai Panaga & Albin Westberg \n\nThankyou for playing!");


                Application.Quit();

                break;
        
            default:
                sequenceList[eventIndex].currentState = EventEntry.STATE.Complete;

                break;

        }

    }

    void CatSpeak(EventEntry entry) {


        switch (entry.CatSpeechSelected[0].DialogueType)
        {
            case CatSpeech.CatDialogueType.Custom:

                catScript.DisplayText(entry.CatSpeechSelected[0].dialogue, entry.CatSpeechSelected[0].isInvisible, entry.CatSpeechSelected[0].position);

                break;

            case CatSpeech.CatDialogueType.NameSpeech:
               

                string FirstPart = "Woah, that was weird, anyway " + Environment.UserName + " could I take a peek into your files?";

                catScript.DisplayText(FirstPart, entry.CatSpeechSelected[0].isInvisible, entry.CatSpeechSelected[0].position);


                break;

            case CatSpeech.CatDialogueType.FileCountSpeech:

                string SecondPart = "I noticed you had " + Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "*", SearchOption.AllDirectories).Length + " files on your desktop, is that normal?";

                catScript.DisplayText(SecondPart, entry.CatSpeechSelected[0].isInvisible, entry.CatSpeechSelected[0].position);

                break;

            default:
                break;
        }


    }
    void PerfromAction(EventEntry entry)
    {

        switch (entry.CatActionSelected[0].selectedAction)
        {

            case CatAction.SelectedAction.ClearText:
                try
                {
                    catScript.RemoveText();
                }
                catch { }

                sequenceList[eventIndex].currentState = EventEntry.STATE.Complete;

                break;

            case CatAction.SelectedAction.WalkLeft:

                catScript.BeginWalkOnScreen(CatScript.Direction.LeftSide);

                break;

            case CatAction.SelectedAction.WalkRight:

                catScript.BeginWalkOnScreen(CatScript.Direction.RightSide);

                break;

            case CatAction.SelectedAction.EnterFiles:

                
                catScript.BeginWalkExitEnterScreen(CatScript.EnterExit.Enter);

                break;

            case CatAction.SelectedAction.ExitFiles:

                catScript.BeginWalkExitEnterScreen(CatScript.EnterExit.Exit);

                break;

            case CatAction.SelectedAction.Disentegrate:

                StartCoroutine(catScript.Disintegrat());

                break;

            default:
                break;
        }

    }

}

