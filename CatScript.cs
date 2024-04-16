using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CatScript : MonoBehaviour
{

    [SerializeField] private Vector2 LWalkEndPos = new Vector2(-10f, -8f);
    [SerializeField] private Vector2 RWalkEndPos = new Vector2(10f, -8f);
    [SerializeField] private Vector2 ExitScreenEndPos = new Vector2(20f, -8f);

    [SerializeField] public float def_speed = .5f;
    float speed;

    [HideInInspector] public bool isActionComplete = true;
    private Vector2 currentTarget;
    private Animator animator;
    private bool shouldRun = false;
    private bool SetFrameRun = false;

    GameObject DialogueBubble;
    
    void Start()
    {

        currentTarget = transform.position;
        animator = GetComponent<Animator>();
        DialogueBubble = GameObject.Find("Cat Dialogue Bubble");

        speed = def_speed;
    }

    public void FixedUpdate()
    {
        if (!ReachedTargetPosition() && shouldRun)
        {

            gameObject.GetComponent<SpriteRenderer>().enabled = true;

            animator.SetBool("Walking", true);

            float step = speed * Time.fixedDeltaTime;
            transform.position = Vector2.MoveTowards(transform.position, currentTarget, step);

        } else if (ReachedTargetPosition() && shouldRun) {

            shouldRun = false;
            isActionComplete = true;

            animator.SetBool("Walking", false);
            speed = def_speed;

            transform.localScale = transform.position.x > 0 ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);

        }
    }


    public void DisplayText(string Dialogue, bool isInvisible, Vector2 CatPos) {

        RemoveText();

        if (isInvisible) {

            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            transform.position = CatPos;

        }

        DialogueBubble.transform.GetChild(0).GetComponent<Text>().text = Dialogue;

        DialogueBubble.GetComponent<Image>().enabled = true;
        DialogueBubble.transform.GetChild(0).GetComponent<Text>().enabled = true;


    }

    public void RemoveText() {

        DialogueBubble.GetComponent<Image>().enabled = false;
        DialogueBubble.transform.GetChild(0).GetComponent<Text>().enabled = false;

    }

    public enum Direction { LeftSide, RightSide }
    public void BeginWalkOnScreen(Direction input) {

        isActionComplete = false;


        switch (input)
        {
            case Direction.LeftSide:

                currentTarget = LWalkEndPos;
                transform.localScale = new Vector3(-1f, 1f, 1f);

                break;

            case Direction.RightSide:

                currentTarget = RWalkEndPos;
                transform.localScale = new Vector3(1f, 1f, 1f);

                break;

            default:
                break;
        }

        shouldRun = true;

    }

    public enum EnterExit { Enter, Exit }
    public void BeginWalkExitEnterScreen(EnterExit input) {

        isActionComplete = false;

        switch (input)
        {
            case EnterExit.Exit:

                GetComponent<SpriteRenderer>().enabled = true;

                transform.position = new Vector2(-20f, -8f);
                currentTarget = LWalkEndPos;
                speed = 10.0f;
                transform.localScale = new Vector3(1f, 1f, 1f);

                break;

            case EnterExit.Enter:

                speed = 8.0f;
                currentTarget = ExitScreenEndPos;
                transform.localScale = new Vector3(1f, 1f, 1f);

                break;

            default:
                break;
        }

        shouldRun = true;

    }


    bool ReachedTargetPosition() {

        return transform.position == new Vector3(currentTarget.x, currentTarget.y);

    }

    public IEnumerator Disintegrat() { 

        transform.localScale = -Vector3.one;

        animator.SetBool("Walking", true);


        yield return new WaitForSecondsRealtime(2.0f);

        transform.localScale = new Vector3(2f, 10.0f, 1.0f);

        yield return new WaitForSecondsRealtime(.1f);

        transform.localScale = new Vector3(10f, 2.0f, 1.0f);

        yield return new WaitForSecondsRealtime(.2f);

        transform.position = new Vector3(-42.88f, -20.02f, 0.0f);
        transform.localScale = new Vector3(48f, 48f, 48f);

        yield return new WaitForSecondsRealtime(.5f);

        isActionComplete = true;

        yield return new WaitForSecondsRealtime(.001f);

        gameObject.SetActive(false);


    }

}
