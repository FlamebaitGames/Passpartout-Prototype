using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class CustomerPath : MonoBehaviour {
    [SerializeField, HideInInspector]
    private PathPoint[] _path;
    public PathPoint[] path { get { return _path; } }

#if UNITY_EDITOR
    [SerializeField]
    private bool show = true;
#endif


	void Update () {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            _path = GetComponentsInChildren<PathPoint>();
            BroadcastMessage("Show", show);
        }
#endif
	}

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!show) return;
        for (int i = 0; i < _path.Length; i++)
        {
            if (i > 0) Gizmos.DrawLine(_path[i - 1].transform.position, _path[i].transform.position);
        }
    }
#endif

}
