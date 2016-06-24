Shader "Custom/ObjectHighlight" 
{
	Properties
	{
		[Header(Normal Unity Texture Settings)]
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		[Space]
		[Header(Object Highlight Settings)]
		[Toggle]_UseHighlights("Enable Highlights", Float) = 1
		[Toggle]_UseFalloff("Use Smooth Falloff", Float) = 1
		_HighlightsWidth("Highlights Width", Range(0,1)) = 1
		_HighlightsColor("HighlightsColor",Color) = (1,0,0,1)
	}
	SubShader
	{
		Tags
		{ 
			"RenderType" = "Opaque" 
			"Queue" = "Geometry" 
		}

		LOD 200

		// Pass 1 Settings
		ZTest Greater // Draw only if occluded by other object
		ZWrite Off  // Dont write into ZBuffer

		// Pass 1, Draw Highlights
		CGPROGRAM
		#pragma surface surf Lambert noshadow alpha:fade keepalpha
		#pragma target 3.0
		struct Input 
		{
			float2 uv_MainTex;
			float3 worldPos;
		};

		fixed4 _HighlightsColor;
		half _HighlightsWidth;
		bool _UseHighlights;
		bool _UseFalloff;
		
		half4 LightingUnlit(SurfaceOutput s, half3 lightDir, half atten) 
		{
			half4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}

		void surf(Input IN, inout SurfaceOutput o) 
		{
			// is this effect active?
			if(_UseHighlights)
			{
				// Calculate Proximity to edge
				half rim = 1 - saturate(abs(dot(normalize(_WorldSpaceCameraPos - IN.worldPos), normalize(o.Normal))));

				// Take color from chams color
				fixed4 c = _HighlightsColor;
				o.Albedo = c;
				// decide for a drawing method
				if(_UseFalloff)
					o.Alpha = (rim - (1 - _HighlightsWidth * 2));
				else 
					o.Alpha = (rim + _HighlightsWidth) > 1 ? 1 : 0;
			}
			// it's not
			else
			{
				// completely transparent
				o.Albedo = 0;
				o.Alpha = 0;
			}
		}
		ENDCG // End Pass 1

		// Pass 2 Settings
		ZTest LEqual // Do not draw when occluded
		Zwrite On // write to ZBuffer
		Tags
		{ 
			"RenderType" = "Opaque" "Queue" = "Geometry" 
		}
		// Pass 2, Render Object as needed (Standard Surface shader...)
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
			float3 viewDir;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		
		void surf(Input IN, inout SurfaceOutputStandard o) 
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG // End Pass 2
	}
	FallBack "Diffuse"
}