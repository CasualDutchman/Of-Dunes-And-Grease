using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingText : MonoBehaviour {

    Text text;
    public float speed = 0.02f;
    public bool startAlone = true;
    public bool loop = false;
    string actualString, str;
    bool done;

    void Start() {
        text = GetComponent<Text>();
        actualString = text.text;
        if (startAlone)
            StartTyping(actualString);
    }

    void Update() {
        if(done && loop) {
            StopCoroutine(AnimateText(""));
            done = false;
            StartTyping(actualString);
        }
    }

    public void StartTyping(string str) {
        StartCoroutine(AnimateText(actualString));
    }

    IEnumerator AnimateText(string strComplete) {
        int i = 0;
        str = "";
        while (i < strComplete.Length) {
            str += strComplete[i++];
            text.text = str;
            if(i> strComplete.Length - 1) {
                done = true;
            }
            yield return new WaitForSeconds(speed);
        }
    }
}
