using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[TweakableClass]
public class Customer : MonoBehaviour {
    public enum Evaluation
    {
        SHIT = 0,
        MY_TASTE = 1 << 1,
        AFFORDABLE = 1 << 2,
        BOTH = MY_TASTE | AFFORDABLE
    }
    public DialogBubble dialog;
    [TweakableField(true)]
    public float moveSpeed = 1.0f;
    [TweakableField]
    public int priceRangeMin = 10;
    [TweakableField]
    public int priceRangeMax = 200;
    [TweakableField, Tooltip("How much the price of the painting can deviate from the true price (upwards)")]
    public int spendingLeeway = 200;
    [TweakableField, Range(0, 160)]
    public float minPaintingTime = 10.0f;
    [TweakableField]
    public int minFame = 2;
    [TweakableField, Range(0.0f, 1.5f)]
    public float minNameFactor = 1.0f;

    private Player player;
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
        Debug.Assert(player != null);
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void BeginTraversingPath(PathPoint[] path)
    {
        StartCoroutine(Travel(path));
    }


    private Evaluation IsWorthPurchasing(Painting painting)
    {
        Evaluation ev = Evaluation.SHIT;
        if (minNameFactor <= painting.nameFactor &&
            minFame <= player.fame)
            ev |= Evaluation.MY_TASTE;
        if (priceRangeMin <= painting.price &&
            painting.price <= priceRangeMax &&
            (painting.price - painting.truePrice) <= spendingLeeway)
            ev |= Evaluation.AFFORDABLE;
        return ev;
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
        Evaluation ev = IsWorthPurchasing(painting);
        switch(ev) {
            case Evaluation.SHIT:
                dialog.vBubble[0].vMessage = "It's shit!";
                dialog.ShowBubble();
                yield return new WaitForSeconds(4.5f);
                break;
            case Evaluation.AFFORDABLE:
                dialog.vBubble[0].vMessage = "I could buy it, but I don't like it.";
                dialog.ShowBubble();
                yield return new WaitForSeconds(4.5f);
                break;
            case Evaluation.MY_TASTE:
                dialog.vBubble[0].vMessage = "Looks nice, but a bit pricey for me.";
                dialog.ShowBubble();
                yield return new WaitForSeconds(4.5f);
                break;
            case Evaluation.BOTH:
                dialog.vBubble[0].vMessage = "I must have this!";
                dialog.ShowBubble();
                SendMessageUpwards("PurchasePainting", painting);
                yield return new WaitForSeconds(4.2f);
                break;
        }
    }
}
