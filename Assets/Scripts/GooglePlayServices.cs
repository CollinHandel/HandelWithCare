using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class GooglePlayServices : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        PlayGamesPlatform.Activate();
        //PlayGamesPlatform.DebugLogEnabled = true;

        DontDestroyOnLoad(transform.gameObject);
    }
}
