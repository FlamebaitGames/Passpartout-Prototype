using UnityEngine;
using System.Collections;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Painting : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    /// <summary>
    /// Example method for creating a read/write texture
    /// </summary>
    /// <returns></returns>
    public static Texture2D CreateSampleTexture2D()
    {
        Texture2D tx = new Texture2D(32, 32);
        tx.wrapMode = TextureWrapMode.Clamp;
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                tx.SetPixel(i, j, Color.yellow);
            }
        }
        tx.Apply();
        return tx;
    }

    /// <summary>
    /// Creates a new painting GameObject
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="gameObjectName"></param>
    /// <returns></returns>
    public static Painting Create(Texture canvas, string gameObjectName = "Painting")
    {
        Painting p = Instantiate<Painting>(Resources.Load<Painting>("Painting"));
        GameObject parent = GameObject.Find("Paintings");
        p.GetComponent<Renderer>().material.mainTexture = canvas;
        if (parent != null) p.transform.parent = parent.transform;
        return p;
    }
}
