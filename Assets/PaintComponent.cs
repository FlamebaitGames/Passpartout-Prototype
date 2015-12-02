﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class PaintComponent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler {
    public Texture2D canvas;
    public Camera cam;

    public Color currentColor = Color.black;
    private Vector2 lastDragPosition = Vector2.zero;

    enum PaletteColors
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
    }

	// Update is called once per frame
	void Update () {
        float d = Input.GetAxis("Mouse ScrollWheel");

        if (d > 0.0f)
        {
            paletteIndex++;
            if (paletteIndex > 7) paletteIndex = 0;
        } else if (d < 0.0f)
        {
            paletteIndex--;
            if (paletteIndex < 0) paletteIndex = 7;
        }

        currentColor = GetCurrentColor((PaletteColors)paletteIndex);


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentColor = Color.black;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentColor = Color.blue;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentColor = Color.green;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentColor = Color.white;
        }




        if (Input.GetMouseButton(0))
        {
            RectTransform rect = GetComponent<RectTransform>();
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out pos);

            pos.y = rect.rect.height - Mathf.Abs(pos.y);
            pos.y /= rect.rect.height / canvas.height;
            pos.x /= rect.rect.width / canvas.width;

            if (lastDragPosition == Vector2.zero)
            {
                DrawCircle(canvas, (int)pos.x, (int)pos.y, 16, Color.black);
            }
            else
            {
                int dx = (int)pos.x - (int)lastDragPosition.x;
                int dy = (int)pos.y - (int)lastDragPosition.y;

                for (int i = 1; i <= 8; i++)
                {
                    int x = (int)lastDragPosition.x + dx * (1 / i);
                    int y = (int)lastDragPosition.y + dy * (1 / i);
                    DrawCircle(canvas, x, y, 16, currentColor);
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

    private Color GetCurrentColor(PaletteColors color)
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
}
