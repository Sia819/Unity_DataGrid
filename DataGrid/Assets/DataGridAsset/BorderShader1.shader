Shader "Custom/BorderShader1"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _BorderColor("Border Color", Color) = (1,1,1,1)
        _BorderWidth("Border Width", Float) = 1.0 // �׵θ��� �ȼ� �ʺ�
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _BorderColor;
            float _BorderWidth;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float borderWidthNormalized = _BorderWidth / _ScreenParams.y; // �ȼ� ���� �׵θ� �ʺ� UV ��ǥ�� �°� ��ȯ
                float alpha = step(borderWidthNormalized, uv.x) * step(borderWidthNormalized, 1-uv.x) *
                              step(borderWidthNormalized, uv.y) * step(borderWidthNormalized, 1-uv.y);
                fixed4 color = tex2D(_MainTex, uv); // �ؽ�ó ���ø�
                color.rgb = lerp(color.rgb, _BorderColor.rgb, 1 - alpha);
                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
