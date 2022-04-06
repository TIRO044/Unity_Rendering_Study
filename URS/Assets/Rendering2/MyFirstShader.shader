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

			struct Interpolators {
				float4 position : SV_POSITION;
				float3 localPosition : TEXCOORD0;
			};

			Interpolators MyVertexProgram(float4 position : POSITION) {
				Interpolators i;
				i.position = UnityObjectToClipPos(position);
				i.localPosition = position.xyz;

				return i;
			}

			float4 MyFragmentProgram(Interpolators i) : SV_TARGET {
					return float4(i.localPosition, 1);
			}

			ENDCG
		}
	}
}
