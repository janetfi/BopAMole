using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIbehavior : MonoBehaviour {

    public GameStatesAndData gameStateAndData;  // used to switch between game states, and execute the corresponding code.

    GameObject startScreen; // used to hide the start screen when user clicks play button.
    GameObject endScreen;   // used to show the end screen when the game is over.

    GameObject cloud1;  // used to animate some clouds before hiding the start screen.
    GameObject cloud2;
    RectTransform rtCloud1;
    RectTransform rtCloud2;
    public float moveInterval;
    public int moveLoops;
    int j = 0;

    AudioSource audSource;


    // Use this for initialization
    void Start () {

        // get references to stuff I'll need in Update.
        startScreen = GameObject.Find("StartScreen");
        cloud1 = GameObject.Find("Cloud1");
        cloud2 = GameObject.Find("Cloud2");
        rtCloud1 = cloud1.GetComponent<RectTransform>();
        rtCloud2 = cloud2.GetComponent<RectTransform>();
        endScreen = GameObject.Find("EndScreen");

        // no need to GameObject.Find("StateObject") for access to the 
        // gameStateAndData.currentGameState because I set the references in the editor.

        audSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update () {

        // cloudsMoving is true when player clicks the play button.
        if (gameStateAndData.currentGameState == GameStatesAndData.gameStateEnum.cloudsMoving)
        {
            // move the clouds a bit each frame.
            rtCloud1.Translate(moveInterval * Time.deltaTime, 0, 0);
            rtCloud2.Translate(-moveInterval * Time.deltaTime, 0, 0);

            // increment counter.
            j++;

            // if the clouds have moved as far as I want:
            // - set cloudIsMoving to false to make sure this block isn't run again.
            // - set cloudIsDoneMoving to true to make sure the next block is run.
            if (j > moveLoops)
            {
                gameStateAndData.currentGameState = GameStatesAndData.gameStateEnum.cloudsDoneMoving;
            }
        }

        if (gameStateAndData.currentGameState == GameStatesAndData.gameStateEnum.cloudsDoneMoving)
        {
            // hide the start screen by layering it behind the background image.
            startScreen.transform.SetSiblingIndex(0);

            // make sure this block is run only once.  This makes a bit of a mini-state machine.  Does it save processing time at all?
            gameStateAndData.currentGameState = GameStatesAndData.gameStateEnum.playTime;
        }

        if (gameStateAndData.currentGameState == GameStatesAndData.gameStateEnum.endGame)
        {
            // hide the start screen by layering it behind the background image.
            endScreen.transform.SetSiblingIndex(10);

            // make sure this block is run only once.  This makes a bit of a mini-state machine.  Does it save processing time at all?
            gameStateAndData.currentGameState = GameStatesAndData.gameStateEnum.inactive;
        }

    }

    public void OnClickPlay()
    {
        /* 
         * The simplest strategy was to destroy the start screen when Play was clicked, but 
         * that means I can't bring it back in the future, like if someone wants an "instructions"
         * button or something.
         * So, I tried making the start screen a prefab, and then instantiating it in the Start()
         * method, but I couldn't set the Canvas as the parent, and some other issue with 
         * the prefab made this solution suck.
         * I finally found https://docs.unity3d.com/Manual/UICanvas.html, which mentioned:
         * transform.SetAsFirstSibling, SetAsLastSibling, and SetSiblingIndex.
        */
        //Destroy(startScreen);

        // moved to Update.
        //startScreen.transform.SetSiblingIndex(0);

        // start the little cloud animation that happens before I hide the start screen.
        gameStateAndData.currentGameState = GameStatesAndData.gameStateEnum.cloudsMoving;
        audSource.Play();

    }
}
