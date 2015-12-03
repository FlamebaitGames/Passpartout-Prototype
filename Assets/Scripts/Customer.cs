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
    private string[] buyResponses;
    [TweakableField, SerializeField]
    private string[] noAffordResponses;
    [TweakableField, SerializeField]
    private string[] notMyTasteResponses;
    [TweakableField, SerializeField]
    private string[] shitResponses;

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
        switch(ev) {
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
        }
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
