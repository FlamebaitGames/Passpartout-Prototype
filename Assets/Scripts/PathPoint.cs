using UnityEngine;
using System.Collections;

public class PathPoint : MonoBehaviour {
    public Painting lookTarget;
    public bool pointToLookAtPaintingFrom { get { return lookTarget != null; } }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
