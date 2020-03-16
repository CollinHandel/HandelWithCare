using UnityEngine;
using System.Collections;

public class GoldBox : MonoBehaviour {

    public GameObject GameManager;
    
    public int Score;
    public int Direction;
    public float MaxY;

    float boxSpeed = 5;
    int difficulty = 0;

    // Use this for initialization
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
        Score = Random.Range(PlayerPrefs.GetInt("Upgrade1MinValue"), PlayerPrefs.GetInt("Upgrade1MaxValue"));

        difficulty = PlayerPrefs.GetInt("DifficultyLevel", 0);

        if (GameManager.GetComponent<Spawners>().counter >= GameManager.GetComponent<Spawners>().speedAmpNumber)
        {
            boxSpeed = 7.5f;
        }
        else if (difficulty == 1 && GameManager.GetComponent<Spawners>().counter >= 100)
        {
            boxSpeed = 6;
        }
        else if (difficulty == 2 && GameManager.GetComponent<Spawners>().counter >= 60)
        {
            boxSpeed = 6;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.GetComponent<Spawners>().gameOver)
        {
            if (this.Direction == 0)
            {
                transform.Translate(Vector3.down * Time.deltaTime * boxSpeed);

                if (this.transform.position.y < 0)
                {
                    Dead();
                }
            } else if (this.Direction == 1)
            {
                transform.Translate(Vector3.up * Time.deltaTime * boxSpeed);

                if (this.transform.position.y > this.MaxY)
                {
                    Dead();
                }
            }
        }
    }

    void Dead()
    {
        GameManager.GetComponent<Spawners>().GameOver();
        Destroy(this.gameObject);
    }
}
