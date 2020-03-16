using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour {
    public int Number;
    public int[] Cost;
    public string[] ExtraInfo;

    public ShopManager ShopManager;
    public GameObject LockedBtn;
    public GameObject BuyBtn;
    public GameObject MakeActiveBtn;
    public GameObject SelectedBtn;
    public GameObject BoughtBtn;
    public GameObject ExtraInfoText;
    private Text Text;

    public bool Background = false;
    public bool FlagBackground = false;
    public bool Upgrade = false;
    public bool Skin = false;

    UpgradeInfo UpgradeInfo;

    void Start()
    {
        ShopManager = GameObject.Find("Shop Panel").GetComponent<ShopManager>();
        UpgradeInfo = ShopManager.GetComponent<UpgradeInfo>();

        if (Background)
            this.name = "Background Shop Item " + Number.ToString();
        else if (Skin)
            this.name = "Skin Shop Item " + Number.ToString();
        else if (Upgrade)
            this.name = "Upgrade Shop Item " + Number.ToString();

        CheckInfo();

        if (Upgrade)
        {
            if (CheckVersion(Number) == this.Cost.Length)
                ExtraInfoText.GetComponent<Text>().text = ExtraInfo[CheckVersion(Number) - 1];
            else
                ExtraInfoText.GetComponent<Text>().text = ExtraInfo[CheckVersion(Number)];
        }
    }

    void CheckInfo()
    {
        string PrefInfo = "";
        if (Background)
        {
            PrefInfo = "Background" + Number.ToString() + "Bought";
        }
        else if (Upgrade)
        {
            PrefInfo = "Upgrade" + Number.ToString() + "Bought";
            if (CheckVersion(Number) == this.Cost.Length)
                ExtraInfoText.GetComponent<Text>().text = this.Cost[CheckVersion(Number) - 1].ToString() + " - Boxes";
            else
                ExtraInfoText.GetComponent<Text>().text = this.Cost[CheckVersion(Number)].ToString() + " - Boxes";
        }
        else if (Skin)
        {
            PrefInfo = "Skin" + Number.ToString() + "Bought";
        }

        if (PlayerPrefs.GetInt(PrefInfo) == 1 && !Upgrade && !Skin &&
            PlayerPrefs.GetInt("BackgroundSelected") == Number)
        {
            SelectedBtn.SetActive(true);
        }
        else if (PlayerPrefs.GetInt(PrefInfo) == 1 && !Upgrade && !Background &&
            PlayerPrefs.GetInt("SkinSelected") == Number)
        {
            SelectedBtn.SetActive(true);
        }
        else if (PlayerPrefs.GetInt(PrefInfo) == this.Cost.Length &&
          (PlayerPrefs.GetInt("BackgroundSelected") != Number || PlayerPrefs.GetInt("SkinSelected") != Number || Upgrade))
        {
            if (Upgrade)
                BoughtBtn.SetActive(true);
            else if (Background || Skin)
                MakeActiveBtn.SetActive(true);
        }
        else
        {
            if (PlayerPrefs.GetInt("TotalBoxCount") >= this.Cost[CheckVersion(Number)])
            {
                BuyBtn.SetActive(true);
            }
            else
            {
                LockedBtn.SetActive(true);
            }
            Text = BuyBtn.transform.GetChild(0).GetComponent<Text>();
            Text.text = this.Cost[CheckVersion(Number)].ToString() + " - Boxes";

            Text = LockedBtn.transform.GetChild(0).GetComponent<Text>();
            Text.text = this.Cost[CheckVersion(Number)].ToString() + " - Boxes";
        }
    }

    public int CheckVersion(int Checking)
    {
        if (Background || Skin)
            return 0;
        else if (Checking == 0)
            return PlayerPrefs.GetInt("Upgrade" + Number.ToString() + "Bought", 0);
        else if (Checking == 1)
            return PlayerPrefs.GetInt("Upgrade" + Number.ToString() + "Value", 0);
        else if (Checking == 2)
            return PlayerPrefs.GetInt("Upgrade" + Number.ToString() + "Bought", 0);

        return 0;
    }

    public void MakeActive()
    {
        if (Background)
        {
            GameObject.Find("Background Shop Item " + PlayerPrefs.GetInt("BackgroundSelected")).transform.GetChild(6).gameObject.SetActive(false);
            GameObject.Find("Background Shop Item " + PlayerPrefs.GetInt("BackgroundSelected")).transform.GetChild(5).gameObject.SetActive(true);
            PlayerPrefs.SetInt("BackgroundSelected", Number);
        }
        else if (Skin)
        {
            GameObject.Find("Skin Shop Item " + PlayerPrefs.GetInt("SkinSelected")).transform.GetChild(6).gameObject.SetActive(false);
            GameObject.Find("Skin Shop Item " + PlayerPrefs.GetInt("SkinSelected")).transform.GetChild(5).gameObject.SetActive(true);
            PlayerPrefs.SetInt("SkinSelected", Number);
        }
        MakeActiveBtn.SetActive(false);
        SelectedBtn.SetActive(true);
    }

    public void BuyBackground()
    {
        if (PlayerPrefs.GetInt("TotalBoxCount") >= this.Cost[0])
        {
            int count = PlayerPrefs.GetInt("TotalBoxCount") - this.Cost[0];
            PlayerPrefs.SetInt("TotalBoxCount", count);

            PlayerPrefs.SetInt("Background" + Number.ToString() + "Bought", 1);
            MakeActiveBtn.SetActive(true);
            BuyBtn.SetActive(false);
            ShopManager.UpdateShop();
        }
    }

    public void BuySkin()
    {
        if (PlayerPrefs.GetInt("TotalBoxCount") >= this.Cost[0])
        {
            int count = PlayerPrefs.GetInt("TotalBoxCount") - this.Cost[0];
            PlayerPrefs.SetInt("TotalBoxCount", count);

            PlayerPrefs.SetInt("Skin" + Number.ToString() + "Bought", 1);
            MakeActiveBtn.SetActive(true);
            BuyBtn.SetActive(false);
            ShopManager.UpdateShop();
        }
    }

    public void BuyUpgradeRate()
    {
        if (PlayerPrefs.GetInt("TotalBoxCount") >= this.Cost[CheckVersion(Number)])
        {
            int count = PlayerPrefs.GetInt("TotalBoxCount") - this.Cost[CheckVersion(0)];
            PlayerPrefs.SetInt("TotalBoxCount", count);

            if (CheckVersion(0) == 0)
            {
                PlayerPrefs.SetInt("Upgrade1MinValue", 25);
                PlayerPrefs.SetInt("Upgrade1MaxValue", 75);
            }
            

            PlayerPrefs.SetFloat("Upgrade0Rate", UpgradeInfo.GoldBoxRate[CheckVersion(0)]);
            PlayerPrefs.SetInt("Upgrade0Bought", CheckVersion(0) + 1);

            if (this.Cost.Length == CheckVersion(0))
            {
                BoughtBtn.SetActive(true);
            }
            else
            {
                BuyBtn.transform.GetChild(0).GetComponent<Text>().text = this.Cost[CheckVersion(0)].ToString() + " - Boxes";
                LockedBtn.transform.GetChild(0).GetComponent<Text>().text = this.Cost[CheckVersion(0)].ToString() + " - Boxes";
                ExtraInfoText.GetComponent<Text>().text = ExtraInfo[CheckVersion(0)];
            }
            ShopManager.UpdateShop();
        }
    }

    public void BuyBoxValue()
    {
        if (PlayerPrefs.GetInt("TotalBoxCount") >= this.Cost[CheckVersion(Number)])
        {
            int count = PlayerPrefs.GetInt("TotalBoxCount") - this.Cost[CheckVersion(1)];
            PlayerPrefs.SetInt("TotalBoxCount", count);

            PlayerPrefs.SetInt("Upgrade" + Number.ToString() + "MinValue", UpgradeInfo.GoldBoxMinValue[CheckVersion(1)]);
            PlayerPrefs.SetInt("Upgrade" + Number.ToString() + "MaxValue", UpgradeInfo.GoldBoxMaxValue[CheckVersion(1)]);
            PlayerPrefs.SetInt("Upgrade" + Number.ToString() + "Value", CheckVersion(1) + 1);

            if (this.Cost.Length == CheckVersion(1))
            {
                BoughtBtn.SetActive(true);
            }
            else
            {
                BuyBtn.transform.GetChild(0).GetComponent<Text>().text = this.Cost[CheckVersion(1)].ToString() + " - Boxes";
                LockedBtn.transform.GetChild(0).GetComponent<Text>().text = this.Cost[CheckVersion(1)].ToString() + " - Boxes";
                ExtraInfoText.GetComponent<Text>().text = ExtraInfo[CheckVersion(1)];
            }
            ShopManager.UpdateShop();
        }
    }

    public void BuyDefuserTimer()
    {
        if (PlayerPrefs.GetInt("TotalBoxCount") >= this.Cost[CheckVersion(Number)])
        {
            int count = PlayerPrefs.GetInt("TotalBoxCount") - this.Cost[CheckVersion(2)];
            PlayerPrefs.SetInt("TotalBoxCount", count);


            PlayerPrefs.SetInt("Upgrade2Timer", UpgradeInfo.BombDefuserTimer[CheckVersion(2)]);
            PlayerPrefs.SetInt("Upgrade2Bought", CheckVersion(2) + 1);

            if (this.Cost.Length == CheckVersion(2))
            {
                BoughtBtn.SetActive(true);
            }
            else
            {
                BuyBtn.transform.GetChild(0).GetComponent<Text>().text = this.Cost[CheckVersion(2)].ToString() + " - Boxes";
                LockedBtn.transform.GetChild(0).GetComponent<Text>().text = this.Cost[CheckVersion(2)].ToString() + " - Boxes";
                ExtraInfoText.GetComponent<Text>().text = ExtraInfo[CheckVersion(0)];
            }
            ShopManager.UpdateShop();
        }
    }
}
