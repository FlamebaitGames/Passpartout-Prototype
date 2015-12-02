using UnityEngine;
using System.Collections;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Painting : MonoBehaviour {
    
    public string title = "DefaultTitle";
    public int price = 0;
    public int truePrice = 0;
    public float timeSpent = 0.0f;
    public float nameFactor = 1.0f;
    public GameObject infoPane;
    public bool visible { get { return gameObject.activeSelf; } }
    /// <summary>
    /// Clears all values of this painting, returning it to a blank slate.
    /// </summary>
    public void Clear()
    {
        title = "DefaultTitle";
        price = 0;
    }
    /// <summary>
    /// Removes the painting from the wall by hiding it.
    /// </summary>
    public void Remove()
    {
        Clear();
        OnDeselected();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Clears this painting and assigns a new texture to the canvas,
    /// effectively replacing the previous painting.
    /// </summary>
    /// <param name="canvas"></param>
    public void OverwriteNew(Texture canvas)
    {
        Clear();
        GetComponent<Renderer>().materials[0].mainTexture = canvas;
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

    void OnMouseDown()
    {
        
    }


    //
    //      EVENTS DEFINED BELOW
    //
    
    // User has clicked on this painting
    private void OnSelected()
    {
        infoPane.SetActive(true);
    }

    private void OnDeselected()
    {
        infoPane.SetActive(false);
    }
}
