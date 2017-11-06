using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class Sign : MonoBehaviour {

    protected Image imageBackground, textBoxBackground;
    protected Text textBox;

    [SerializeField]
    TextAsset inkText;
    private Story story;

    protected GameObject animator, cam, observer;

    private bool dialogue, forceMove, listenChoice, startRefresh;
    private int i;

    // Use this for initialization
    void Start () {
        dialogue = false;
        forceMove = false;
        listenChoice = false;
        startRefresh = false;

        i = 0;

        animator = transform.Find("inspect").gameObject;
        imageBackground = GameObject.Find("Canvas").transform.Find("TextBackground").gameObject.GetComponent<Image>();
        textBoxBackground = GameObject.Find("Canvas").transform.Find("Textbox Image").gameObject.GetComponent<Image>();
        textBox = textBoxBackground.transform.Find("Text").gameObject.GetComponentInChildren<Text>();


        imageBackground.gameObject.SetActive(false);
        textBoxBackground.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (startRefresh)
        {
            RefreshView();
        }

        if (listenChoice)
        {
            ListenForChoice();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            animator.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            animator.SetActive(false);
            EndStory();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Observe(collision.gameObject);
            }
        }
    }

    public void Observe(GameObject observerT)
    {
        imageBackground.gameObject.SetActive(true);
        textBoxBackground.gameObject.SetActive(true);
        //observer = observerT;
        //observer.GetComponent<PlayerInteractor>().canObserve = false;
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
        if (story.canContinue)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.E))
            {
                string text = story.Continue().Trim();
                textBox.text = text;
            }
        }

        else if (story.currentChoices.Count > 0)
        {
            textBox.text += "\n\n";
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Choice choice = story.currentChoices[i];
                textBox.text += (i + 1 + " - " + choice.text + "\n");
            }

            listenChoice = true;
        }
        else
        {
            EndStory();
        }
    }

    void EndStory()
    {
        imageBackground.gameObject.SetActive(false);
        textBoxBackground.gameObject.SetActive(false);
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
}
