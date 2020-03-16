using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyBonus : MonoBehaviour {

    int dailyBonusNumber = 0;

    public GameObject[] Days;
    public GameObject BoxBonusTxt;

    public Sprite[] DaysActive;

    public void CheckDayBonus()
    {
        dailyBonusNumber = (int)(System.DateTime.Now.Date - System.DateTime.Parse(PlayerPrefs.GetString("DailyBonusStart", System.DateTime.MinValue.ToString()))).TotalDays;
        int counter = PlayerPrefs.GetInt("DailyBonus", 0);

        if (counter > 5)
            counter = 5;

        if ((System.DateTime.Now.Date - System.DateTime.Parse(PlayerPrefs.GetString("LastLoginDate", System.DateTime.MinValue.ToString()))).TotalDays <= 1)
        {

            if (dailyBonusNumber != counter)
            {
                Debug.Log("New Day of Daily Bonus");
                counter = dailyBonusNumber;
                PlayerPrefs.SetInt("DailyBonus", counter);
                PlayerPrefs.SetString("LastLoginDate", System.DateTime.Now.Date.ToString());
                DistributeBoxes(counter);
            }
        } else
        {
            Debug.Log("Reset Daily Bonus");
            ResetDailyBonus();
        }

        if (counter > 0)
            CheckActiveDays(counter);

        Debug.Log("Show Daily Bonus");
        this.GetComponent<Canvas>().enabled = true;
    }

    void ResetDailyBonus()
    {
        PlayerPrefs.SetInt("DailyBonus", 0);
        PlayerPrefs.SetString("DailyBonusStart", System.DateTime.Now.Date.ToString());
        PlayerPrefs.SetString("LastLoginDate", System.DateTime.Now.Date.ToString());
    }

    void CheckActiveDays(int days)
    {
        for (int i = 0; i < days; i++)
        {
            Days[i].GetComponent<Image>().sprite = DaysActive[i];
        }
    }

    void DistributeBoxes(int days)
    {
        if (days <= 0)
        {
            return;
        }

        int totalBoxes = PlayerPrefs.GetInt("TotalBoxesCollected", 0);

        if (days == 1)
        {
            Debug.Log(100);

            totalBoxes += 100;
            PlayerPrefs.SetInt("TotalBoxesCollected", totalBoxes);

            BoxBonusTxt.GetComponent<Text>().text = "+100";
            BoxBonusTxt.SetActive(true);
        }
        else if (days == 2)
        {
            Debug.Log(200);

            totalBoxes += 200;
            PlayerPrefs.SetInt("TotalBoxesCollected", totalBoxes);

            BoxBonusTxt.GetComponent<Text>().text = "+200";
            BoxBonusTxt.SetActive(true);
        }
        else if (days == 3)
        {
            Debug.Log(300);

            totalBoxes += 300;
            PlayerPrefs.SetInt("TotalBoxesCollected", totalBoxes);

            BoxBonusTxt.GetComponent<Text>().text = "+300";
            BoxBonusTxt.SetActive(true);
        }
        else if (days == 4)
        {
            Debug.Log(400);

            totalBoxes += 400;
            PlayerPrefs.SetInt("TotalBoxesCollected", totalBoxes);

            BoxBonusTxt.GetComponent<Text>().text = "+400";
            BoxBonusTxt.SetActive(true);
        }
        else if (days == 5)
        {
            Debug.Log(900);

            totalBoxes += 900;
            PlayerPrefs.SetInt("TotalBoxesCollected", totalBoxes);

            BoxBonusTxt.GetComponent<Text>().text = "+900";
            BoxBonusTxt.SetActive(true);
        }
    }
}
