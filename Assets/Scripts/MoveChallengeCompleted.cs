using UnityEngine;
using System.Collections;

public class MoveChallengeCompleted : MonoBehaviour {

    public int counter = 0;
    private bool move = false;
    private float timer = 0;

	// Use this for initialization
	void Start () {
        if (this.name == "ChallengeCompleted(Clone)")
            move = true;
	}
	
	// Update is called once per frame
	void Update () {
	    if (move)
        {
            if (counter >= 1)
            {
                if (timer > (5 * counter))
                    transform.Translate(Vector3.left * 2 * Time.deltaTime);
            }
            else {
                transform.Translate(Vector3.left * 2 * Time.deltaTime);
            }
        }

        timer += Time.deltaTime;
	}
}
