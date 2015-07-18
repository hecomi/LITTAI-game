Shader "Custom/UvChange" {
	Properties {
 		_MainTex ("", 2D) = "white" {}
	}
	
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		Pass {
			// Draw Black
		}
		Pass {
			CGPROGRAM
			
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			
			uniform sampler2D _MainTex;
			
			v2f_img vert(appdata_base v) {
				v2f_img o;
				
				v.vertex.x += fmod(v.texcoord.x + 1, 2) * fmod(v.texcoord.y + 1, 2) * 0.3;
				v.vertex.x -= fmod(v.texcoord.x + 0, 2) * fmod(v.texcoord.y + 1, 2) * 0.3;
				v.vertex.x += fmod(v.texcoord.x + 1, 2) * fmod(v.texcoord.y + 0, 2) * 0.1;
				v.vertex.x -= fmod(v.texcoord.x + 0, 2) * fmod(v.texcoord.y + 0, 2) * 0.1;
				
				v.vertex.y += fmod(v.texcoord.x + 1, 2) * fmod(v.texcoord.y + 1, 2) * 0.3;
				v.vertex.y += fmod(v.texcoord.x + 0, 2) * fmod(v.texcoord.y + 1, 2) * 0.3;
				v.vertex.y -= fmod(v.texcoord.x + 1, 2) * fmod(v.texcoord.y + 0, 2) * 0.1;
				v.vertex.y -= fmod(v.texcoord.x + 0, 2) * fmod(v.texcoord.y + 0, 2) * 0.1;
				
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;
				return o;
			}
			
			half4 frag(v2f_img i) : COLOR {
				return tex2D(_MainTex, i.uv);
			}
			
			ENDCG
		}
	} 
	Fallback "Diffuse"
}
