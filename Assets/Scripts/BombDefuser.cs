using UnityEngine;
using System.Collections;

public class BombDefuser : MonoBehaviour {

    public Transform FallingBackground;
    public GameObject GameManager;
    private bool defused = false;

    private float timeStartedLerping;

    private Vector3 startingPosition = new Vector3(0, 0, 0);
    private Vector3 endingPosition = new Vector3(0, -50f, 0);

    float time;

	// Use this for initialization
	void Start () {
        GameManager = GameObject.Find("GameManager");
        defused = GameManager.GetComponent<Spawners>().BombDefuser;
        time = PlayerPrefs.GetInt("Upgrade2Timer", 5);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!GameManager.GetComponent<Spawners>().gameOver)
        {
            if (FallingBackground.localPosition.y > -50 && defused)
            {
                float timeSinceStarted = Time.time - timeStartedLerping;
                float percentageComplete = timeSinceStarted / time;
                FallingBackground.localPosition = Vector3.Lerp(startingPosition, endingPosition, percentageComplete);
            }
            else
            {
                this.gameObject.SetActive(false);
                FallingBackground.localPosition = new Vector3(0, 0, 0);
                GameManager.GetComponent<Spawners>().BombDefuser = false;
            }
        }
	}

    public void ActivateDefuser()
    {
        defused = true;
        timeStartedLerping = Time.time;
        this.gameObject.SetActive(true);
    }
}
