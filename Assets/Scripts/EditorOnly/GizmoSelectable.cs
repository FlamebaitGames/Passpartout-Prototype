#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEditor;
[ExecuteInEditMode]
public class GizmoSelectable : MonoBehaviour {
    private bool show;
    [UnityEditor.DrawGizmo(GizmoType.Pickable)]
    void OnDrawGizmos()
    {
        if (!show) return;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }

    private void Show(bool show)
    {
        this.show = show;
    }
}

#endif
