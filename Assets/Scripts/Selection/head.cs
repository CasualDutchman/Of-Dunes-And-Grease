using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class head : MonoBehaviour {

    public Sprite face1, face2;

    Image im;

    float timer;

	void Start () {
        im = GetComponent<Image>();
	}
	
	void FixedUpdate () {
        timer += Time.fixedDeltaTime;
        if (timer >= 0.1f) {
            im.sprite = im.sprite == face1 ? face2 : face1;
            timer = 0;
        }

    }
}
