using UnityEngine;
using System.Collections;

public class MoveProjectile : MonoBehaviour {

    Rigidbody rb;
    bool bHasFired;
    public float force = 10f;
    public float stepIncrease = 2f;
    AudioSource audSource;

    Vector3[] whichSpin = new[] { // used to put a spin on the crate.
        new Vector3(5f, 2f, 0f),
        new Vector3(0f, 5f, 2f),
        new Vector3(2f, 0f, 5f),
        new Vector3(0f, 2f, 5f),
        new Vector3(5f, 0f, 2f),
        new Vector3(2f, 5f, 0f)};
    float randSpin;

    // Use this for initialization
    void Start () {

        rb = GetComponent<Rigidbody>();
        bHasFired = false;
        randSpin = Random.Range(1f, (float)whichSpin.Length);
        audSource = GetComponent<AudioSource>();
        audSource.Play();

    }

    // Update is called once per frame
    void Update () {
        if (!bHasFired)
        {
            rb.angularVelocity = whichSpin[(int)randSpin];   // the order might matter here with these two rb.* calls.
            rb.AddForce(transform.forward * force, ForceMode.Impulse);
            randSpin = Random.Range(1f, (float)whichSpin.Length);   // OMG, in version 1, I forgot to update randSpin with a new member of the array.
            bHasFired = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // the bunny already has a destruction time set when it's created, but that time is meant to remove the bunny if it doesn't get shot in time.
        // so, this destruction timeout is set to match when the crate gets destroyed, after collision.
        Destroy(collision.gameObject, 1.2f);
    }

}
