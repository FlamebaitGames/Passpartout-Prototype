using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[TweakableClass]
public class Customer : MonoBehaviour {
    public enum Evaluation
    {
        WORTH = 0,
        OUTSIDE_PRICE_RANGE = 1 << 1,
        OUTSIDE_PRICE_LEEWAY = 1 << 2,
        BELOW_TIME_REQ = 1 << 3,
        BELOW_FAME_REQ = 1 << 4,
        BELOW_NAMEFAC_REQ = 1 << 5,
        LAST,
        ALL = LAST * 2 - 1
    }

    private DialogBubble dialog;
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
    [TweakableField, SerializeField]
    private string[] buyResponses = { "This i must have!" };
    [TweakableField, SerializeField]
    private string[] outsidePricerangeResponses = { "This is out of my price range." };
    [TweakableField, SerializeField]
    private string[] outsideLeewayResponses = {"This painting is far overpriced." };
    [TweakableField, SerializeField]
    private string[] belowTimespentResponses = { "Not much work went in to this one..." };
    [TweakableField, SerializeField]
    private string[] belowFameResponses = { "This guy really is a nobody." };
    [TweakableField, SerializeField]
    private string[] belowNameFactorResponses = { "The name is bad." };

    private Vector3 lookTarget = Vector3.zero;

    private Player player;

	public AudioClip[] greetingSound;
	public AudioClip[] wowSound;
	public AudioClip[] byeSound;
	public AudioClip[] ponderSound;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
        dialog = GetComponentInChildren<DialogBubble>();
        Debug.Assert(player != null);

	}
	
	// Update is called once per frame
	void Update () {
        FacePoint(lookTarget);
	}

    private string RandomLine(string[] lines)
    {
        if (lines.Length == 0) return "Err: No lines!";
        return lines[Random.Range(0, lines.Length)];
    }

    public void BeginTraversingPath(PathPoint[] path)
    {
        StartCoroutine(Travel(path));
		StartCoroutine (WaitForGreeting ());
    }


    private Evaluation IsWorthPurchasing(Painting painting)
    {
        Evaluation ev = Evaluation.WORTH;
        
        if (spendingLeeway < (painting.price - painting.truePrice)) ev |= Evaluation.OUTSIDE_PRICE_LEEWAY;
        if (priceRangeMax < painting.price || painting.price < priceRangeMin) ev |= Evaluation.OUTSIDE_PRICE_RANGE;
        if (painting.nameFactor < minNameFactor) ev |= Evaluation.BELOW_NAMEFAC_REQ;
        if (player.fame < minFame) ev |= Evaluation.BELOW_FAME_REQ;
        if (painting.timeSpent < minPaintingTime) ev |= Evaluation.BELOW_TIME_REQ;

        return ev;
    }


    private void FacePoint(Vector3 point)
    {
        point.y = 0.0f;
        Quaternion a = transform.rotation;
        Quaternion b = Quaternion.LookRotation((point - transform.position).normalized, transform.up);
        float angle = Quaternion.Angle(a, b);
        transform.rotation = Quaternion.Lerp(a, b, Time.deltaTime * angle * 0.1f);
    }

    private IEnumerator Travel(PathPoint[] path)
    {
        foreach (PathPoint p in path)
        {
            GetComponentInChildren<Animator>().SetFloat("Moving", 1.0f);
            while (Vector3.Distance(transform.position, p.point) > 0.1f)
            {
                
                transform.position = Vector3.MoveTowards(transform.position, p.point, Time.deltaTime);
                lookTarget = p.point;
                yield return null;
            }
            if (p.canLookAtPainting)
            {
                GetComponentInChildren<Animator>().SetFloat("Moving", 0.0f);
                lookTarget = p.targetPainting.transform.position;
                yield return new WaitForSeconds(0.3f);
                yield return StartCoroutine(ExaminePainting(p.targetPainting));
                
            }
            
        }
		Debug.Log ("Bye!");
		PlayBye ();
        SendMessageUpwards("OnCustomerExit", this);



    }

    private IEnumerator ExaminePainting(Painting painting)
    {

        Evaluation ev = IsWorthPurchasing(painting);
        List<Evaluation> indicies = new List<Evaluation>();
        for (int i = 1; i <= 5; i++)
        {
            Evaluation e = (Evaluation)(1 << i);
            if ((ev & e) == e)
            {
                indicies.Add(e);
            }
        }

        if (indicies.Count > 0)
        {
            Evaluation eval = indicies[Random.Range(0, indicies.Count)];
            switch (eval)
            {
                case Evaluation.BELOW_FAME_REQ:
                    dialog.vBubble[0].vMessage = RandomLine(belowFameResponses);
                    dialog.ShowBubble();
                    yield return new WaitForSeconds(4.2f);
                    
                    break;
                case Evaluation.BELOW_NAMEFAC_REQ:
                    dialog.vBubble[0].vMessage = RandomLine(belowNameFactorResponses);
                    dialog.ShowBubble();
                    yield return new WaitForSeconds(4.2f);
                    break;
                case Evaluation.BELOW_TIME_REQ:
                    dialog.vBubble[0].vMessage = RandomLine(belowTimespentResponses);
                    dialog.ShowBubble();
                    yield return new WaitForSeconds(4.2f);
                    break;
                case Evaluation.OUTSIDE_PRICE_LEEWAY:
                    dialog.vBubble[0].vMessage = RandomLine(outsideLeewayResponses);
                    dialog.ShowBubble();
                    yield return new WaitForSeconds(4.2f);
                    break;
                case Evaluation.OUTSIDE_PRICE_RANGE:
                    dialog.vBubble[0].vMessage = RandomLine(outsidePricerangeResponses);
                    dialog.ShowBubble();
                    yield return new WaitForSeconds(4.2f);
                    break;
                default:
                    Debug.LogError("Evaluation type unknown: " + eval.ToString());
                    Debug.Break();
                    break;
            }
        }
        else
        {
            dialog.vBubble[0].vMessage = RandomLine(buyResponses);
            dialog.ShowBubble();
            SendMessageUpwards("PurchasePainting", painting);
            yield return new WaitForSeconds(4.2f);
        }
        Destroy(dialog.GetComponentInChildren<Appear>().gameObject);

        

        
        /*switch(ev) {
            case Evaluation.SHIT:
                dialog.vBubble[0].vMessage = RandomLine(shitResponses);
                dialog.ShowBubble();
				PlayPonder ();
                yield return new WaitForSeconds(4.5f);
                break;
            case Evaluation.AFFORDABLE:
                dialog.vBubble[0].vMessage = RandomLine(notMyTasteResponses);
                dialog.ShowBubble();
				PlayPonder ();
                yield return new WaitForSeconds(4.5f);
                break;
            case Evaluation.MY_TASTE:
                dialog.vBubble[0].vMessage = RandomLine(noAffordResponses);
                dialog.ShowBubble();
				PlayPonder ();
                yield return new WaitForSeconds(4.5f);
                break;
            case Evaluation.BOTH:
                dialog.vBubble[0].vMessage = RandomLine(buyResponses);
                dialog.ShowBubble();
				PlayWow();
                SendMessageUpwards("PurchasePainting", painting);
                yield return new WaitForSeconds(4.2f);
                break;
        }*/
    }

	private IEnumerator WaitForGreeting()
	{
		yield return new WaitForSeconds (1);
		PlayGreeting();
		Debug.Log ("Helloooo!");
	}


	private void PlayGreeting()
	{
		if (greetingSound.Length == 0) return;
		GetComponent<AudioSource>().PlayOneShot(greetingSound[Random.Range(0, greetingSound.Length - 1)]);
	}

	private void PlayPonder()
	{
		if (ponderSound.Length == 0) return;
		GetComponent<AudioSource>().PlayOneShot(ponderSound[Random.Range(0, ponderSound.Length - 1)]);
	}

	private void PlayWow()
	{
		if (wowSound.Length == 0) return;
		GetComponent<AudioSource>().PlayOneShot(wowSound[Random.Range(0, wowSound.Length - 1)]);
	}

	private void PlayBye()
	{
		if (byeSound.Length == 0) return;
		GetComponent<AudioSource>().PlayOneShot(byeSound[Random.Range(0, byeSound.Length - 1)]);
	}

}
