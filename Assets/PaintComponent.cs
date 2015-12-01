using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class PaintComponent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler {
    public Texture2D canvas;
    public Camera cam;


    private Vector2 lastMousePoistion = Vector2.zero;
	// Use this for initialization
	void Start () {
        var original = gameObject.GetComponent<UnityEngine.UI.RawImage>().mainTexture as Texture2D;

        canvas = new Texture2D((int)GetComponent<RectTransform>().rect.width, (int)GetComponent<RectTransform>().rect.height);
        gameObject.GetComponent<UnityEngine.UI.RawImage>().texture = canvas;

        for (int y = 0; y < canvas.height; ++y)
        {
            for (int x = 0; x < canvas.width; ++x)
            {
                canvas.SetPixel(x, y, Color.white);
            }
        }
        canvas.Apply();
    }

    public void OnPointerDown(PointerEventData data)
    {
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {

    }
    public void OnDrag(PointerEventData data)
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, data.pointerCurrentRaycast.screenPosition, data.pressEventCamera, out pos);

        pos.y = rect.rect.height - Mathf.Abs(pos.y);
        pos.y /= rect.rect.height / canvas.height;
        pos.x /= rect.rect.width / canvas.width;
        DrawCircle(canvas, (int)pos.x, (int)pos.y, 16, Color.black);
        canvas.Apply();
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetMouseButton(0))
        {
            RectTransform rect = GetComponent<RectTransform>();
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out pos);

            pos.y = rect.rect.height - Mathf.Abs(pos.y);
            pos.y /= rect.rect.height / canvas.height;
            pos.x /= rect.rect.width / canvas.width;
            DrawCircle(canvas, (int)pos.x, (int)pos.y, 16, Color.black);
            canvas.Apply();
        }
        */
	}


    public void DrawCircle(Texture2D tex, int cx, int cy, int r, Color col)
    {
        Color32[] pixels = canvas.GetPixels32();

        int x, y, px, nx, py, ny, d;

        for (x = 0; x <= r; x++)
        {
            d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
            for (y = 0; y <= d; y++)
            {
                px = cx + x;
                nx = cx - x;
                py = cy + y;
                ny = cy - y;


                pixels[py * canvas.width + px] = col;
                pixels[py * canvas.width + nx] = col;
                pixels[ny * canvas.width + px] = col;
                pixels[ny * canvas.width + nx] = col;

                /*
                tex.SetPixel(px, py, col);
                tex.SetPixel(nx, py, col);

                tex.SetPixel(px, ny, col);
                tex.SetPixel(nx, ny, col);
                */
            }
        }
        tex.SetPixels32(pixels);
    }
}
