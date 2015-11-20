using UnityEngine;
using System.Collections;

public class Gallery : MonoBehaviour {


    public Painting[] paintings { get; private set; }
    /// <summary>
    /// How many slots are left to put paintings on
    /// </summary>
    public int slotsLeft
    {
        get
        {
            int i = 0;
            foreach (Painting p in paintings)
            {
                if (!p.gameObject.activeSelf) i++;
            }
            return i;
        }
    }

    public Customer tmp;

    private PathPoint[] galleryPath;
	// Use this for initialization
	void Start () {
        paintings = GetComponentsInChildren<Painting>();
        foreach (Painting p in paintings)
        {
            p.gameObject.SetActive(false);
        }
        galleryPath = GetComponentInChildren<CustomerPath>().path;
        Debug.Log(galleryPath);
        tmp.BeginTraversingPath(galleryPath);
	}

    

    /// <summary>
    /// Adds a new painting on the wall, will throw an error if no slots
    /// are left.
    /// </summary>
    /// <param name="canvas"></param>
    public Painting AddNewPainting(Texture canvas)
    {
        if (!(slotsLeft > 0))
        {
            Debug.LogError("No slots left to put paintings on!");
            Debug.Break();
        }
        
        foreach (Painting p in paintings)
        {
            Debug.Log(p);
            if (!p.gameObject.activeSelf)
            {
                p.gameObject.SetActive(true);
                p.OverwriteNew(canvas);
                return p;
            }
        }
        return null;
        
    }
}
