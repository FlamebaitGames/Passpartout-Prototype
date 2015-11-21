using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[UsedInTweaker]
public class Customer : MonoBehaviour {
    public DialogBubble dialog;
    [ShowInTweaker(true)]
    public float moveSpeed = 1.0f;
    [ShowInTweaker]
    public int test;
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


    private bool IsWorthPurchasing(Painting painting)
    {
        // TODO - do them calculations
        return Random.Range(0.0f, 1.0f) < 0.7f; // Temp
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
                yield return StartCoroutine(ExaminePainting(p.targetPainting));
                
            }
            
        }
        SendMessageUpwards("OnCustomerExit", this);
    }

    private IEnumerator ExaminePainting(Painting painting)
    {
        if (IsWorthPurchasing(painting))
        { // purchase
            dialog.vBubble[0].vMessage = "I buy!";
            dialog.ShowBubble();
            SendMessageUpwards("PurchasePainting", painting);
            yield return new WaitForSeconds(4.2f);
        }
        else
        { // reject
            dialog.vBubble[0].vMessage = "It's shit!";
            dialog.ShowBubble();
            yield return new WaitForSeconds(4.5f);
        }
        yield return null;
    }
}
