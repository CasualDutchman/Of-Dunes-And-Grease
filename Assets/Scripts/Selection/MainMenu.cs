using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public RectTransform leftFist, rightFist, head, playButton, fightText, mover;
    public Image fader;

    bool hasPressed;

    bool contracting;
    bool smashing;

    bool gottime;
    bool faded;

    float timer;
    float time;

	void Start () {
		
	}
 
    void Update() {
        mover.position = Input.mousePosition + new Vector3(0, 0, -500);
        head.LookAt(mover);
    }

	void FixedUpdate () {
        if (smashing) {
            leftFist.Translate(Vector3.right * 1500 * Time.fixedDeltaTime);
            rightFist.Translate(-Vector3.right * 1500 * Time.fixedDeltaTime);

            if(leftFist.localPosition.x >= -600) {
                playButton.localScale = new Vector3((-500 - leftFist.localPosition.x) / 100.0f, 1, 1);
                if(playButton.localScale.x <= 0) {
                    playButton.localScale = new Vector3(0, 1, 1);
                }
            }

            if (leftFist.localPosition.x >= -500) {
                smashing = false;
                contracting = true;
            }
        }

        if (contracting) {
            leftFist.Translate(-Vector3.right * 1000 * Time.fixedDeltaTime);
            rightFist.Translate(Vector3.right * 1000 * Time.fixedDeltaTime);

            if (leftFist.localPosition.x <= -500) {
                fightText.localScale = new Vector3((-500 - leftFist.localPosition.x) / 200.0f, 1, 1);
                if (fightText.localScale.x >= 1.0f) {
                    fightText.localScale = new Vector3(1, 1, 1);
                }
            }

            if (leftFist.localPosition.x <= -900) {
                contracting = false;
                hasPressed = true;
                fader.gameObject.SetActive(true);
                StartLoading();
            }
        }

        if(hasPressed && !faded) {
            timer += Time.fixedDeltaTime * 2;

            if(timer >= 3) {
                if (!gottime) {
                    time = timer;
                    gottime = true;
                }

                fader.color = new Color(0, 0, 0, (timer - time) / 2.0f);

                if(fader.color.a >= 1) {
                    ActivateScene();
                }
            }
        }
    }

    AsyncOperation async;

    public void StartLoading() {
        StartCoroutine("load");
    }

    IEnumerator load() {
        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;
        yield return async;
    }

    public void ActivateScene() {
        if(async.progress >= 0.9f)
            async.allowSceneActivation = true;
    }

    public void ButtonPressPlay() {
        smashing = true;
        playButton.GetComponent<Button>().enabled = false;
    }
}
