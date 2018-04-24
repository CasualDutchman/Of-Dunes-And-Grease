using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginGame : MonoBehaviour {

    public GameObject[] activateOnStart;
    public RectTransform hud;

    public StartState state;

    public bool startWithoutTerminal;

    public enum StartState {
        Terminal,
        Start
    }

	void Start () {
        state = StartState.Terminal;
        if(startWithoutTerminal) {
            DestroyImmediate( GameObject.FindGameObjectWithTag("Terminal") );
            hud.sizeDelta = new Vector2(100, 100);
            startGame();
        }
	}
	
	void Update () {
		
	}

    public void startGame() {
        state = StartState.Start;

        GetComponent<PlayerMover>().enabled = true;
        GetComponent<Player>().enabled = true;

        foreach(GameObject go in activateOnStart) {
            go.SetActive(true);
        }

        this.enabled = false;
    }
}
