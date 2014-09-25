Shader "Custom/Animated 2D Shader" {
	Properties {
		_Tex ("Texture Image", 2D) = "white" {} 
		_Width ("Width", Float) = 3 
		_Height ("Height", Float) = 3 
		_Index ("Height", Float) = 0 
	}
	SubShader {
		Pass {
			CGPROGRAM 
			
 			#pragma vertex vert 
			#pragma fragment frag
			
			uniform float _Index;
			uniform float _Width;
			uniform float _Height;
			uniform sampler2D _Tex;
				
			struct vin {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 tex : TEXCOORD0;
			};

			v2f vert(vin v) {
				v2f vf;
				vf.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				float i = _Index % _Width * _Height;
				float2 t = float2(i%_Width, _Height - i/_Width) + v.texcoord;
				vf.tex = t * float2(1/_Width, 1/_Height);
				return vf;
			}
			float4 frag(v2f vf) : COLOR {
				return tex2D(_Tex, vf.tex.xy);
			}
	 
			ENDCG
		}
	}
}