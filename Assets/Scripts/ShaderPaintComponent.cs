using UnityEngine;
using System.Collections;

public class ShaderPaintComponent : MonoBehaviour {
    private Material painterShader;
    private RenderTexture renderTex;
    private RenderTexture renderTex2;
    private RectTransform rect;
    private Vector2 lastMousePos = -Vector2.one;
    private Vector2 secondLastMousePos = -Vector2.one;
    //private MouseTracker tracker;

	// Use this for initialization
	void Start () {
        rect = GetComponent<RectTransform>();
        painterShader = new Material(Shader.Find("Hidden/PaintShader"));
        renderTex = new RenderTexture((int)GetComponent<RectTransform>().rect.width, (int)GetComponent<RectTransform>().rect.height, 24);
        renderTex2 = new RenderTexture((int)GetComponent<RectTransform>().rect.width, (int)GetComponent<RectTransform>().rect.height, 24);
        renderTex.wrapMode = TextureWrapMode.Clamp;
        renderTex2.wrapMode = TextureWrapMode.Clamp;
        gameObject.GetComponent<UnityEngine.UI.RawImage>().texture = renderTex;
        Clear();
        //tracker = new MouseTracker();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (Input.GetMouseButton(0)) // && !Mathf.Approximately(Input.mousePosition.magnitude, lastMousePos.magnitude))
        {
            Vector2 pos;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out pos)) return;

            pos.y = rect.rect.height - Mathf.Abs(pos.y);
            pos.y /= rect.rect.height;
            pos.x /= rect.rect.width;

            Vector2 diff = pos - lastMousePos;
            Debug.Log(diff.magnitude);
            if (diff.magnitude > 0.015)
            {
                RenderBezier(pos);
            } else
            {
                //RenderStraight(pos);
            }



        }
        else if (Input.GetMouseButtonUp(0))
        {
            lastMousePos = -Vector2.one;
            secondLastMousePos = -Vector2.one;
        }

        if (Input.mouseScrollDelta.magnitude > 0.0f)
        {
            float size = painterShader.GetFloat("_BrushSize");
            painterShader.SetFloat("_BrushSize", Mathf.Clamp01(size + (Input.mouseScrollDelta.y < 0.0f ? -0.05f : 0.05f)));
        }

        if (Input.GetMouseButtonDown(1))
        {
            Clear();
        }
        
	}

    void Render()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out pos);

        pos.y = rect.rect.height - Mathf.Abs(pos.y);
        pos.y /= rect.rect.height;
        pos.x /= rect.rect.width;
        Debug.Log(pos);
        Vector2 lpos = (lastMousePos == -Vector2.one ? pos : lastMousePos);
        Vector2 llPos = (secondLastMousePos == -Vector2.one ? lastMousePos : secondLastMousePos) - lpos;

        Vector3 paintDir = Vector3.Project(new Vector3(llPos.x, llPos.y, 0), new Vector3(lpos.y - pos.y, -(lpos.x - pos.x)).normalized);
        if (Mathf.Approximately(paintDir.magnitude, 0))
            paintDir = Vector3.zero;
        else paintDir.Normalize();

        //Vector3.Dot(new Vector3(llPos.x, llPos.y).normalized, new Vector3(lpos.x, lpos.y).normalized);
        paintDir *= Vector3.Dot(new Vector3(llPos.x, llPos.y).normalized, new Vector3(lpos.x - pos.x, lpos.y - pos.y).normalized);

        Debug.Log(": " + paintDir);
        painterShader.SetVector("_PaintDirection", paintDir);
        painterShader.SetVector("_LastMousePosition", lpos);
        //painterShader.SetVector("_SecondLastMousePosition", secondLastMousePos);//(secondLastMousePos == -Vector2.one ? (lastMousePos == -Vector2.one ? pos : lastMousePos) : secondLastMousePos));
        painterShader.SetVector("_MousePosition", new Vector4(pos.x, pos.y));

        if (llPos != -Vector2.one)
        {
            painterShader.SetVector("_PointA", llPos);
            painterShader.SetVector("_PointB", lpos);
            painterShader.SetVector("_PointC", pos);

            Graphics.Blit(renderTex2, renderTex, painterShader, 2);
            Graphics.Blit(renderTex, renderTex2);
            llPos = -Vector3.one;
            lpos = -Vector3.one;
        }
        else if (pos != lastMousePos)
        {

            secondLastMousePos = lastMousePos;
            lastMousePos = pos;
        }
    }

    void RenderStraight(Vector2 pos)
    {
        if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1) return;
        if (secondLastMousePos != -Vector2.one)
        {
            Vector3 paintDir = Vector3.Project(new Vector3(secondLastMousePos.x, secondLastMousePos.y, 0), new Vector3(lastMousePos.y - pos.y, -(lastMousePos.x - pos.x)).normalized);
            if (Mathf.Approximately(paintDir.magnitude, 0))
                paintDir = Vector3.zero;
            else paintDir.Normalize();
            Debug.Log(": " + paintDir);
            painterShader.SetVector("_PaintDirection", paintDir);

            painterShader.SetVector("_PointA", secondLastMousePos);
            painterShader.SetVector("_PointB", lastMousePos);
            painterShader.SetVector("_PointC", pos);

            Graphics.Blit(renderTex2, renderTex, painterShader, 0);
            Graphics.Blit(renderTex, renderTex2);
        }
        secondLastMousePos = lastMousePos;
        lastMousePos = pos;
    }

    void RenderBezier(Vector2 pos)
    {
        
        if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1) return;
        if (secondLastMousePos != -Vector2.one)
        {
            Vector3 paintDir = Vector3.Project(new Vector3(secondLastMousePos.x, secondLastMousePos.y, 0), new Vector3(lastMousePos.y - pos.y, -(lastMousePos.x - pos.x)).normalized);
            if (Mathf.Approximately(paintDir.magnitude, 0))
                paintDir = Vector3.zero;
            else paintDir.Normalize();
            Debug.Log(": " + paintDir);
            painterShader.SetVector("_PaintDirection", paintDir);

            painterShader.SetVector("_PointA", secondLastMousePos);
            painterShader.SetVector("_PointB", lastMousePos);
            painterShader.SetVector("_PointC", pos);

            Graphics.Blit(renderTex2, renderTex, painterShader, 2);
            Graphics.Blit(renderTex, renderTex2);
        }
        secondLastMousePos = lastMousePos;
        lastMousePos = pos;
    }

    void RenderBezier2()
    {
        Vector2 pos;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out pos)) return;

        pos.y = rect.rect.height - Mathf.Abs(pos.y);
        pos.y /= rect.rect.height;
        pos.x /= rect.rect.width;
        if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1) return;
        if (secondLastMousePos != -Vector2.one)
        {
            Vector3 paintDir = Vector3.Project(new Vector3(secondLastMousePos.x, secondLastMousePos.y, 0), new Vector3(lastMousePos.y - pos.y, -(lastMousePos.x - pos.x)).normalized);
            if (Mathf.Approximately(paintDir.magnitude, 0))
                paintDir = Vector3.zero;
            else paintDir.Normalize();
            Debug.Log(": " + paintDir);
            painterShader.SetVector("_PaintDirection", paintDir);

            painterShader.SetVector("_PointA", secondLastMousePos);
            painterShader.SetVector("_PointB", lastMousePos);
            painterShader.SetVector("_PointC", pos);

            Graphics.Blit(renderTex2, renderTex, painterShader, 2);
            Graphics.Blit(renderTex, renderTex2);
            secondLastMousePos = -Vector3.one;
            lastMousePos = -Vector3.one;
        }
        secondLastMousePos = lastMousePos;
        lastMousePos = pos;
    }
    
    void Clear()
    {
        Graphics.Blit(renderTex2, renderTex, painterShader, 1);
        Graphics.Blit(renderTex, renderTex2);
    }
}
