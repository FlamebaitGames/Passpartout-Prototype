using UnityEngine;
using System.Collections;

public class PricingSoundScript : MonoBehaviour {

	public AudioClip[] typeSound;
	public AudioClip[] enterSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayType()
	{
		if (typeSound.Length == 0) return;
		GetComponent<AudioSource>().PlayOneShot(typeSound[Random.Range(0, typeSound.Length - 1)]);
	}

	public void PlayEnter()
	{
		if (enterSound.Length == 0) return;
		GetComponent<AudioSource>().PlayOneShot(enterSound[Random.Range(0, enterSound.Length - 1)]);
	}
}
