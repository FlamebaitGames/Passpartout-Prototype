using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class PaintComponent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler {
    public Texture2D canvas;
    public Camera cam;
	public AudioSource paintSound1;
	public AudioSource paintSound2;
	public AudioSource paintSound3;
	public AudioSource paintSound4;
	public bool fadeIn;
	public bool fadeOut;
	public int randomSound;

    public int brushSize = 16;

    public Color currentColor = Color.black;
    private Vector2 lastDragPosition = Vector2.zero;

    public enum PaletteColors
    {
        YELLOW,
        GREEN,
        BLUE,
        PURPLE,
        RED,
        BROWN,
        WHITE,
        BLACK
    }

    private int paletteIndex = (int)PaletteColors.BLACK;

    // Palette Colors
    public Color C_YELLOW;
    public Color C_GREEN;
    public Color C_BLUE;
    public Color C_PURPLE;
    public Color C_RED;
    public Color C_BROWN;
    public Color C_WHITE;
    public Color C_BLACK;

	// Use this for initialization
	void Start () {
        // Setup the color palette.


        var original = gameObject.GetComponent<UnityEngine.UI.RawImage>().mainTexture as Texture2D;
		fadeIn = false;
		fadeOut = false;
		paintSound1.volume = 0;
		paintSound2.volume = 0;
		paintSound3.volume = 0;
		paintSound4.volume = 0;



        canvas = new Texture2D((int)GetComponent<RectTransform>().rect.width, (int)GetComponent<RectTransform>().rect.height);
        gameObject.GetComponent<UnityEngine.UI.RawImage>().texture = canvas;

        for (int y = 0; y < canvas.height; ++y)
        {
            for (int x = 0; x < canvas.width; ++x)
            {
                canvas.SetPixel(x, y, Color.white);
            }
        }
        canvas.Apply();
    }

    public void OnPointerDown(PointerEventData data)
    {
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
		randomSound = (Random.Range (1, 4));

		if (paintSound1.isPlaying == true)
		{
			randomSound = 1;
		}

		if (paintSound2.isPlaying == true)
		{
			randomSound = 2;
		}

		if (paintSound3.isPlaying == true)
		{
			randomSound = 3;
		}

		if (paintSound4.isPlaying == true)
		{
			randomSound = 4;
		}

		if (paintSound1.isPlaying == false && randomSound == 1) 
		{
			paintSound1.Play ();
		}

		if (paintSound2.isPlaying == false && randomSound == 2) 
		{
			paintSound2.Play ();
		}

		if (paintSound3.isPlaying == false && randomSound == 3) 
		{
			paintSound3.Play ();
		}

		if (paintSound4.isPlaying == false && randomSound == 4) 
		{
			paintSound4.Play ();
		}

		fadeOut = false;
		fadeIn = true;

    }
    public void OnDrag(PointerEventData data)
    {
        return;
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, data.pointerCurrentRaycast.screenPosition, data.pressEventCamera, out pos);

        pos.y = rect.rect.height - Mathf.Abs(pos.y);
        pos.y /= rect.rect.height / canvas.height;
        pos.x /= rect.rect.width / canvas.width;

        if (lastDragPosition == Vector2.zero)
        {
            DrawCircle(canvas, (int)pos.x, (int)pos.y, 64, Color.black);
        }
        else
        {
            int dx = (int)pos.x - (int)lastDragPosition.x;
            int dy = (int)pos.y - (int)lastDragPosition.y;

            for (int i = 1; i <= 8; i++)
            {
                int x = (int)lastDragPosition.x + dx * (1 / i);
                int y = (int)lastDragPosition.y + dy * (1 / i);
                DrawCircle(canvas, x, y, 64, Color.black);
            }
        }
        
        canvas.Apply();

        lastDragPosition = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
		fadeIn = false;
		fadeOut = true;

    }

	// Update is called once per frame
	void Update () {
        float d = Input.GetAxis("Mouse ScrollWheel");
        if (d < 0.0f)
        {
            brushSize--;
        }
        else if (d > 0.0f)
        {
            brushSize++;
        }

        if (brushSize < 4) brushSize = 4;
        if (brushSize > 32) brushSize = 32;

		if (fadeIn == true) 
		{
			if (paintSound1.volume < 0.8)
			{
				paintSound1.volume = (paintSound1.volume + 0.03f);
			}

			if (paintSound2.volume < 0.8)
			{
				paintSound2.volume = (paintSound2.volume + 0.03f);
			}

			if (paintSound3.volume < 0.8)
			{
				paintSound3.volume = (paintSound3.volume + 0.03f);
			}

			if (paintSound4.volume < 0.8)
			{
				paintSound4.volume = (paintSound4.volume + 0.03f);
			}

			else
			{
				fadeIn = false;
			}
		
			/*if(paintSound1.volume >= 0.4)
			{
				fadeIn = false;
			}
			*/

		}

		if (fadeOut == true) 
		{
			if (paintSound1.volume > 0)
			{
				paintSound1.volume = (paintSound1.volume - 0.03f);
			}

			if (paintSound2.volume > 0)
			{
				paintSound2.volume = (paintSound2.volume - 0.03f);
			}

			if (paintSound3.volume > 0)
			{
				paintSound3.volume = (paintSound3.volume - 0.03f);
			}

			if (paintSound4.volume > 0)
			{
				paintSound4.volume = (paintSound4.volume - 0.03f);
			}

			else
			{
				fadeOut = false;
				paintSound1.Stop();
				paintSound2.Stop();
				paintSound3.Stop();
				paintSound4.Stop();

			}
		}


        if (Input.GetMouseButton(0))
        {
            RectTransform rect = GetComponent<RectTransform>();
            Rect r = GetComponent<RectTransform>().rect;

            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out pos);

            pos.y = rect.rect.height - Mathf.Abs(pos.y);
            pos.y /= rect.rect.height / canvas.height;
            pos.x /= rect.rect.width / canvas.width;

            if (lastDragPosition == Vector2.zero)
            {
                DrawCircle(canvas, (int)pos.x, (int)pos.y, brushSize, Color.black);
            }
            else
            {
                int dx = (int)pos.x - (int)lastDragPosition.x;
                int dy = (int)pos.y - (int)lastDragPosition.y;

                for (int i = 1; i <= 8; i++)
                {
                    int x = (int)lastDragPosition.x + dx * (1 / i);
                    int y = (int)lastDragPosition.y + dy * (1 / i);
                    DrawCircle(canvas, x, y, brushSize, currentColor);
                }
            }

            canvas.Apply();

            lastDragPosition = pos;
        }
        else
        {
            lastDragPosition = Vector2.zero;
        }
	}


    public void DrawCircle(Texture2D tex, int cx, int cy, int r, Color col)
    {
        Color32[] pixels = canvas.GetPixels32();

        int x, y, px, nx, py, ny, d;

        for (x = 0; x <= r; x++)
        {
            d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
            for (y = 0; y <= d; y++)
            {
                px = cx + x;
                nx = cx - x;
                py = cy + y;
                ny = cy - y;


                pixels[py * canvas.width + px] = col;
                pixels[py * canvas.width + nx] = col;
                pixels[ny * canvas.width + px] = col;
                pixels[ny * canvas.width + nx] = col;

                /*
                tex.SetPixel(px, py, col);
                tex.SetPixel(nx, py, col);

                tex.SetPixel(px, ny, col);
                tex.SetPixel(nx, ny, col);
                */
            }
        }
        tex.SetPixels32(pixels);
    }

    public Color GetCurrentColor(PaletteColors color)
    {
        switch (color)
        {
            case PaletteColors.YELLOW:  return C_YELLOW;
            case PaletteColors.GREEN:   return C_GREEN;
            case PaletteColors.BLUE:    return C_BLUE;
            case PaletteColors.PURPLE:  return C_PURPLE;
            case PaletteColors.RED:     return C_RED;
            case PaletteColors.BROWN:   return C_BROWN;
            case PaletteColors.WHITE:   return C_WHITE;
            case PaletteColors.BLACK:   return C_BLACK;
            default: return C_BLACK;
        }
    }

    public void SetCurrentColor(int c)
    {
        currentColor = GetCurrentColor((PaletteColors)c);
    }
}
