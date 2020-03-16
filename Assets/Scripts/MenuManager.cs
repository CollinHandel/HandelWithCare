using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class MenuManager : MonoBehaviour {

    public Transform HighScore;
    public GameObject ShopBox;
    public GameObject LevelBox;
    public GameObject ChallengeBox;
    public GameObject ErrorBox;
    public GameObject DailyBonus;
    public GameObject DifSlider;
    private int[] highScoreRequired = new int[3] { 0, 100, 300 };

    public GameObject[] LevelOptions;

    Ray ray;
    RaycastHit2D hit;

    private int activeLevel = 0;

    // Use this for initialization
    void Start ()
    {
        PlayGamesPlatform.Activate();

        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    CheckFirstTimeAchievement();

                    if (System.DateTime.Parse(PlayerPrefs.GetString("LastLoginDate", System.DateTime.MinValue.ToString())) != System.DateTime.Now.Date)
                    {
                        OpenDailyBonus();
                    }
                }
                else
                    ErrorBox.SetActive(true);
            });
        }

        DifSlider.GetComponent<Slider>().value = PlayerPrefs.GetInt("DifficultyLevel", 0);

        if (PlayerPrefs.GetString("AdViewDate") != System.DateTime.Now.ToString("yyyyMMdd"))
            PlayerPrefs.SetInt("AvailableAds", 5);

        if (PlayerPrefs.GetInt("TotalBoxCount") < 0)
        {
            PlayerPrefs.SetInt("TotalBoxCount", 0);
        }

        PlayerPrefs.SetInt("Background0Bought", 1);
        PlayerPrefs.SetInt("Skin0Bought", 1);

        int counter = 0;
        foreach (GameObject level in LevelOptions)
        {
            if (PlayerPrefs.GetInt("Level" + counter.ToString() + "HighScore") >= highScoreRequired[counter])
            {
                level.transform.GetChild(3).gameObject.SetActive(true);
                level.transform.GetChild(4).gameObject.SetActive(true);
                level.transform.GetChild(5).gameObject.SetActive(true);
                level.transform.GetChild(6).gameObject.SetActive(false);
                level.transform.GetChild(4).GetComponent<Text>().text = PlayerPrefs.GetInt("Level" + (counter + 1).ToString() + "HighScore", 0).ToString();
            } else
            {
                level.transform.GetChild(6).GetChild(1).GetComponent<Text>().text = highScoreRequired[counter].ToString() + " minimum high score required on previous level";
            }

            counter++;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (LevelBox.activeSelf)
            {
                LevelBox.SetActive(false);
            } else if (ShopBox.activeSelf)
            {
                ShopBox.SetActive(false);
            }
        }
    }

    void CheckFirstTimeAchievement()
    {

        if (Social.localUser.authenticated)
        {
            Social.ReportProgress("CgkIvbbbuOQaEAIQAA", 100.0f, (bool success) =>
            {

            });
        }
    }

    public void StartGame()
    {
        LevelBox.SetActive(true);
    }

    public void LoadLevel(int Level)
    {
        if (activeLevel == Level)
            SceneManager.LoadScene(Level);
        else
        {
            if (PlayerPrefs.GetInt("Level" + (Level - 1).ToString() + "HighScore") >= highScoreRequired[Level - 1] || Level == 1)
            {
                if (activeLevel > 0)
                {
                    GameObject.Find("Level " + activeLevel).transform.GetChild(7).gameObject.SetActive(false);
                }

                activeLevel = Level;
                GameObject.Find("Level " + Level).transform.GetChild(7).gameObject.SetActive(true);
            }
        }
    }

    public void DifficultySlider(GameObject difText)
    {
        int value = (int)DifSlider.GetComponent<Slider>().value;

        if (value == 0)
            difText.GetComponent<Text>().text = "Beginner";
        else if (value == 1)
            difText.GetComponent<Text>().text = "Experienced";
        else if (value == 2)
            difText.GetComponent<Text>().text = "Pro";
        else
        {
            difText.GetComponent<Text>().text = "Beginner";
            value = 0;
        }

        PlayerPrefs.SetInt("DifficultyLevel", value);
    }

    public void ResetActiveLevel()
    {
        if (activeLevel > 0)
        {
            GameObject.Find("Level " + activeLevel).transform.GetChild(7).gameObject.SetActive(false);
            activeLevel = 0;
        }
    }

    public void CloseLevelPanel()
    {
        LevelBox.SetActive(false);
    }

    public void OpenShop()
    {
        ShopBox.GetComponent<ShopManager>().UpdateShop();
        ShopBox.SetActive(true);
    }

    public void CloseShop()
    {
        ShopBox.SetActive(false);
    }

    public void OpenChallengePanel()
    {
        ChallengeBox.SetActive(true);
    }

    public void CloseChallengePanel()
    {
        ChallengeBox.SetActive(false);
    }

    public void OpenDailyBonus()
    {
        //if (Social.localUser.authenticated)
        //{
            DailyBonus.GetComponent<DailyBonus>().CheckDayBonus();
        //}
        //else
        //{
        //    ErrorBox.SetActive(true);
        //}
    }

    public void CloseDailyBonus()
    {
        DailyBonus.GetComponent<Canvas>().enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AchievementButton()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
        else
        {
            ErrorBox.SetActive(true);
        }
    }

    public void LeaderboardButton()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            ErrorBox.SetActive(true);
        }
    }



    public void ShowRewardedAd()
    {
        if (PlayerPrefs.GetInt("AvailableAds") >= 1)
        {
            if (Advertisement.IsReady("rewardedVideo"))
            {
                var options = new ShowOptions { resultCallback = HandleShowResult };
                Advertisement.Show("rewardedVideo", options);
            }
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");

                if (PlayerPrefs.GetInt("AvailableAds") >= 1)
                {
                    int boxes = PlayerPrefs.GetInt("TotalBoxCount", 0) + 500;
                    PlayerPrefs.SetInt("TotalBoxCount", boxes);

                    PlayerPrefs.SetString("AdViewDate", System.DateTime.Now.ToString("yyyyMMdd"));
                    int views = PlayerPrefs.GetInt("AvailableAds", 5) - 1;
                    PlayerPrefs.SetInt("AvailableAds", views);

                    ShopBox.GetComponent<ShopManager>().CheckAvailableAds();
                    ShopBox.GetComponent<ShopManager>().UpdateShop();
                }

                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}