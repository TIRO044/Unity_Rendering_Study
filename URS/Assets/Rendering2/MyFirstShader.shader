Shader "Custom/MyFirstShader"
{
	Properties{
		_Tint("Tint", Color) = (1, 1, 1, 1)
	}

	SubShader{
		Pass{
			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"

			float4 _Tint;

			float4 MyVertexProgram(
				float4 position : POSITION,
				out float3 localPos : TEXCOORD0) : SV_POSITION {
					localPos = position.xyz;
					return UnityObjectToClipPos(position);
			}

			float4 MyFragmentProgram(
				float4 position: SV_POSITION,
				float3 localPos : TEXCOORD0) : SV_TARGET {
					return float4(localPos, 1);
			}

			ENDCG
		}
	}
}
