using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeText : MonoBehaviour {

	// Update is called once per frame
	void Update () {
	    if (this.GetComponent<Text>().color.a < 0)
        {
            Destroy(this.gameObject);
        }

        Color newColor = this.GetComponent<Text>().color;
        newColor.a -= .5f * Time.deltaTime;

        this.GetComponent<Text>().color = newColor;
	}
}
