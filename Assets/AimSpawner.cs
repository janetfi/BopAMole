using UnityEngine;
using System.Collections;

public class AimSpawner : MonoBehaviour {

    public GameObject prefabCrate;
    public int lifeOfCloneInSecs;

    GameStatesAndData gameStateAndData;  // used to update the shotsLeft property.

    Camera ourCamera;   // used to get a ray via the getAimRay method.
    Ray aimRay;
    Quaternion aimRotation;


    // Use this for initialization
    void Start () {
        ourCamera = GetComponent<Camera>();
        gameStateAndData = GameObject.Find("StateObject").GetComponent<GameStatesAndData>();
    }

    // Update is called once per frame
    void Update () {

        if ( (Input.GetMouseButtonDown(0)) && (gameStateAndData.currentGameState == GameStatesAndData.gameStateEnum.playTime) )
        {
            // get our aimRay.
            aimRay = ourCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(aimRay.origin, aimRay.direction, Color.white);

            aimRotation = Quaternion.LookRotation(aimRay.direction);    // 
            Destroy(Instantiate(prefabCrate, transform.position, aimRotation), lifeOfCloneInSecs);

            gameStateAndData.shotsLeft -= 1;    // reduce the leftover shots.
            
            if (gameStateAndData.shotsLeft < 0)
            {
                print("setting endGame");
                gameStateAndData.currentGameState = GameStatesAndData.gameStateEnum.endGame;

            }
        }

    }

}
