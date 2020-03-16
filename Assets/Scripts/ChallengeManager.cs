using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChallengeManager : MonoBehaviour {

    public ChallengeInfo ChallengeData;

    public GameObject TaskUI;
    public GameObject DescriptionUI;
    public GameObject RewardUI;
    public GameObject SliderUI;

    public int[] RequiredInteger;
    public int[] Reward;
    public string[] Description;
    public bool LevelChallenge;
    public int Level;

    private int totalTasks;
    private int currentTask;

    private int value = 0;

	// Use this for initialization
	void Start ()
    {
        ChallengeData = this.GetComponent<ChallengeInfo>();
        SliderUI = SliderUI.transform.GetChild(0).gameObject;
        UpdateAll();
	}

    void UpdateAll()
    {
        if (LevelChallenge)
            value = PlayerPrefs.GetInt("Level" + Level.ToString() + "HighScore", 0);
        else
            value = PlayerPrefs.GetInt("TotalBoxesCollected", 0);

        CheckTask();
        UpdateSlider();
        UpdateTasks();
        UpdateReward();
        UpdateDescription();
    }

    void CheckTask()
    {
        foreach (int required in RequiredInteger)
        {
            if (value < required)
            {
                if (LevelChallenge)
                    currentTask = PlayerPrefs.GetInt("Level" + Level.ToString() + "Challenge", 0);
                else
                {
                    currentTask = PlayerPrefs.GetInt("TotalBoxesCollectedVersion", 0);
                }
                break;
            } else
            {
                if (LevelChallenge && PlayerPrefs.GetInt("Level" + Level.ToString() + "ChallengeAwaiting", 0) == 1 ||
                    PlayerPrefs.GetInt("Level" + Level.ToString() + "Challenge", 0) >= RequiredInteger.Length)
                {
                    currentTask = PlayerPrefs.GetInt("Level" + Level.ToString() + "Challenge", 0);
                    break;
                }
                else if (!LevelChallenge && (PlayerPrefs.GetInt("TotalBoxesCollectedAwaiting", 0) == 1 ||
                    PlayerPrefs.GetInt("TotalBoxesCollectedVersion", 0) >= RequiredInteger.Length))
                {
                    currentTask = PlayerPrefs.GetInt("TotalBoxesCollectedVersion", 0);
                    break;
                }
            }
        }
    }

    void UpdateTasks()
    {
        TaskUI.GetComponent<Text>().text = (currentTask + 1) + " / " + RequiredInteger.Length + " Tasks";
    }

    void UpdateSlider()
    {
        Color32 normalColor = new Color32(76, 255, 32, 255);
        Color32 awaitingColor = new Color32(0, 206, 255, 255);
        Color32 completedColor = new Color32(255, 221, 32, 255);

        if (currentTask >= RequiredInteger.Length)
        {
            SliderUI.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = completedColor;
            this.GetComponent<Button>().enabled = false;
            SliderUI.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "Completed";
            currentTask--;
            SliderUI.GetComponent<Slider>().maxValue = 1;
            SliderUI.GetComponent<Slider>().value = 1;
            return; 
        }
        else if (LevelChallenge && PlayerPrefs.GetInt("Level" + Level.ToString() + "ChallengeAwaiting", 0) == 1)
        {
            SliderUI.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = awaitingColor;
            this.GetComponent<Button>().enabled = true;
            SliderUI.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "Tap To Collect - (" + value + " / " + RequiredInteger[currentTask].ToString() + ")";
        }
        else if (!LevelChallenge && PlayerPrefs.GetInt("TotalBoxesCollectedAwaiting", 0) == 1)
        {
            SliderUI.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = awaitingColor;
            this.GetComponent<Button>().enabled = true;
            SliderUI.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "Tap To Collect - (" + value + " / " + RequiredInteger[currentTask].ToString() + ")";
        }
        else
        {
            SliderUI.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = normalColor;
            this.GetComponent<Button>().enabled = false;
            SliderUI.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = value + " / " + RequiredInteger[currentTask].ToString();
        }

        SliderUI.GetComponent<Slider>().maxValue = RequiredInteger[currentTask];
        SliderUI.GetComponent<Slider>().value = value;

    }

    void UpdateReward()
    {
        RewardUI.GetComponent<Text>().text = Reward[currentTask].ToString() + " boxes";
    }

    void UpdateDescription()
    {
        DescriptionUI.GetComponent<Text>().text = Description[currentTask];
    }

    public void CollectChallenge()
    {
        int boxes = PlayerPrefs.GetInt("TotalBoxCount", 0);
        boxes += Reward[currentTask];
        PlayerPrefs.SetInt("TotalBoxCount", boxes);

        if (LevelChallenge)
        {
            PlayerPrefs.SetInt("Level" + Level.ToString() + "ChallengeAwaiting", 0);

            int version = PlayerPrefs.GetInt("Level" + Level.ToString() + "Challenge", 0);
            version++;
            PlayerPrefs.SetInt("Level" + Level.ToString() + "Challenge", version);
            Debug.Log(version);

            if (version < RequiredInteger.Length)
            {

                if (PlayerPrefs.GetInt("Level" + Level.ToString() + "HighScore", 0) >= RequiredInteger[version])
                {
                    PlayerPrefs.SetInt("Level" + Level.ToString() + "ChallengeAwaiting", 1);
                }
            }
        } else
        {
            PlayerPrefs.SetInt("TotalBoxesCollectedAwaiting", 0);

            int version = PlayerPrefs.GetInt("TotalBoxesCollectedVersion", 0);
            version++;
            PlayerPrefs.SetInt("TotalBoxesCollectedVersion", version);

            if (version < RequiredInteger.Length)
            {
                if (PlayerPrefs.GetInt("TotalBoxesCollected", 0) >= RequiredInteger[version])
                {
                    PlayerPrefs.SetInt("TotalBoxesCollectedAwaiting", 1);
                }
            }
        }

        UpdateAll();
    }
}
