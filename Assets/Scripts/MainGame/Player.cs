using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public GameObject[] disableOnDeath;
    public Text scoreText;
    public GameObject menupopup, menutext, deadtext;

    public Image Red, Blue, Yellow, Green;
    public Image RedBroken, BlueBroken, YellowBroken, GreenBroken;

    public float redOil, blueOil, yellowOil, greenOil;
    public bool hasRed = true, hasBlue = true, hasYellow = true, hasGreen = true;
    public float maxOil = 100;

    public int score;
    float scoretimer;

    public bool Dead;
    bool hasdied;

    void Start () {
    }
	
    void Update() {
        scoretimer += Time.deltaTime;
        if(scoretimer >= 10 && !Dead) {
            score += 100;
            scoretimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !Dead) {
            if (!menutext.activeSelf) {
                menupopup.SetActive(true);
                menutext.SetActive(true);
                deadtext.SetActive(false);
                Time.timeScale = 0.00f;
            }
            else {
                menupopup.SetActive(false);
                menutext.SetActive(false);
                deadtext.SetActive(false);
                Time.timeScale = 1;
            }
        } 
    }

    public void ClickResume() {
        menupopup.SetActive(false);
        menutext.SetActive(false);
        deadtext.SetActive(false);
        Time.timeScale = 1;
    }

    public void ClickMenu() {
        SceneManager.LoadScene(0);
    }

    public void ClickQuit() {
        Application.Quit();
    }

    void FixedUpdate () {
        redOil = Mathf.Clamp(redOil - (0.5f * Time.fixedDeltaTime), 0, maxOil);
        blueOil = Mathf.Clamp(blueOil - (0.5f * Time.fixedDeltaTime), 0, maxOil);
        yellowOil = Mathf.Clamp(yellowOil - (0.5f * Time.fixedDeltaTime), 0, maxOil);
        greenOil = Mathf.Clamp(greenOil - (0.5f * Time.fixedDeltaTime), 0, maxOil);

        Red.fillAmount = redOil / 100;
        Blue.fillAmount = blueOil / 100;
        Yellow.fillAmount = yellowOil / 100;
        Green.fillAmount = greenOil / 100;

        if (redOil <= 0 && hasRed) {
            hasRed = false;
            Red.transform.parent.gameObject.SetActive(false);
            RedBroken.gameObject.SetActive(true);
            GetComponent<AudioSource>().Play();
        }
        if (blueOil <= 0 && hasBlue) {
            hasBlue = false;
            Blue.transform.parent.gameObject.SetActive(false);
            BlueBroken.gameObject.SetActive(true);
            GetComponent<AudioSource>().Play();
        }
        if (yellowOil <= 0 && hasYellow) {
            hasYellow = false;
            Yellow.transform.parent.gameObject.SetActive(false);
            YellowBroken.gameObject.SetActive(true);
            GetComponent<AudioSource>().Play();
        }
        if (greenOil <= 0 && hasGreen) {
            hasGreen = false;
            Green.transform.parent.gameObject.SetActive(false);
            GreenBroken.gameObject.SetActive(true);
            GetComponent<AudioSource>().Play();
        }

        if (!hasRed && !hasBlue && !hasYellow && !hasGreen) {
            Dead = true;
        }

        if (Dead && !hasdied) {
            hasdied = true;
            scoreText.text = "Score: " + score;

            menupopup.SetActive(true);
            menutext.SetActive(false);
            deadtext.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void AddOil(int id, float amount) {
        if(id == 0 && hasRed)
            redOil = Mathf.Clamp(redOil + amount, 0, maxOil);
        if (id == 1 && hasBlue)
            blueOil = Mathf.Clamp(blueOil + amount, 0, maxOil);
        if (id == 2 && hasYellow)
            yellowOil = Mathf.Clamp(yellowOil + amount, 0, maxOil);
        if (id == 3 && hasGreen)
            greenOil = Mathf.Clamp(greenOil + amount, 0, maxOil);
    }

    public float RemoveOil(int id, float amount) {
        AddOil(id, -amount);
        return amount * ((100 - greenOil) / 50);
    }
}
