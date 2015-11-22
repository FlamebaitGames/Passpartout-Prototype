using UnityEngine;
using System.Collections;

public class AlignToCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Camera.main.transform.position + (transform.parent.position - Camera.main.transform.position).normalized * 2.0f;
        transform.forward = Camera.main.transform.forward;
	}
}
