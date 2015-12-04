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
        StartCoroutine(PlayDelayed());
	}

    IEnumerator PlayDelayed()
    {
        yield return new WaitForSeconds(musicInit.clip.length);
        musicLoop.Play();
    }

	
	// Update is called once per frame
	void Update () 
	{
	}

}

