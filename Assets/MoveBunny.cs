using UnityEngine;
using System.Collections;

public class MoveBunny : MonoBehaviour {

    Animator anim;
    int loopMax = 200;
    int j = 180;
    bool isDying = false;
    GameStatesAndData gameStateAndData;  // used to update the shotsLeft property.
    AudioSource audSource;


    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        if (null == anim)
            print("anim is null");
        gameStateAndData = GameObject.Find("StateObject").GetComponent<GameStatesAndData>();
        audSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        // the Animator component doesn't have a way to tell if there's an animation currently playing.
        // so instead, I'm counting time.  :(
        if (j >= loopMax)
        {
            if (!isDying)
            {
                anim.Play("Idle");
            }
            j = 0;
        }
        else
            j += 1;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!isDying)
        {
            gameStateAndData.hitCount += 1;
            isDying = true;     // the OnCollisionEnter seemed to run twice per hit.
            Destroy(collision.gameObject, 1.2f);
            anim.Play("Death");
            audSource.Play();
        }   //todo: look up screen dimentions.
    }

    public void StartSinking ()
    {

    }

}
