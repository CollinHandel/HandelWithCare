using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LostMenuScript : MonoBehaviour {

    GameObject GameManager;

    void Start()
    {
        GameManager = GameObject.Find("GameManager");
    }

	public void Retry()
    {
        SceneManager.LoadScene(GameManager.GetComponent<Spawners>().LevelNum);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
