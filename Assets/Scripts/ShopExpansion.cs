using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopExpansion : MonoBehaviour {

    public GameObject[] Subordinates;
    bool isRotated = true;

	
	public void ShowOrHideSubordinates()
    {
        foreach (GameObject sub in Subordinates)
        {
            if (sub.name != "UpgradeShopItem 1")
                sub.SetActive(!sub.activeSelf);
        }

        if (isRotated)
        {
            this.gameObject.transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            this.gameObject.transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, 0, -90);
        }

        isRotated = !isRotated;
    }
}
