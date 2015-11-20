using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine.UI;
using System.Linq;

public class DialogBubble : MonoBehaviour {

	Ray ray;
	RaycastHit hit;
	public GameObject vCurrentBubble = null; //just to make sure we cannot open multiple bubble at the same time.
	public bool IsTalking = false;
	public List<PixelBubble> vBubble = new List<PixelBubble>();
    public Transform orientor;
	private PixelBubble vActiveBubble = null;
    

	//show the right bubble on the current character
	public void ShowBubble()
	{
		bool gotonextbubble = false;

		//if vcurrentbubble is still there, just close it
		if (vActiveBubble != null) {
			if (vActiveBubble.vClickToCloseBubble) {
				//get the function to close bubble
				Appear vAppear = vCurrentBubble.GetComponent<Appear> ();
				vAppear.valpha = 0f;
				vAppear.vTimer = 0f; //instantly
				vAppear.vchoice = false; //close bubble
				
				//check if last bubble
				if (vActiveBubble == vBubble.Last ())
					IsTalking = false;
			}
		}
		
		foreach (PixelBubble vBub in vBubble)
		{
			//make sure the bubble isn't already opened
			if (vCurrentBubble == null)
			{
				//make the character in talking status
				IsTalking = true;
				
				//cut the message into 24 characters
				string vTrueMessage = "";
				string cLine = "";
				int vLimit = 24;
                if (vBub.vMessageForm == BubbleType.Round)
					vLimit = 16;
				
				//cut each word in a text in 24 characters.
                foreach (string vWord in vBub.vMessage.Split(' '))
				{
					if (cLine.Length + vWord.Length > vLimit)
					{
						vTrueMessage += cLine+System.Environment.NewLine;  
						
						//add a line break after
						cLine = ""; //then reset the current line
					}
					
					//add the current word with a space
					cLine += vWord+" ";
				}
				
				//add the last word
				vTrueMessage += cLine;
				GameObject vBubbleObject = null;
                Vector3 offset;
				//create a rectangle or round bubble
                if (vBub.vMessageForm == BubbleType.Rectangle)
				{
					//create bubble
					vBubbleObject = Instantiate(Resources.Load<GameObject> ("Customs/BubbleRectangle"));
					offset = new Vector3(1.35f, 1.9f, 0f); //move a little bit the teleport particle effect
				}
				else 
				{
					//create bubble
					vBubbleObject = Instantiate(Resources.Load<GameObject> ("Customs/BubbleRound"));
                    offset = new Vector3(0.15f, 1.75f, 0f); //move a little bit the teleport particle effect
				}

				//show the mouse and wait for the user to left click OR NOT (if not, after 10 sec, it disappear)
                vBubbleObject.GetComponent<Appear>().needtoclick = vBub.vClickToCloseBubble;

                Color vNewBodyColor = new Color(vBub.vBodyColor.r, vBub.vBodyColor.g, vBub.vBodyColor.b, 0f);
                Color vNewBorderColor = new Color(vBub.vBorderColor.r, vBub.vBorderColor.g, vBub.vBorderColor.b, 0f);
                Color vNewFontColor = new Color(vBub.vFontColor.r, vBub.vFontColor.g, vBub.vFontColor.b, 255f);
				
				//get all image below the main Object
				foreach (Transform child in vBubbleObject.transform)
				{
					SpriteRenderer vRenderer = child.GetComponent<SpriteRenderer> ();
					TextMesh vTextMesh = child.GetComponent<TextMesh> ();
					
					if (vRenderer != null && child.name.Contains("Body"))
					{
						//change the body color
						vRenderer.color = vNewBodyColor;
						
						if (vRenderer.sortingOrder < 10)
							vRenderer.sortingOrder = 1500;
					}
					else if (vRenderer != null && child.name.Contains("Border"))
					{
						//change the border color
						vRenderer.color = vNewBorderColor;
						if (vRenderer.sortingOrder < 10)
							vRenderer.sortingOrder = 1501;
					} 
					else if (vTextMesh != null && child.name.Contains("Message"))
					{
						//change the message and show it in front of everything
						vTextMesh.color = vNewFontColor;
						vTextMesh.text = vTrueMessage;
						child.GetComponent<MeshRenderer>().sortingOrder = 1550;
						
						Transform vMouseIcon = child.FindChild("MouseIcon");
                        if (vMouseIcon != null && !vBub.vClickToCloseBubble)
							vMouseIcon.gameObject.SetActive(false);
					}
					
					//disable the mouse icon because it will close by itself
                    if (child.name == "MouseIcon" && !vBub.vClickToCloseBubble)
						child.gameObject.SetActive(false);
					else
                        vActiveBubble = vBub; //keep the active bubble and wait for the Left Click
				}
				
				vCurrentBubble = vBubbleObject; //attach it to the player
				vBubbleObject.transform.parent = transform; //make him his parent

                vBubbleObject.transform.rotation = Camera.main.transform.rotation;
            }
            else if (vActiveBubble == vBub && vActiveBubble.vClickToCloseBubble)
			{
				gotonextbubble = true;
				vCurrentBubble = null;
			}
		}
	}	

	void Update () 
	{
        if(vCurrentBubble != null)
            vCurrentBubble.transform.position = Camera.main.transform.position + (transform.position - Camera.main.transform.position).normalized * 3.0f;

		//can't have a current character 
		if (!IsTalking)
		{			
			vActiveBubble = null;
		}
	}
}
