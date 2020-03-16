using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class ShopManager : MonoBehaviour {

    public GameObject BoxCountText;
    public GameObject[] Items;
    public GameObject GoldBoxValueUpgrade;

    public int BackgroundSelected
    {
        get
        {
            return _backgroundSelected;
        }
        set
        {
            if (value < 0)
                _backgroundSelected = 0;
            else
                _backgroundSelected = value;
        }
    }
    private int _backgroundSelected;

    public int SkinSelected
    {
        get
        {
            return _skinSelected;
        }
        set
        {
            if (value < 0)
                _skinSelected = 0;
            else
                _skinSelected = value;
        }
    }
    private int _skinSelected;

    void Start()
    {
        CheckAvailableAds();
        CheckAchievementStatuses();

        if (PlayerPrefs.GetInt("TotalBoxCount") < 0)
        {
            PlayerPrefs.SetInt("TotalBoxCount", 0);
        }

        CheckIfUpgradeable();
    }

	public void UpdateShop()
    {
        CheckIfUpgradeable();
        CheckAchievementStatuses();

        BoxCountText.GetComponent<Text>().text = PlayerPrefs.GetInt("TotalBoxCount").ToString();
        this.BackgroundSelected = PlayerPrefs.GetInt("BackgroundSelected");
        this.SkinSelected = PlayerPrefs.GetInt("SkinSelected");

        CheckPurchasableItems();
    }

    public void CheckAvailableAds()
    {
        GameObject go = GameObject.Find("Ad Reward").transform.GetChild(3).gameObject;

        int views = PlayerPrefs.GetInt("AvailableAds", 5);

        go.transform.GetChild(0).GetComponent<Text>().text = "Watch Ad - " + views + " views left today";

        if (PlayerPrefs.GetInt("AvailableAds", 5) <= 0)
            go.GetComponent<Button>().interactable = false;
    }

    private void CheckIfUpgradeable()
    {
        if (PlayerPrefs.GetInt("Upgrade0Bought") >= 1)
        {
            GoldBoxValueUpgrade.SetActive(true);
        }
    }

    private void CheckPurchasableItems()
    {
        foreach (GameObject item in Items)
        {
            int Number = item.GetComponent<ShopItem>().Number;
            ShopItem ShopItem = item.GetComponent<ShopItem>();

            if (!ShopItem.Upgrade && !ShopItem.Skin)
            {
                if (PlayerPrefs.GetInt("Background" + Number.ToString() + "Bought") == 1 &&
                    PlayerPrefs.GetInt("BackgroundSelected") == Number)
                {
                    ShopItem.LockedBtn.SetActive(false);
                    ShopItem.BuyBtn.SetActive(false);
                    ShopItem.MakeActiveBtn.SetActive(false);
                    ShopItem.SelectedBtn.SetActive(true);
                }
                else if (PlayerPrefs.GetInt("Background" + Number.ToString() + "Bought") == 1 &&
                        PlayerPrefs.GetInt("BackgroundSelected") != Number)
                {
                    ShopItem.LockedBtn.SetActive(false);
                    ShopItem.BuyBtn.SetActive(false);
                    ShopItem.MakeActiveBtn.SetActive(true);
                    ShopItem.SelectedBtn.SetActive(false);
                } else
                {
                    if (PlayerPrefs.GetInt("TotalBoxCount") >= ShopItem.Cost[0])
                    {
                        ShopItem.LockedBtn.SetActive(false);
                        ShopItem.BuyBtn.SetActive(true);
                        ShopItem.MakeActiveBtn.SetActive(false);
                        ShopItem.SelectedBtn.SetActive(false);
                    } else
                    {
                        ShopItem.LockedBtn.SetActive(true);
                        ShopItem.BuyBtn.SetActive(false);
                        ShopItem.MakeActiveBtn.SetActive(false);
                        ShopItem.SelectedBtn.SetActive(false);
                    }
                }
            }

            if (!ShopItem.Upgrade && !ShopItem.Background)
            {
                if (PlayerPrefs.GetInt("Skin" + Number.ToString() + "Bought") == 1 &&
                    PlayerPrefs.GetInt("SkinSelected") == Number)
                {
                    ShopItem.LockedBtn.SetActive(false);
                    ShopItem.BuyBtn.SetActive(false);
                    ShopItem.MakeActiveBtn.SetActive(false);
                    ShopItem.SelectedBtn.SetActive(true);
                }
                else if (PlayerPrefs.GetInt("Skin" + Number.ToString() + "Bought") == 1 &&
                        PlayerPrefs.GetInt("SkinSelected") != Number)
                {
                    ShopItem.LockedBtn.SetActive(false);
                    ShopItem.BuyBtn.SetActive(false);
                    ShopItem.MakeActiveBtn.SetActive(true);
                    ShopItem.SelectedBtn.SetActive(false);
                }
                else
                {
                    if (PlayerPrefs.GetInt("TotalBoxCount") >= ShopItem.Cost[0])
                    {
                        ShopItem.LockedBtn.SetActive(false);
                        ShopItem.BuyBtn.SetActive(true);
                        ShopItem.MakeActiveBtn.SetActive(false);
                        ShopItem.SelectedBtn.SetActive(false);
                    }
                    else
                    {
                        ShopItem.LockedBtn.SetActive(true);
                        ShopItem.BuyBtn.SetActive(false);
                        ShopItem.MakeActiveBtn.SetActive(false);
                        ShopItem.SelectedBtn.SetActive(false);
                    }
                }
            }

            if (ShopItem.Upgrade)
            {
                if (item.name == "Upgrade Shop Item 0")
                {
                    if (ShopItem.CheckVersion(0) == ShopItem.Cost.Length)
                    {
                        ShopItem.LockedBtn.SetActive(false);
                        ShopItem.BuyBtn.SetActive(false);
                        ShopItem.BoughtBtn.SetActive(true);
                    }
                    else {
                        if (PlayerPrefs.GetInt("TotalBoxCount") >= ShopItem.Cost[ShopItem.CheckVersion(0)])
                        {
                            ShopItem.LockedBtn.SetActive(false);
                            ShopItem.BuyBtn.SetActive(true);
                            ShopItem.BoughtBtn.SetActive(false);
                        } else
                        {
                            ShopItem.LockedBtn.SetActive(true);
                            ShopItem.BuyBtn.SetActive(false);
                            ShopItem.BoughtBtn.SetActive(false);
                        }
                    }
                }
                else if (item.name == "Upgrade Shop Item 1")
                {
                    if (ShopItem.CheckVersion(1) == ShopItem.Cost.Length)
                    {
                        ShopItem.LockedBtn.SetActive(false);
                        ShopItem.BuyBtn.SetActive(false);
                        ShopItem.BoughtBtn.SetActive(true);
                    }
                    else {
                        if (PlayerPrefs.GetInt("TotalBoxCount") >= ShopItem.Cost[ShopItem.CheckVersion(1)])
                        {
                            ShopItem.LockedBtn.SetActive(false);
                            ShopItem.BuyBtn.SetActive(true);
                            ShopItem.BoughtBtn.SetActive(false);
                        }
                        else
                        {
                            ShopItem.LockedBtn.SetActive(true);
                            ShopItem.BuyBtn.SetActive(false);
                            ShopItem.BoughtBtn.SetActive(false);
                        }
                    }
                }
                else if (item.name == "Upgrade Shop Item 2")
                {
                    if (ShopItem.CheckVersion(2) == ShopItem.Cost.Length)
                    {
                        ShopItem.LockedBtn.SetActive(false);
                        ShopItem.BuyBtn.SetActive(false);
                        ShopItem.BoughtBtn.SetActive(true);
                    }
                    else {
                        if (PlayerPrefs.GetInt("TotalBoxCount") >= ShopItem.Cost[ShopItem.CheckVersion(2)])
                        {
                            ShopItem.LockedBtn.SetActive(false);
                            ShopItem.BuyBtn.SetActive(true);
                            ShopItem.BoughtBtn.SetActive(false);
                        }
                        else
                        {
                            ShopItem.LockedBtn.SetActive(true);
                            ShopItem.BuyBtn.SetActive(false);
                            ShopItem.BoughtBtn.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    void CheckAchievementStatuses()
    {
        if (Social.localUser.authenticated)
        {
            // Check for Sightseer and World Traveler Achievements
            int count = 0;
            int numberOfFlags = 0;
            int flagStarts = Items.Length;

            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].GetComponent<ShopItem>().FlagBackground == true)
                {
                    numberOfFlags++;

                    if (i < flagStarts)
                    {
                        flagStarts = i;
                    }
                }
            }

            for (int i = flagStarts; i <= Items.Length; i++)
            {
                if (PlayerPrefs.GetInt("Background" + i.ToString() + "Bought", 0) == 1)
                {
                    Social.ReportProgress("CgkIvbbbuOQaEAIQAw", 100.0f, (bool success) =>
                    {

                    });
                    
                    count++;
                }
            }


            // Check if all flags are purchased for World Traveler achievement.
            if (count >= numberOfFlags)
            {
                Social.ReportProgress("CgkIvbbbuOQaEAIQBw", 100.0f, (bool success) =>
                {

                });
            }


            // Check for Look Mom, I Did It! Achievement
            if (PlayerPrefs.GetInt("TotalBoxCount", 0) >= 10)
            {
                Social.ReportProgress("CgkIvbbbuOQaEAIQBA", 100.0f, (bool success) =>
                {

                });
            }


            // Check for 100000 BOXES achievement
            if (PlayerPrefs.GetInt("TotalBoxCount", 0) >= 100000)
            {
                Social.ReportProgress("CgkIvbbbuOQaEAIQAg", 100.0f, (bool success) =>
                {

                });
            }

            CheckUpgradeAchievements();


            // Post On Hand Box Count to Leaderboard
            Social.ReportScore(PlayerPrefs.GetInt("TotalBoxCount", 0), "CgkIvbbbuOQaEAIQCg", (bool success) =>
            {

            });
        }
    }

    void CheckUpgradeAchievements()
    {

        // CHeck for I FEEL THE POWER! achievement
        UpgradeInfo UpgradeInfo = GetComponent<UpgradeInfo>();
        if (UpgradeInfo.GoldBoxRate.Length > PlayerPrefs.GetInt("Upgrade0Bought", 0))
        {
            return;
        }

        if (UpgradeInfo.GoldBoxMinValue.Length > PlayerPrefs.GetInt("Upgrade1Value", 0))
        {
            return;
        }

        if (UpgradeInfo.BombDefuserTimer.Length > PlayerPrefs.GetInt("Upgrade2Bought", 0))
        {
            return;
        }


        Social.ReportProgress("CgkIvbbbuOQaEAIQAQ", 100.0f, (bool success) =>
        {

        });
    }
}
