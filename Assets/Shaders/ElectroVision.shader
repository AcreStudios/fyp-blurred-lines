Shader "Hidden/ElectroVision"
{
	SubShader
	{
		Tags
		{
			"Queue" = "Background"
		}
		LOD 100
		ZTest Always
		ZWrite Off
		Blend One One

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				return o;
			}

			half4 _VisionColor;

			fixed4 frag(v2f i) : SV_Target
			{
				return _VisionColor;
			}
			ENDCG
		}
	}
}
