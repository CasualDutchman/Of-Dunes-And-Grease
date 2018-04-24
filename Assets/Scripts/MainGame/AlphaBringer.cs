using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaBringer : MonoBehaviour {

    public Material nonAlpha;
    public Material Alpha;

    void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag.Equals("TerrainObj")) {
            col.GetComponent<MeshRenderer>().material = Alpha;
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.gameObject.tag.Equals("TerrainObj")) {
            col.GetComponent<MeshRenderer>().material = nonAlpha;
        }
    }
}
