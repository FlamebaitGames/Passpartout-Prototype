using UnityEngine;
using System.Collections;

public class PaintingSettingsPanel : MonoBehaviour {
    private Painting currentPainting;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (currentPainting != null)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(currentPainting.transform.position);
            GetComponent<RectTransform>().position = pos;
        }
	}

    public void Link(Painting painting)
    {
        if (currentPainting != null) Unlink();
        gameObject.SetActive(true);
        currentPainting = painting;
        Vector3 pos = Camera.main.WorldToScreenPoint(currentPainting.transform.position);
        GetComponent<RectTransform>().position = pos;
    }

    public void Unlink()
    {
        currentPainting = null;
        gameObject.SetActive(false);
    }


    private void RemovePainting()
    {
        currentPainting.Remove();
        Unlink();
    }

    private void OnEndDay()
    {
        Unlink();
    }
}
