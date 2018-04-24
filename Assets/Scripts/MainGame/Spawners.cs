using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawners : MonoBehaviour {

    public GameObject enemy, bigEnemy, sandsplash;
    public Transform[] spawners;

    public float interval;

    float timer;

	void Start () {
		
	}
	
	void Update () {
        timer += 1 * Time.deltaTime;
        if(timer >= interval) {
            int ran = Random.Range(0, spawners.Length);
            Instantiate(sandsplash, spawners[ran].position + new Vector3(0, 0, .5f), Quaternion.identity);
            Instantiate(Random.Range(0, 20) == 0 ? bigEnemy : enemy, spawners[ran].position, spawners[ran].rotation);
            

            timer = 0;
        }
	}
}
