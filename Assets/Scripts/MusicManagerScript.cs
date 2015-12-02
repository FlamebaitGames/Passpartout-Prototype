using UnityEngine;
using System.Collections;

public class MusicManagerScript : MonoBehaviour
{
	public AudioSource musicLoop;
	public AudioSource musicInit;
	
	// Use this for initialization
	void Start () 
	{
		musicInit.Play();
		musicLoop.PlayDelayed(musicInit.clip.length);
	}
	

	
	// Update is called once per frame
	void Update () 
	{
	}

}

