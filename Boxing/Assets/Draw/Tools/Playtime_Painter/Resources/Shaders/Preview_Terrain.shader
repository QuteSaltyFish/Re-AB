﻿Shader "Editor/TerrainPreview" {
	Category{
            Tags { "RenderType" = "Opaque"
             "LightMode" = "ForwardBase"
               "Queue" = "Geometry"
             }
                LOD 200
            ColorMask RGBA


    SubShader {
        Pass {

        CGPROGRAM

        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_fog
        #include "UnityLightingCommon.cginc" 
        #include "Lighting.cginc"
        #include "UnityCG.cginc"
        #include "AutoLight.cginc"
        #include "Assets/Tools/SHARED/VertexDataProcessInclude.cginc"

        #pragma multi_compile_fwdbase //nolightmap nodirlightmap nodynlightmap novertexlight
      //  #pragma addshadow
        #pragma multi_compile  ___ MODIFY_BRIGHTNESS 
        #pragma multi_compile  ___ COLOR_BLEED
    
   

            struct v2f {
                float4 pos : POSITION;
                float3 wpos : TEXCOORD3;
                float3 tc_Control : TEXCOORD4;
                SHADOW_COORDS(6) 
                float3 normal : TEXCOORD7;
                float2 texcoord : TEXCOORD8;
            };

            v2f vert (appdata_full v) {
                v2f o;

                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

                o.tc_Control.xyz = (worldPos.xyz - _mergeTeraPosition.xyz) / _mergeTerrainScale.xyz;

                float height = tex2Dlod(_mergeTerrainHeight, float4(o.tc_Control.xz, 0, 0)).a;
                float2 ts = _mergeTerrainHeight_TexelSize.xy;
                float up    = tex2Dlod(_mergeTerrainHeight, float4(o.tc_Control.x,        o.tc_Control.z+ts.y , 0, 0)).a;
                float right = tex2Dlod(_mergeTerrainHeight, float4(o.tc_Control.x+ts.x,   o.tc_Control.z , 0, 0)).a;

                worldPos.y = _mergeTeraPosition.y + height*_mergeTerrainScale.y;
                v.vertex = mul(unity_WorldToObject, float4(worldPos.xyz, v.vertex.w));

                o.tc_Control.xyz = (worldPos.xyz - _mergeTeraPosition.xyz) / _mergeTerrainScale.xyz;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.wpos = worldPos;
            
                o.texcoord = v.texcoord;
                TRANSFER_SHADOW(o);

                float3 worldNormal =  float3(height-right, 0.01, height-up);//UnityObjectToWorldNormal(v.normal);

                o.normal =  normalize(worldNormal);

                return o;
            }


            float4 frag (v2f i) : COLOR {

            float4 cont = tex2D(_mergeControl, i.tc_Control.xz);
            float4 height = tex2D(_mergeTerrainHeight, i.tc_Control.xz + _mergeTerrainScale.w);
            height.rgb = i.normal+0.5;
            float3 bump = (height.rgb - 0.5)*2;

            float aboveTerrain = saturate((((i.wpos.y - _mergeTeraPosition.y) - height.a*_mergeTerrainScale.y) - 0.5)*0.5);
            float deAboveTerrain = 1 - aboveTerrain;


            bump = bump*deAboveTerrain + i.normal * aboveTerrain;


            float2 tiled = i.tc_Control.xz*_mergeTerrainTiling.xy+ _mergeTerrainTiling.zw;
            float tiledY = i.tc_Control.y*_mergeTeraPosition.w*2;

            float4 splat0 =  tex2D(_mergeSplat_0, tiled);
            float4 splat1 =  tex2D(_mergeSplat_1, tiled);
            float4 splat2 =  tex2D(_mergeSplat_2, tiled);
            float4 splat3 =  tex2D(_mergeSplat_3, tiled);
            
            //float4 splaty = tex2D(_mergeSplat_4, tiled);
            //float4 splatz = tex2D(_mergeSplat_4, float2(tiled.x, tiledY));
            //float4 splatx = tex2D(_mergeSplat_4, float2(tiled.y, tiledY));

            float4 splat0N =  tex2D(_mergeSplatN_0, tiled);
            float4 splat1N =  tex2D(_mergeSplatN_1, tiled);
            float4 splat2N =  tex2D(_mergeSplatN_2, tiled);
            float4 splat3N =  tex2D(_mergeSplatN_3, tiled);

            // Splat 4 is a base layer:
            //float4 splatNy = tex2D(_mergeSplatN_4, tiled);
            //float4 splatNz = tex2D(_mergeSplatN_4, float2(tiled.x, tiledY));
            //float4 splatNx = tex2D(_mergeSplatN_4, float2(tiled.y, tiledY));
        
            float edge = MERGE_POWER;

            float4 terrain = splat0;
            float4 terrainN = splat0N;

            float maxheight = (1)*abs(bump.y);

            float3 newBump = float3(0,1, 0);

            //Triplanar X:
            /*float newHeight = (1.5 + splatx.a)*abs(bump.x);
            float adiff = max(0, (newHeight - maxheight));
            float alpha = min(1, adiff*(1 + edge*terrainN.b*splatNx.b));
            float dAlpha = (1 - alpha);
            terrain = terrain*dAlpha + splatx*alpha;
            terrainN.ba = terrainN.ba*dAlpha + splatNx.ba*alpha;
            newBump = newBump*dAlpha + float3(0, splatNx.y - 0.5,splatNx.x-0.5)*alpha;
            maxheight += adiff;

            //Triplanar Z:
            newHeight = (1.5 + splatz.a)*abs(bump.z);
            adiff = max(0, (newHeight - maxheight));
            alpha = min(1, adiff*(1 + edge*terrainN.b*splatNz.b));
            dAlpha = (1 - alpha);
            terrain = terrain*(dAlpha) +splatz*alpha;
            terrainN.ba = terrainN.ba*dAlpha + splatNz.ba*alpha;
            newBump = newBump*dAlpha + float3(splatNz.x - 0.5, splatNz.y - 0.5, 0)*alpha;
            maxheight += adiff;*/

            terrainN.rg = 0.5;

            float tripMaxH = maxheight;
            float3 tmpbump = normalize(bump+ newBump*2);

            float triplanarY = max(0, tmpbump.y)*2; // Recalculate it based on previously sampled bump

            float newHeight = cont.r * triplanarY + splat0.a;
            float adiff = max(0, (newHeight - maxheight));
            float alpha = min(1, adiff*(1 + edge*terrainN.b*splat0N.b));
            float dAlpha = (1 - alpha);
            terrain = terrain*(dAlpha) + splat0*alpha;
            terrainN = terrainN*(dAlpha) + splat0N*alpha;
            maxheight += adiff;


            newHeight = cont.g*triplanarY +splat1.a;
            adiff = max(0, (newHeight-maxheight));
            alpha = min(1,adiff*(1 + edge*terrainN.b*splat1N.b));
            dAlpha = (1 - alpha);
            terrain = terrain*(dAlpha)+splat1*alpha;
            terrainN = terrainN*(dAlpha)+splat1N*alpha;
            maxheight += adiff;
            

            newHeight = cont.b*triplanarY +splat2.a;
            adiff = max(0, (newHeight-maxheight));
            alpha = min(1,adiff*(1 + edge*terrainN.b*splat2N.b));
            dAlpha = (1 - alpha);
            terrain = terrain*(dAlpha)+splat2*alpha;
            terrainN = terrainN*(dAlpha)+splat2N*alpha;
            maxheight += adiff;

            newHeight = cont.a*triplanarY +splat3.a;
            adiff = max(0, (newHeight-maxheight));
            alpha = min(1,adiff*(1 + edge*terrainN.b*splat3N.b));
            dAlpha = (1 - alpha);
            terrain = terrain*(dAlpha)+splat3*alpha;
            terrainN = terrainN*(dAlpha)+splat3N*alpha;
            maxheight += adiff;
        
            terrainN.rg = terrainN.rg*2 -1;

            adiff = max(0, (tripMaxH+0.5- maxheight));
            alpha = min(1, adiff*2);

            bump = tmpbump*alpha + (1 - alpha)*bump;

            cont = terrain;

            float3 worldNormal = normalize(bump +float3(terrainN.r, 0, terrainN.g));

            float shadow = SHADOW_ATTENUATION(i);

            float diff = dot(worldNormal, _WorldSpaceLightPos0.xyz);
          //  diff = saturate(diff - ambientBlock*4*(1- diff));

            float direct = diff*shadow;

            float4 col = 0;

            col.rgb = cont.rgb*_LightColor0*direct;

            #if MODIFY_BRIGHTNESS
            col.rgb *= _lightControl.a;
#endif

            #if COLOR_BLEED
            float3 mix = col.gbr + col.brg;
            col.rgb += mix*mix*_lightControl.r;
#endif



          //  col = height.a;

            return col;
            }

        ENDCG
    }
      //UsePass "Editor/br_TerrainPreview_Shadow"
}
}
}
