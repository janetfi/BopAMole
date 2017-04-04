using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* 
 * Later, I'll split out the functionality of spawning bunnies.  
 * For now, it's merged in this script with the game state and data.
 */

public class GameStatesAndData : MonoBehaviour {

    // for score keeping.
    Text shotsLeftObj;
    Text hitCountObj;
    Text levelObj;
    public int shotsGiven = 50; // how many shots the player is given when starting.
    internal int shotsLeft = 0; // keeps track of how many shots are left during play.
    internal int hitCount = 0;  // keeps track of bunny hits.

    public GameObject prefabBunny;
    public int lifeOfCloneInSecs = 7;   // making this public so it's tunable during playtesting.

    public int loopsBetweenBunnies = 100;   // this controls how fast the bunnies appear.
    int j = 80;    // setting this to 80 instead of 0 makes sure the first bunny appears in 20 FixedUpdates instead of 100.
    public int stepDown = 20;   // after every 10 hits, how much should be subtracted from loopsBetweenBunnies to speed up their appearance?
    bool bHadIncreasedDifficulty = false;   // Broadcasting via my case statement lead to a fault in logic that I'm too tired to fix right now.  At hitCount = 10, a gazillion bunnies got instantiated.

    Vector3 randomLoc;
    Camera mainCam;

    // I decided I needed a global controller of game state.
    public enum gameStateEnum
    {
        startMenu,
        cloudsMoving,
        cloudsDoneMoving,
        start,
        playTime,
        endGame,
        inactive
    };
    public gameStateEnum currentGameState;

    public enum gameLevelEnum
    {
        level1,
        level2,
        level3,
        level4
    };
    public gameLevelEnum currentGameLevel;


    // Use this for initialization
    void Start () {

        // set the game state and level.
        currentGameState = gameStateEnum.startMenu;
        currentGameLevel = gameLevelEnum.level1;

        // get references to the stuff I'll need, including elements in other game objects.
        shotsLeft = shotsGiven;
        shotsLeftObj = GameObject.Find("ShotsLeftActual").GetComponent<Text>();
        hitCountObj = GameObject.Find("HitCountActual").GetComponent<Text>();
        levelObj = GameObject.Find("LevelActual").GetComponent<Text>();
        UpdateTheHUD();

        mainCam = GameObject.FindObjectOfType<Camera>();
        if (mainCam == null) print("Cam object is empty");

        // debug.
        print("loopsBetweenBunnies = " + loopsBetweenBunnies);
        
    }

    // Using FixedUpdate to keep our bunnies appearing at regular intervals.  Not sure if this is best.
    void FixedUpdate () {

        if (currentGameState == gameStateEnum.start)
        {
            // reset score, loops, and shots.
            loopsBetweenBunnies = 100;
            shotsLeft = shotsGiven;
            hitCount = 0;
            currentGameLevel = gameLevelEnum.level1;
            UpdateTheHUD();
            currentGameState = gameStateEnum.playTime;
        }

        if (currentGameState == gameStateEnum.playTime)
        {
            if (j >= loopsBetweenBunnies)   // if we've gone through enough FixedUpdates to show another bunny...
            {
                // TODO: Hmmm.  This Screen.width thing doesn't work (to randomly place a bunny in the viewable area).  Screen.width is a constant.  
                // I think I need to project a point onto a 2D plane to get what I'm looking for.  Or get a ViewPort type object.  
                // Otherwise, these magic numbers below might position bunnies offscreen when the screen is resized.
                //randomLoc = new Vector3(
                //    Random.Range(-(Screen.width/2), Screen.width / 2),
                //    Random.Range(-(Screen.height / 2), Screen.height / 2),
                //    Random.Range(20, 40));
                // UPDATE:  Woot!  Check out Camera.ViewportToworld.  That'll work to position the bunny well.
                //randomLoc = new Vector3(
                //Random.Range(-20, 20),  // the magic numbers to which I referred above.
                //Random.Range(-10, 10),
                //Random.Range(20, 30));

                randomLoc = mainCam.ViewportToWorldPoint(new Vector3(
                    Random.Range(0.1f, 0.9f),
                    Random.Range(0.1f, 0.9f),
                    Random.Range(20, 30)
                    ));
                // debug.
                print("randomLoc = " + randomLoc);

                //randomLoc = new Vector3(0f, 0f, 0f);  // for testing.
                Destroy(Instantiate(prefabBunny, randomLoc, transform.rotation), lifeOfCloneInSecs);
                j = 0;
            }
            else
                j += 1; // ...otherwise, count off another FixedUpdate.

            UpdateTheHUD();

            // increase difficulty every 10 hits.
            // hmmm, is it better to do a state machine thing for game states, or control it via broadcasts?  Broadcasted messages are only received by scripts on this object or its children.
            // broadcasts probably shouldn't be used like this, because they get sent out multiple times unless you do the boolean thing.
            switch (hitCount)
            {
                case 10:
                case 20:
                case 30:
                case 40:
                    if (!bHadIncreasedDifficulty)
                    {
                        SpeedUp();
                        bHadIncreasedDifficulty = true;
                        currentGameLevel++;
                        UpdateTheHUD();
                        // debug.
                        print("loopsBetweenBunnies = " + loopsBetweenBunnies);
                    }
                    break;
                case 11:
                case 21:
                case 31:
                case 41:
                    bHadIncreasedDifficulty = false;
                    break;
                default:
                    break;
            }
        }

    }

    
    // helper methods.

    public void SpeedUp()
    {
            loopsBetweenBunnies -= stepDown;
            print("loopsBetweenBunnies = " + loopsBetweenBunnies);
    }

    void UpdateTheHUD()
    {
        shotsLeftObj.text = shotsLeft.ToString();
        hitCountObj.text = hitCount.ToString();
        switch (currentGameLevel)
        {
            case gameLevelEnum.level1:
                levelObj.text = "1";
                break;
            case gameLevelEnum.level2:
                levelObj.text = "2";
                break;
            case gameLevelEnum.level3:
                levelObj.text = "3";
                break;
            case gameLevelEnum.level4:
                levelObj.text = "4";
                break;
            default:
                break;
        }
    }

}
