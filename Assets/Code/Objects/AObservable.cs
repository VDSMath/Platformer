using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

abstract public class AObservable : MonoBehaviour {


    protected GameObject cam, observer;
    protected bool canMove, canMoveBack, moveBack;
    protected float journeyLength, startTime;
    private Vector3 iniz;

    [SerializeField]
    private Image instructionImage;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected Image imageBackground, textBackground;
    [SerializeField]
    protected Text textBox;

    [SerializeField]
    TextAsset inkText;
    private Story story;
	
    private bool dialogue, forceMove, listenChoice, startRefresh;
    private int i;

    private void Start()
    {
        canMove = false;
        canMoveBack = false;
        dialogue = false;
        forceMove = false;
        listenChoice = false;
        moveBack = false;
        startRefresh = false;

        i = 0;

        textBackground = transform.parent.Find("Canvas").transform.Find("Textbox Image").gameObject.GetComponent<Image>();
        textBox = textBackground.transform.Find("Text").gameObject.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        /*
        if (canMove)
        {
            Move();
        }
        

        if ((Input.GetButtonDown("Fire3") && canMoveBack) || (Input.GetButtonDown("Fire1") && forceMove))
        {
            if (canMove)
            {
                canMove = false;
            }

            HideText();
            //observer.GetComponent<PlayerMovement>().SetMove(true);
            startTime = Time.time;
            journeyLength = Vector2.Distance(cam.transform.position, iniz);
            moveBack = true;
            canMoveBack = false;
            forceMove = false;
        }
        
        

        if (moveBack)
        {
            MoveBack();
        }
        */

        if (startRefresh)
        {
            RefreshView();
        }

        if (listenChoice)
        {
            ListenForChoice();
        }
    }

    void StartMoveBack()
    {

    }

    public void Observe(GameObject observerT)
    {
        imageBackground.gameObject.SetActive(true);
        textBackground.gameObject.SetActive(true);
        //observer = observerT;
        //observer.GetComponent<PlayerInteractor>().canObserve = false;
        cam = Camera.main.gameObject;
        iniz = cam.transform.position;
        startTime = Time.time;
        journeyLength = Vector2.Distance(iniz, transform.position);
        canMove = true;

        StartStory();
    }

    private void StartStory()
    {
        story = new Story(inkText.text);
        string text = story.Continue().Trim();
        textBox.text = text;
        startRefresh = true;
    }

    void RefreshView()
    {

        if(textBox.text == ".instrucoes.")
        {
            instructionImage.gameObject.SetActive(true);
        }
        else
        {
            instructionImage.gameObject.SetActive(false);
        }


        if (story.canContinue)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                string text = story.Continue().Trim();
                textBox.text = text;
            }
        }

        else if(story.currentChoices.Count > 0)
        {
            textBox.text += "\n\n";
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Choice choice = story.currentChoices[i];
                textBox.text += (i + 1 + " - " + choice.text + "\n");
            }

            listenChoice = true;
        } else
        {
            forceMove = true;
        }
    }
    
    void ListenForChoice()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            listenChoice = false;
            SelectChoice(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            listenChoice = false;
            SelectChoice(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            listenChoice = false;
            SelectChoice(2);
        }
    }

    void SelectChoice(int choice)
    {
        story.ChooseChoiceIndex(choice);
        string text = story.Continue().Trim();
        textBox.text = text;
    }

    protected void MoveBack()
    {
        float distCovered = (Time.time - startTime) * moveSpeed;
        float fracJourney = distCovered / journeyLength;
        cam.transform.position = Vector2.Lerp(cam.transform.position, iniz, fracJourney);
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, iniz.z);
        
        if (fracJourney >= 0.3f)
        {
            if(observer != null)
            {
                //observer.GetComponent<PlayerInteractor>().canObserve = true;
            }
            moveBack = false;
            cam.transform.position = iniz;
            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, iniz.z);
        }
    }

    protected void Move()
    {
        float distCovered = (Time.time - startTime) * moveSpeed;
        float fracJourney = distCovered / journeyLength;
        cam.transform.position = Vector2.Lerp(cam.transform.position, this.transform.position, fracJourney);
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, iniz.z);

        if (fracJourney >= 0.1)
        {
            canMoveBack = true;
        }

        if (fracJourney >= 0.3f)
        {
            cam.transform.position = this.transform.position;
            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, iniz.z);
            canMove = false;

            ShowText();
        }
    }

    protected void ShowText()
    {
        textBackground.gameObject.SetActive(true);
    }

    protected void HideText()
    {
        textBackground.gameObject.SetActive(false);
        imageBackground.gameObject.SetActive(false);
    }
}
