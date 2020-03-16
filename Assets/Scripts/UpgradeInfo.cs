using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradeInfo : MonoBehaviour {

    public float[] GoldBoxRate = new float[5];
    public int[] GoldBoxMinValue = new int[5];
    public int[] GoldBoxMaxValue = new int[5];
    public int[] BombDefuserTimer = new int[4];

    // Use this for initialization
    void Start()
    {
        GoldBoxRate[0] = .01f;
        GoldBoxRate[1] = .02f;
        GoldBoxRate[2] = .05f;
        GoldBoxRate[3] = .07f;
        GoldBoxRate[4] = .1f;

        
        GoldBoxMinValue[0] = 50;
        GoldBoxMinValue[1] = 75;
        GoldBoxMinValue[2] = 100;
        GoldBoxMinValue[3] = 100;
        GoldBoxMinValue[4] = 100;

        
        GoldBoxMaxValue[0] = 150;
        GoldBoxMaxValue[1] = 175;
        GoldBoxMaxValue[2] = 200;
        GoldBoxMaxValue[3] = 250;
        GoldBoxMaxValue[4] = 300;


        BombDefuserTimer[0] = 5;
        BombDefuserTimer[1] = 10;
        BombDefuserTimer[2] = 15;
        BombDefuserTimer[3] = 20;
    }
}
