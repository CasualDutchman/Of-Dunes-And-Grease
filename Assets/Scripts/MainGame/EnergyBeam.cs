using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBeam : MonoBehaviour {

    public float countDown;

    float timer;
    bool done;

	void Start () {
        GetComponent<AudioSource>().PlayDelayed(Random.Range(0, 0.1f));
	}
	
	void Update () {
        timer += Time.deltaTime;

        if (timer >= countDown && !done) {
            DestroyImmediate(transform.GetChild(0).gameObject);
            done = true;
        }

        if(!GetComponent<AudioSource>().isPlaying && done) {
            DestroyImmediate(gameObject);
        }

	}
}
