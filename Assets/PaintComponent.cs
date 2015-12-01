using UnityEngine;
using System.Collections;

public class PaintComponent : MonoBehaviour {
    public Texture2D canvas;
    public Camera cam;
	// Use this for initialization
	void Start () {
        var original = gameObject.GetComponent<UnityEngine.UI.RawImage>().mainTexture as Texture2D;

        canvas = new Texture2D(original.width, original.height);
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

	// Update is called once per frame
	void Update () {
        /*
        if (!Input.GetMouseButton(0))
            return;

        RaycastHit hit;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        Renderer rend = hit.transform.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
            return;

        Texture2D tex = canvas;
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;
        tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.black);
        tex.Apply();
        */

        Vector2 s = new Vector2(canvas.width, canvas.height);
        Vector2 p = gameObject.GetComponent<RectTransform>().position;
        Rect r = gameObject.GetComponent<RectTransform>().rect;
        Vector2 m = Input.mousePosition;
        float relWidth = Screen.width / 1920.0f;
        float relHeight = Screen.height / 1080.0f;

        m.y *= relHeight;
        m.x *= relWidth;
        // Bounds check
        if (r.Contains(m))
        {
            int x = (int)(m.x);
            int y = (int)(m.y);

            DrawCircle(canvas, (int)m.x, (int)m.y,16, Color.black);
            canvas.Apply();
        }
	}


    public void DrawCircle(Texture2D tex, int cx, int cy, int r, Color col)
    {
        var pixels = canvas.GetPixels32();
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

                tex.SetPixel(px, py, col);
                tex.SetPixel(nx, py, col);

                tex.SetPixel(px, ny, col);
                tex.SetPixel(nx, ny, col);

            }
        }
    }
}
