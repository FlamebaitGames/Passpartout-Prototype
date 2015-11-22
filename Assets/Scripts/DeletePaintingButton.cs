using UnityEngine;
using System.Collections;

public class DeletePaintingButton : MonoBehaviour {
    void OnMouseUp()
    {
        GetComponentInParent<Painting>().Remove();
    }
}
