Shader "Hidden/PaintShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MousePosition ("Mouse Position", Vector) = (0, 0, 0, 0)
		_LastMousePosition ("Last Mouse Position", Vector) = (0, 0, 0, 0)
		_SecondLastMousePosition ("Second Last Mouse Position", Vector) = (0,0,0,0)
		_PaintDirection ("Paint Direction", Vector) = (0,0,0,0)
		_PointA ("Point A", Vector) = (0,0,0,0)
		_PointB ("Point B", Vector) = (0,0,0,0)
		_PointC ("Point C", Vector) = (0,0,0,0)
		_BrushColor ("Brush Color", Color) = (0,0,0,1)
		_BrushSize ("Brush Size", Float) = 0.0
	}

	CGINCLUDE

		#include "UnityCG.cginc"
			float _Timer;
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			float4 _MousePosition;
			float4 _LastMousePosition;
			float4 _SecondLastMousePosition;
			float4 _PaintDirection;
			float4 _BrushColor;
			float _BrushSize;
			float4 _PointA;
			float4 _PointB;
			float4 _PointC;
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 scrPos:TEXCOORD1;
				float4 vertex : SV_POSITION;

			};

			float2 project(float2 a, float2 b) 
			{
				return (dot(a, b) / dot(b, b)) * b;
			}

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				o.scrPos=ComputeScreenPos(o.vertex);
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float4 brushCol = _BrushColor;
				
				// Fill end circles
				float inBox = smoothstep(0.99, 0.995, (1 - length((i.uv.xy- _PointB).xy)));
				inBox = max(inBox, smoothstep(0.99, 0.995, (1 - length((i.uv.xy- _PointC).xy))));

				float2 rectF = _PointB -_PointC;
				float rLen = length(rectF);
				float2 rectU = float2(rectF.y, -rectF.x);

				float2 center = _PointC + rectF * 0.5;


				

				float2 uvPos = i.uv.xy - center;
				float uvDist = length(project(uvPos, normalize(rectF))) / length(rectF*0.5); // rectF
				uvDist = 1 - uvDist*uvDist;
				uvDist *= length(rectF) *length(rectF) * length(rectF) * 1.3 * step(0.05, length(rectF));
				

				//float2 prevF = _LastMousePosition - _SecondLastMousePosition;
				//float2 prevProj = project(float2(1,0), normalize(rectU));
				//prevProj = lerp(float2(0, 0), normalize(prevProj), )
				//prevF = lerp(float2(0, 0), prevF, step(0.5, length(prevF)));

				float2 offset = (_PaintDirection*uvDist);// * step(0.1, length(float2(0, 0)));//lerp(k1 * uvDist, k2 * uvDist, uvDist);
				//offset = normalize(offset) * clamp(length(offset), 0, 1);
				float2 posF = project(uvPos, normalize(rectF)); // 
				float2 posU = project(uvPos, normalize(rectU));

				// Fill Box in between
				inBox = max( inBox, step(0.01, rLen) * step(1 - length(rectF), 1 - length(posF) * 2) * smoothstep(0.99, 0.995, 1 - length(posU)) );
				
				float4 c = lerp(col, brushCol, inBox);

				return c;// length(i.uv.xy-_MousePosition.xy);
			}

			

			fixed4 fragBezier (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				fixed2 start = _PointA;
				fixed2 bezierPoint = _PointB;
				fixed2 end = _PointC;
				


				// Fill end circles
				float inBox = smoothstep(0.99, 0.995, (1 - length((i.uv.xy- start).xy)));
				inBox = max(inBox, smoothstep(0.99, 0.995, (1 - length((i.uv.xy- end).xy))));

				float2 rectF = end - start;
				float2 rectU = float2(rectF.y, -rectF.x);

				float2 center = start + rectF * 0.5;


				

				float2 uvPos = i.uv.xy - start;
				float2 uvProj = project(uvPos, normalize(rectF));
				float uvDist = length(uvProj) / length(rectF); // rectF


				float2 k1 = bezierPoint-start;
				float2 k2 = end-start;
				float2 h = k1 + k2;

				float2 hO = _PaintDirection;


				float2 offset = lerp(lerp(start, bezierPoint, uvDist) - start, lerp(bezierPoint, end, uvDist) - bezierPoint, uvDist);
				offset = project(-offset, hO);


				//float2 offset = hO * lerp(length(a), length(b), uvDist);



				float2 posF = project(uvPos + offset, normalize(rectF)); // 
				float2 posU = project(uvPos + offset, normalize(rectU));

				// Fill Box in between
				inBox = max( inBox,  step(0, length(rectF) - length(posF)) * step(length(rectF), length(rectF + posF)) * smoothstep(0.99, 0.995, 1 - length(posU)) );
				float4 brushCol = float4(0, 0, 0, 1);
				float4 c = lerp(col, brushCol, inBox);

				return c;// length(i.uv.xy-_MousePosition.xy);
			}

			fixed4 fragBezierSneaky (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				fixed2 start = _PointA;
				fixed2 bezierPoint = _PointB;
				fixed2 end = _PointC;
				


				// Fill end circles
				float inBox = smoothstep(0.99, 0.995, (1 - length((i.uv.xy- start).xy)));
				inBox = max(inBox, smoothstep(0.99, 0.995, (1 - length((i.uv.xy- end).xy))));

				float2 rectF = end - start;
				float2 rectU = float2(rectF.y, -rectF.x);

				float2 center = start + rectF * 0.5;


				

				float2 uvPos = i.uv.xy - start;
				float2 uvProj = project(uvPos, normalize(rectF));
				float uvDist = length(uvProj) / length(rectF); // rectF


				float2 k1 = bezierPoint-start;
				float2 k2 = end-start;
				float2 h = k1 + k2;

				float2 hO = _PaintDirection;


				float2 a = project(h*uvDist, normalize(k1));
				a = normalize(a) * clamp(length(a), 0, length(k1));
				a = project(a, hO);
				float2 b = project(h*uvDist, normalize(k2));
				b = normalize(b) * clamp(length(b), 0, length(k2));
				b -= project(b, hO);


				float2 offset = lerp(lerp(start, bezierPoint, uvDist) - start, lerp(bezierPoint, end, uvDist) - bezierPoint, uvDist);
				offset = project(-offset, hO);


				//float2 offset = hO * lerp(length(a), length(b), uvDist);



				float2 posF = project(uvPos + offset, normalize(rectF)); // 
				float2 posU = project(uvPos + offset, normalize(rectU));

				// Fill Box in between
				inBox = max( inBox,  step(0, length(rectF) - length(posF)) * step(length(rectF), length(rectF + posF)) * smoothstep(0.99, 0.995, 1 - length(posU)) );
				float4 brushCol = float4(0, 0, 0, 1);
				float4 c = lerp(col, brushCol, inBox);

				return c;// length(i.uv.xy-_MousePosition.xy);
			}

			struct lerpResult {
				float2 lerpRes;
				float2 lerpDir;
				float2 lastLerpA;
				float2 lastLerpB;
			};
			lerpResult bezierLerp(float2 a, float2 b, float2 c, float2 d, float2 e, float delta)
			{
				// lerp 1
				float2 lerp0 = lerp(a, b, delta);
				// lerp 2
				float2 lerp1 = lerp(b, c, delta);
				// lerp 3
				float2 lerp2 = lerp(c, d, delta);
				// lerp 4
				float2  lerp3 = lerp(d, e, delta);
					
				float2 lerp01 = lerp(lerp0, lerp1, delta);
				float2 lerp12 = lerp(lerp1, lerp2, delta);
				float2 lerp23 = lerp(lerp2, lerp3, delta);

				float2 lerp0112 = lerp(lerp01, lerp12, delta);
				float2 lerp1223 = lerp(lerp12, lerp23, delta);

				float2 tot = lerp(lerp0112, lerp1223, delta);
				lerpResult r;
				r.lerpRes = tot;
				r.lerpDir = normalize(lerp1223 - lerp0112);
				r.lastLerpA = lerp0112;
				r.lastLerpB = lerp1223;
				return r;
			}

			/*
			public double getClosestPointToCubicBezier(double fx, double fy, int slices, double x0, double y0, double x1, double y1, double x2, double y2, double x3, double y3)  {
				double tick = 1d / (double) slices;
				double x;
				double y;
				double t;
				double best = 0;
				double bestDistance = Double.POSITIVE_INFINITY;
				double currentDistance;
				for (int i = 0; i <= slices; i++) {
					t = i * tick;
					//B(t) = (1-t)**3 p0 + 3(1 - t)**2 t P1 + 3(1-t)t**2 P2 + t**3 P3
					x = (1 - t) * (1 - t) * (1 - t) * x0 + 3 * (1 - t) * (1 - t) * t * x1 + 3 * (1 - t) * t * t * x2 + t * t * t * x3;
					y = (1 - t) * (1 - t) * (1 - t) * y0 + 3 * (1 - t) * (1 - t) * t * y1 + 3 * (1 - t) * t * t * y2 + t * t * t * y3;

					currentDistance = Point.distanceSq(x,y,fx,fy);
					if (currentDistance < bestDistance) {
						bestDistance = currentDistance;
						best = t;
					}
				}
				return best;
			}	
			*/

			float2 projectOnBezier(float2 a, float2 b, float2 c, float2 d, float2 e, float2 p, uint slices)
			{
				float tickDelta = 1.0 / (float)slices;
				float2 best = float2(100, 100);
				float bestLen = 100;
				for (uint i = 0; i <= slices; i++) {
					lerpResult res = bezierLerp(a, b, c, d, e, tickDelta*(float)i);
					//if(length(res.lerpRes) < length(best))
					float select = step(length(p - res.lerpRes), bestLen);
					best = lerp(best, res.lerpRes, select);
					bestLen = length(p - best);
				}
				return best;
			}

			fixed4 fragBezierEndPoint (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				fixed2 start = _PointA;
				fixed2 end = _PointB;
				float brushSize = lerp(0.99, 0.5, _BrushSize);
				float4 brushCol = _BrushColor;

				

				float2 rectF = end - start;
				float2 rectU = float2(rectF.y, -rectF.x);
				fixed2 bezierPoint = rectF + normalize(_PointB - _PointC) * length(rectF)*0.2;
				float2 center = start + rectF * 0.5;


				

				float2 uvPos = i.uv.xy - start;
				float2 uvProj = project(uvPos, normalize(rectF));
				float uvDist = clamp(length(uvProj) / length(rectF), 0, 1); // rectF


				float2 hO = _PaintDirection;
				
				lerpResult lerpRes = bezierLerp(
					float2(0,0), // a
					rectF*0.5, // b
					rectF*0.5 + normalize(_PointB - _PointC) * length(_PointB - _PointC), //c
					bezierPoint, //d
					rectF, //e
					uvDist); //delta
				float2 projUvPos = projectOnBezier(
					float2(0, 0), // a
					rectF*0.5, // b
					bezierPoint - rectF*0.5, //c
					bezierPoint, //d
					rectF, // e
					uvPos,
					(uint)max((10000 * length(rectF)) * length(_PointB-_PointC), 8));
				float2 offset = lerpRes.lerpRes;
				float2 lerpDir = lerpRes.lerpDir;

				float2 uv_pos = uvPos - lerpRes.lastLerpA;
				float2 uv_lerpPos = project(uv_pos, lerpDir);
				float2 uv_lerpDist = uvPos - projUvPos;//uv_pos - uv_lerpPos;

				/*// lerp 1
				float2 lerp0 = lerp(float2(0,0), rectF*0.5, uvDist);
				// lerp 2
				float2 lerp1 = lerp(rectF*0.5, rectF*0.5 + normalize(_PointB - _PointC) * length(_PointB - _PointC), uvDist);
				// lerp 3
				float2 lerp2 = lerp(rectF*0.5 + normalize(_PointB - _PointC) * length(_PointB - _PointC), bezierPoint, uvDist);
				// lerp 4
				float2  lerp3 = lerp(bezierPoint, rectF, uvDist);
					
				float2 lerp01 = lerp(lerp0, lerp1, uvDist);
				float2 lerp23 = lerp(lerp2, lerp3, uvDist);
				offset = lerp(lerp01, lerp23, uvDist);

				float2 lerp0123U = -bezierPoint*uvDist;
				lerp0123U = float2(lerp0123U.y, -lerp0123U.x);*/
				//offset = project(-bezierPoint*uvDist, normalize(lerp0123U));

				//float2 offset = lerp(lerp(float2(0,0), bezierPoint * 0.5, uvDist ), lerp(bezierPoint, rectF, uvDist), uvDist);
				
				//offset = project(-offset, normalize(rectU));


				//float2 offset = hO * lerp(length(a), length(b), uvDist);



				float2 posF = project(uvPos, normalize(rectF)); // 
				float2 posU = project(uvPos, normalize(rectU));

				// Fill end circles
				float inBox = 0;
				inBox = lerp(
					max(inBox, (1 - step(length(rectF), length(posF - rectF))) * step(length(posF), length(rectF)) * smoothstep(brushSize, brushSize + 0.005, 1 - length(posU))),
					smoothstep(brushSize, brushSize + 0.005, (1 - length(uv_lerpDist))),
					step(0.02, length(rectF)));
				inBox = max(inBox, smoothstep(brushSize, brushSize + 0.005, (1 - length((i.uv.xy- start).xy))));
				inBox = max(inBox, smoothstep(brushSize, brushSize + 0.005, (1 - length((i.uv.xy- end).xy))));
				// Fill Box in between
				//inBox = max( inBox,  (1-step(length(rectF), length(posF - rectF))) * step(length(posF), length(rectF)) * smoothstep(brushSize, brushSize + 0.005, 1 - length(posU)) );
				//col float4(projUvPos.x, projUvPos.y, 0, 1)
				float4 c = lerp(col, brushCol, inBox);

				return c;// length(i.uv.xy-_MousePosition.xy);
			}

			fixed4 clear (v2f i) : SV_Target
			{
				return 1;
			}

	ENDCG

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		

		// Pass 0 - painting pass
		Pass
		{
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			
			
			ENDCG
		}

		// Pass1 - Clear pass
		Pass
		{
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment clear
			ENDCG
		}

		// Pass2 - Bezier pass
		Pass
		{
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment fragBezierEndPoint
			ENDCG
		}
		
	}
}
