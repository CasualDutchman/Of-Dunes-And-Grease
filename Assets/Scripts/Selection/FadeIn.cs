using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {

    public DisplayText dText;
    Image image;

    float timer, time;
    bool gottime;

	void Start () {
        image = GetComponent<Image>();
        image.color = new Color(0, 0, 0, 1);
	}
	
	void Update () {
        timer += Time.deltaTime;

        if (timer >= 0.5f) {
            if(!gottime) {
                time = timer;
                gottime = true;
            }
            image.color = new Color(0, 0, 0, 1 - ((timer - time) / 1.7f));

            if (image.color.a <= 0) {
                if(dText != null)
                    dText.enabled = true;
                DestroyImmediate(gameObject);
            }
        }
    }
}
