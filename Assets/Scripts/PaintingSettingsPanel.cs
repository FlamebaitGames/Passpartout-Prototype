using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PaintingSettingsPanel : MonoBehaviour {
    private Painting currentPainting;
    [SerializeField]
    private InputField priceField;
    [SerializeField]
    private Text descriptiveText;
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
        
        priceField.text = currentPainting.price.ToString();
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

    private void OnPriceInputChange()
    {
        currentPainting.price = int.Parse(priceField.text);
    }
}
