﻿// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

Shader "Projector/Light" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_ShadowTex("Cookie", 2D) = "" {}
	}

		Subshader{
		Pass{
		ZWrite Off
		Fog{ Color(0, 0, 0) }
		ColorMask RGBA
		Blend DstColor One
		Offset -1, -1

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
		struct v2f {
		float4 uvShadow : TEXCOORD0;
		float4 pos : SV_POSITION;
	};
	float4x4 unity_Projector;
	float4x4 unity_ProjectorClip;
	v2f vert(float4 vertex : POSITION)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, vertex);
		o.uvShadow = mul(unity_Projector, vertex);
		return o;
	}

	fixed4 _Color;
	sampler2D _ShadowTex;
	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 texS = tex2D(_ShadowTex, UNITY_PROJ_COORD(i.uvShadow));
	texS.rgb *= _Color.rgb;
	texS.a = 1.0 - texS.a;

	fixed4 res = texS;
	return res;
	}
		ENDCG
	}
	}
}