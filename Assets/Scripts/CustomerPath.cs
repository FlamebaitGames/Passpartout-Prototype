using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class CustomerPath : MonoBehaviour {
    [SerializeField]
    public Vector3[] points { get; private set; }

#if UNITY_EDITOR
    [SerializeField]
    private bool show = true;
#endif


	void Update () {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            points = new Vector3[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                points[i] = transform.GetChild(i).position;
            }
            BroadcastMessage("Show", show);
        }
#endif
	}

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!show) return;
        for (int i = 0; i < points.Length; i++)
        {
            if (i > 0) Gizmos.DrawLine(points[i - 1], points[i]);
        }
    }
#endif

}
