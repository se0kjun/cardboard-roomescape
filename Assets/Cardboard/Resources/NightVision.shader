// Night Vision Shader Version 1.00 
// Ogre3D convertion
// adapted by Max, Yoda the rebel, 
// July 2012.

Shader "ImageEffects/NightVision" {

	Properties{
		_MainTex("Base (RGB)", RECT) = "white" {}
	}

	SubShader{
		Pass{
			ZTest Always Cull Off ZWrite Off
			Fog{ Mode off }

			CGPROGRAM

			#pragma target 3.0
			#pragma vertex vert_img
			#pragma fragment frag
			//#pragma fragmentoption ARB_precision_hint_fastest 
			#include "UnityCG.cginc"

			// frag shaders data
			uniform sampler2D _MainTex;
			uniform float4 lum;
			uniform float time;
			uniform float noiseFactor;

			// frag shader
			float4 frag(v2f_img i) : COLOR
			{

				float4 pix = tex2D(_MainTex, i.uv);

				//obtain luminence value
				pix = dot(pix,lum);

				// noise simulation
				float2 t = i.uv;
				float x = t.x *t.y * 123456 * time;
				x = fmod(x,13) * fmod(x,123);
				float dx = fmod(x,noiseFactor);
				float dy = fmod(x,noiseFactor);
				float4 c = tex2D(_MainTex, t + float2(dx,dy))*0.5;
				pix += c;

				//add lens circle effect
				//(could be optimised by using texture)
				//float dist = distance(i.uv, float2(0.5,0.5));
				//pix *= smoothstep(0.5,0.45,dist);

				//add rb to the brightest pixels
				pix.rb = max(pix.r - 0.75, 0) * 4;

				// return pix pixel ;)	
				return pix;
			}
			ENDCG
		}
	}
	Fallback off
}
