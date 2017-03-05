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
    public int shotsLeft = 50;
    public int hitCount = 0;

    public GameObject prefabBunny;
    int lifeOfCloneInSecs = 7;

    public int loopsBetweenBunnies = 100;   // this controls how fast the bunnies appear.
    public int j = 80;    // setting this to 150 instead of 0 makes sure the first bunny appears in 50 FixedUpdates.
    public int stepDown = 20;   // at each 10 hits, how much should be subtracted from loopsBetweenBunnies?
    bool bHadIncreasedDifficulty = false;   // Broadcasting via my case statement lead to a fault in logic that I'm too tired to fix right now.  At hitCount = 10, a gazillion bunnies got instantiated.

    Vector3 randomLoc;

    // I decided I needed a global controller of game state.
    public enum gameStateEnum
    {
        startMenu,
        cloudsMoving,
        cloudsDoneMoving,
        playTime,
        endGame,
        inactive
    };
    public gameStateEnum currentGameState;


	// Use this for initialization
	void Start () {

        currentGameState = gameStateEnum.startMenu;
        shotsLeftObj = GameObject.Find("ShotsLeftActual").GetComponent<Text>();
        hitCountObj = GameObject.Find("HitCountActual").GetComponent<Text>();
        shotsLeftObj.text = shotsLeft.ToString();
        hitCountObj.text = hitCount.ToString();
        print("loopsBetweenBunnies = " + loopsBetweenBunnies);

    }

    // Using FixedUpdate to keep our bunnies appearing at regular intervals.  Not sure if this is best.
    void FixedUpdate () {

        if (currentGameState == gameStateEnum.playTime)
        {
            if (j >= loopsBetweenBunnies)
            {
                // Hmmm.  This Screen.width thing doesn't work.  It's a constant.  Plus, I think I need to project a point onto a 2D plane to get what I'm looking for.  Or get a ViewPort type object.
                //randomLoc = new Vector3(
                //    Random.Range(-(Screen.width/2), Screen.width / 2),
                //    Random.Range(-(Screen.height / 2), Screen.height / 2),
                //    Random.Range(20, 40));
                randomLoc = new Vector3(
                Random.Range(-20, 20),
                Random.Range(-10, 10),
                Random.Range(20, 30));

                //randomLoc = new Vector3(0f, 0f, 0f);
                Destroy(Instantiate(prefabBunny, randomLoc, transform.rotation), lifeOfCloneInSecs);
                j = 0;
            }
            else
                j += 1;

            // update the HUD.
            shotsLeftObj.text = shotsLeft.ToString();
            hitCountObj.text = hitCount.ToString();

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
                        gameObject.BroadcastMessage("SpeedUp");   // holy crap!  Just like in Scratch, there's a Broadcast method!!  Yay!
                        bHadIncreasedDifficulty = true;
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

    public void SpeedUp()
    {
            loopsBetweenBunnies -= stepDown;
            print("loopsBetweenBunnies = " + loopsBetweenBunnies);
    }

}
