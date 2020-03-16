using UnityEngine;
using System.Collections;

public class ChallengeInfo : MonoBehaviour {

    private int[] Level1Challenge = new int[3] { 100, 500, 1000 };
    private int[] Level2Challenge = new int[3] { 100, 500, 1000 };
    private int[] Level3Challenge = new int[3] { 100, 500, 1000 };
    private int[] TotalBoxes = new int[5] { 500, 1000, 7500, 15000, 50000 };

    public int LevelChallenge(int level, int version)
    {
        if (level == 1)
            return Level1Challenge[version];
        else if (level == 2)
            return Level2Challenge[version];
        else if (level == 3)
            return Level3Challenge[version];
        else
        {
            Debug.LogError("INVALID LEVEL NUMBER");
            return 0;
        }
    }

    public int ReturnLength(int level)
    {
        if (level == 0)
            return TotalBoxes.Length;
        else if (level == 1)
            return Level1Challenge.Length;
        else if (level == 2)
            return Level2Challenge.Length;
        else if (level == 3)
            return Level3Challenge.Length;
        else
        {
            Debug.LogError("INVALID LEVEL NUMBER");
            return 0;
        }
    }

    public int TotalBoxesResult()
    {
        int version = PlayerPrefs.GetInt("TotalBoxesCollectedVersion", 0);
        if (version >= TotalBoxes.Length)
        {
            return 0;
        } else
            return TotalBoxes[version];
    }
}
