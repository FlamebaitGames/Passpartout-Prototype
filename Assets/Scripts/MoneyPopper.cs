using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MoneyPopper : MonoBehaviour {

    [SerializeField]
    private Vector3 impulseDir = new Vector3(15.0f, 40.0f);
    [SerializeField]
    private float time = 2.0f;
    [SerializeField]
    private float drag = 5.0f;
    [SerializeField]
    private Color gain = Color.green;
    [SerializeField]
    private Color loss = Color.red;
    private Vector3 velocity = Vector3.zero;

    private Vector3 startPos;
	// Use this for initialization
	void Awake () {
        startPos = GetComponent<RectTransform>().localPosition;
	}

    public void Play(int money)
    {
        gameObject.SetActive(true);
        if(money > 0) {
            GetComponent<Text>().text = "+" + money;
            GetComponent<Text>().color = gain;
        }
        else
        {
            GetComponent<Text>().text = "-" + money;
            GetComponent<Text>().color = loss;
        }
        
        StopAllCoroutines();
        StartCoroutine(BounceSequence());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator BounceSequence()
    {
        velocity = impulseDir;
        GetComponent<RectTransform>().localPosition = startPos;
        float dt = 0.0f;
        while (dt < time)
        {
            yield return null;
            dt += Time.deltaTime;
            velocity -= velocity.normalized * Time.deltaTime * drag;
            GetComponent<RectTransform>().localPosition += velocity * Time.deltaTime;
        }
        gameObject.SetActive(false);
    }
}
