Shader "Custom/TextureZTest"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		//_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Glossiness("Smoothness", Range(0,1)) = 0.5
	}
	Category 
	{
		SubShader 
		{ 
			Tags { "RenderType" = "Opaque" }

			Pass
			{
				ZTest Greater
				Lighting Off
				Color [_Color]
			}
			Pass 
			{
				ZTest Less          
				Color[_Color]
				//SetTexture [_MainTex] {combine texture}
			}

			CGPROGRAM

			#pragma surface surf Standard fullforwardshadows

			#pragma target 3.0

			//sampler2D _MainTex;

			struct Input
			{
				float2 uv_MainTex;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				// Albedo comes from a texture tinted by color
				// fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = _Color; //c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = _Color.a; //c.a;
			}

			ENDCG
		}
	}
	FallBack "Specular", 1
}