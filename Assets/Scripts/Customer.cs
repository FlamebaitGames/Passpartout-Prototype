using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BeginTraversingPath(PathPoint[] path)
    {
        StartCoroutine(Travel(path));
    }

    private IEnumerator Travel(PathPoint[] path)
    {
        foreach (PathPoint p in path)
        {
            while (Vector3.Distance(transform.position, p.point) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, p.point, Time.deltaTime);
                yield return null;
            }
            if (p.canLookAtPainting)
            {
                yield return new WaitForSeconds(0.3f);
            }
            
        }
        SendMessageUpwards("OnCustomerExit", this);
    }
}
