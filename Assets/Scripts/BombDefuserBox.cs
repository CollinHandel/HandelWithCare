using UnityEngine;
using System.Collections;

public class BombDefuserBox : MonoBehaviour
{

    public GameObject GameManager;
    public int Direction;
    public float MaxY;

    float boxSpeed = 7.5f;
    int difficulty = 0;

    // Use this for initialization
    void Start()
    {
        GameManager = GameObject.Find("GameManager");

        difficulty = PlayerPrefs.GetInt("DifficultyLevel", 0);

        if (GameManager.GetComponent<Spawners>().counter >= GameManager.GetComponent<Spawners>().speedAmpNumber)
        {
            boxSpeed = 7.5f;
        }
        else if (difficulty == 1 && GameManager.GetComponent<Spawners>().counter >= 100)
        {
            boxSpeed = 8.5f;
        }
        else if (difficulty == 2 && GameManager.GetComponent<Spawners>().counter >= 60)
        {
            boxSpeed = 8.5f;
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
            }
            else if (this.Direction == 1)
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
        Destroy(this.gameObject);
    }
}
