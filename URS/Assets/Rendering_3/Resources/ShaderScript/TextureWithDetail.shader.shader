Shader "Custom/Texture With Detail"
{
	Properties{
		_Tint ("Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Texture", 2D) = "white" {}
		_DetailTex ("Detail Texture", 2D) = "gray" {}

		_Bright1 ("Bright1", Range(0.0, 1.0)) = 1
		_Bright2 ("Bright2", Range(0.0, 1.0)) = 1
	}

	SubShader{
		Pass{
			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"

			float4 _Tint;
			float4 _MainTex_ST;

			float _Bright1;
			float _Bright2;

			sampler2D _MainTex;
			sampler2D _DetailTex;
			float4 _MainTex_ST, _DetailTex_ST;

			struct VertexData {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Interpolators {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			Interpolators MyVertexProgram(VertexData v) {
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position.xyz);
				i.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return i;
			}

			float4 MyFragmentProgram(Interpolators i) : SV_TARGET {
				float4 color = tex2D(_MainTex, i.uv) * _Bright1;
				color *= tex2D(_MainTex, i.uv * 10) * _Bright2;
				return color;
			}

			ENDCG
		}
	}
}
