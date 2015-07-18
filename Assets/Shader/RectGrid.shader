Shader "Grid/Rect" {
	Properties {
        _Color("Color", Color) = (0.5, 0.5, 0.5, 1.0)
        _Width("Width", Range(0.0, 1.0)) = 0.5
        _GridNumX("GridNumX", Int) = 10
        _GridNumY("GridNumY", Int) = 10
        _CenterX("CneterX", Float) = 0.5
        _CenterY("CneterY", Float) = 0.5
    }
    SubShader {
	    Tags { "Queue" = "Transparent" } 
        Pass {
        	Name "AnimCircle"
        	
      		Blend SrcAlpha One
      		ZWrite On
      		Cull Off
      		
            CGPROGRAM
            
            #include "UnityCG.cginc"
            #pragma vertex vert_img
            #pragma fragment frag
            
            uniform half4 _Color;
            uniform half _Width;
            uniform int _GridNumX;
            uniform int _GridNumY;
            uniform half _CenterX;
            uniform half _CenterY;

            half4 frag(v2f_img i) : COLOR {
            	half x  = i.uv.x;
            	half y  = i.uv.y;
            	
            	half t = _Time.y;
            	half r0 = -1 + 2 * fmod(t / 3, 1);
            	half aspect = half(_GridNumY) / _GridNumX * 0.5;
            	half r = sqrt(pow(x - _CenterX, 2) + aspect * pow(y - _CenterY, 2));
            	half circle = clamp(pow(0.1 / abs(r - r0), 1), 0.0, 3.0);
            	
            	half x0 = fmod(x * _GridNumX, 1.0);
            	half y0 = fmod(y * _GridNumY, 1.0);
            	half p  = pow(1 / _Width, 2);
            	half x1 = pow(x0,       p);
            	half y1 = pow(y0,       p);
            	half x2 = pow(1.0 - x0, p);
            	half y2 = pow(1.0 - y0, p);
            	half4 grid = ((x1 + x2) + (y1 + y2)) / 2 * _Color;
            	
            	// half alpha = (0.5 + circle) * _Color.a;
                // return half4(half3(alpha * grid), alpha);
            	half alpha = _Color.a;
                return half4(half3(grid), alpha);
            }

            ENDCG
        }
    }
}