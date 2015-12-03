using UnityEngine;
using System.Collections;

public class PathPoint : MonoBehaviour {
    public Painting targetPainting;
    public bool canLookAtPainting { get { return targetPainting != null && targetPainting.visible; } }
    public Vector3 point { get { return transform.position; } }
	public string callEvent;
}
