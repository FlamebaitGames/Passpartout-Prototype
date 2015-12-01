using UnityEngine;
using System.Collections;

public class PaintComponent : MonoBehaviour {
    public Texture2D canvas;
    public RenderTexture rtexture;
    public Camera camera;
	// Use this for initialization
	void Start () {
        canvas = new Texture2D(550, 900);
        


        for (int y = 0; y < canvas.height; ++y)
        {
            for (int x = 0; x < canvas.width; ++x)
            {
                canvas.SetPixel(x, y, Color.white);
            }
        }
        canvas.Apply();
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), canvas);
    }
	
	// Update is called once per frame
	void Update () {
        canvas.SetPixel(100, 100, Color.green);
        canvas.Apply();
        //gameObject.GetComponent<Renderer>().material.mainTexture = canvas;
        if (!Input.GetMouseButton(0)) return;

        RaycastHit hit = new RaycastHit();
        if (!Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        Renderer renderer = hit.collider.GetComponent<Renderer>();

        MeshCollider collider = (MeshCollider)hit.collider;

        if (renderer != null || renderer.sharedMaterial == null
            || renderer.sharedMaterial.mainTexture == null || collider == null) return;

        int total = 70 * 30;

        Color[] colors = new Color[total];
        for (int i = 0; i < total; ++i)
        {
            colors[i] = Color.black;
        }

        //Texture texture = renderer.material.mainTexture;
        Vector2 PixelUV = hit.textureCoord2;

        PixelUV.x *= canvas.width;
        PixelUV.y *= canvas.height;


        RenderTexture.active = rtexture;
        canvas.SetPixel(100, 100, Color.green);
        //canvas.SetPixels((int)PixelUV.x, (int)PixelUV.y, 70, 30, colors);
        canvas.Apply();
        RenderTexture.active = null;
        
	}
}
