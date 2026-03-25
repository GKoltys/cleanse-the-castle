Shader "Custom/DimOverlay"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _DimColor ("Dim Color", Color) = (0,0,0,0.8)
        _FOVTex ("FOV Render Texture", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent+100" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _DimColor;
            sampler2D _FOVTex;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the FOV texture — white = visible, black = hidden
                fixed4 fov = tex2D(_FOVTex, i.uv);

                // Where FOV is white (visible), reduce alpha to 0 (clear)
                // Where FOV is black (hidden), keep full dim alpha
                float dimAlpha = _DimColor.a * (1.0 - fov.r);
                return fixed4(_DimColor.rgb, dimAlpha);
            }
            ENDCG
        }
    }
}