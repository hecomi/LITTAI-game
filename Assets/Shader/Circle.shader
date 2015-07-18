Shader "Grid/Circle" {
	Properties {
        _Color("Color", Color) = (0.5, 0.5, 0.5, 1.0)
        _Width("Width", Range(0.0, 1.0)) = 0.5
        _CenterX("CneterX", Float) = 0.5
        _CenterY("CneterY", Float) = 0.5
        _Speed("Speed", Float) = 1.0
    }
    SubShader {
    	Tags { "Queue" = "Transparent" } 
        Pass {
        	Name "AnimCircle"
        	
      		ZWrite On
      		Cull Back
      		Blend SrcAlpha OneMinusSrcAlpha
      		
            CGPROGRAM
            
            #include "UnityCG.cginc"
            #pragma vertex vert_img
            #pragma fragment frag
            
            uniform half4 _Color;
            uniform half _Width;
            uniform half _CenterX;
            uniform half _CenterY;
            uniform half _Speed;

            half4 frag(v2f_img i) : COLOR {
            	half x  = i.uv.x;
            	half y  = i.uv.y;
            	half circle = 0;
            	for (int i = 0; i < 6; ++i) {
	            	half r0 = -1 + 3 * fmod((_Time.y * _Speed + 0.5 * i) / 3, 1);
	            	half r = 2 * sqrt(pow(x - _CenterX, 2) + pow(y - _CenterY, 2));
            		circle += clamp(pow((_Width * 0.1) / abs(r - r0), 1), 0.0, 3.0);
            	}
            	
            	half3 col = circle * _Color;
                return half4(col, circle * _Color.a);
            }

            ENDCG
        }
    }
}