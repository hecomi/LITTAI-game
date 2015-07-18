Shader "Particle/AdditiveZWrite" {
	Properties {
		_TintColor ("Tint Color", Color) = (1, 1, 1, 0.5)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Pass {
      		
      		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
			Blend SrcAlpha One
			AlphaTest Greater .01
			ColorMask RGB
			Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
      		ZWrite On
			BindChannels {
				Bind "Color", color
				Bind "Vertex", vertex
				Bind "TexCoord", texcoord
			}
      		
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform half4 _TintColor;

			float4 frag(v2f_img i) : COLOR {
				return _TintColor * tex2D(_MainTex, i.uv);
			}
			ENDCG
		}
	}
}