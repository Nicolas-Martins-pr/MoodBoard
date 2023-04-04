Shader "Custom/NewUnlitShader" {
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _Center("Center", Vector) = (0,0,0,0)
        _Radius("Radius", Range(0.0, 1.0)) = 0.5
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 100
            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                    float4 screenPos : TEXCOORD0;
                };

                float4 _Color;
                float3 _Center;
                float _Radius;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.screenPos = ComputeScreenPos(o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    float2 center = _Center.xy;
                    float2 pos = i.screenPos.xy;
                    float dist = distance(center, pos);
                    if (dist < _Radius) {
                        return _Color;
                    }
                    return fixed4(0, 0, 0, 0);
                }
                ENDCG
            }
    }
        Fallback "Diffuse"
}
