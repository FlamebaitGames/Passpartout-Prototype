using UnityEngine;
using System.Collections;

public class PathPoint : MonoBehaviour {
    public Painting targetPainting;
    public bool pointToLookAtPaintingFrom { get { return targetPainting != null; } }
    public Vector3 point { get { return transform.position; } }
}
