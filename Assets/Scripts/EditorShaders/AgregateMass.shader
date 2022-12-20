Shader "T22_AD_Sim/AgregateMass" {
	Properties {
		//normalized in relation to the heaviest child
		_Weight ("_Weight", Float) = 0.0


	}
	SubShader {
		Tags {"RenderType"="Opaque"}
	 	Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			float _Weight;
			
			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};
			struct v2f {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};
			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color;
				return o;
			}
			float4 frag (v2f i) : SV_Target {
				//print _Weight
				
				//get the weight of the child
				if(_Weight < 0.5 && _Weight >= 0.0){
					return float4(0.0, 0.0, 1-_Weight, 1.0);
				}
				else if(_Weight >= 0.5 && _Weight <= 1.0){
					return float4(_Weight, 0.0,0.0, 1.0);
				}
				else{
					return float4(1.0, 1.0, 1.0, 1.0);
				}

			}
			ENDCG
		}
	}
}
