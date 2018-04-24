using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour {

    public RectTransform hud;
    public Text[] texts;
    public float[] intervals;

    float timer, sizer;
    int nextIndex = 0;
    bool doneLoading, doneAll;

	void Start () {

    }
	
	void Update () {
        timer += Time.deltaTime;

        if(!doneAll && timer >= intervals[nextIndex]) {
            doneLoading = true;
        }

        if (!doneAll && doneLoading && Input.anyKeyDown) {
            doneLoading = false;
            timer = 0;
            texts[nextIndex].gameObject.SetActive(false);
            nextIndex++;
            if(nextIndex == texts.Length) {
                doneAll = true;
                timer = 0;
                return;
            }
            texts[nextIndex].gameObject.SetActive(true);
        }

        if(doneAll) {
            transform.Translate(Vector3.up * Time.deltaTime * 350);
            sizer = Mathf.Clamp(1000 - (timer * 350), 100, 9999);
            hud.sizeDelta = new Vector2(sizer, sizer);

            if(transform.localPosition.y >= 930)
                GameObject.FindGameObjectWithTag("Player").GetComponent<BeginGame>().startGame();
            if (transform.localPosition.y >= 1211)
                DestroyImmediate(gameObject);
        }
	}
}
