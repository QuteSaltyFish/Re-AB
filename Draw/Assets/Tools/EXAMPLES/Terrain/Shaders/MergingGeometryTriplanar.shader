﻿// Note: I've commented out the Regulr BumpMap option because it was giving "Not Marked as Normal" warning on the material, which is annoying. 
// It's safe to uncomment all comments below:

Shader "Terrain/MergingGeometryTriplanar" {
	Properties{
	[NoScaleOffset]_MainTex("Geometry Texture (RGB)", 2D) = "white" {}
	//[KeywordEnum(None, Regular, Combined)] _BUMP("Bump Map", Float) = 0
	[KeywordEnum(None, Combined)] _BUMP("Map", Float) = 0

	[NoScaleOffset]_Map("Geometry Combined Maps (RGBA)", 2D) = "gray" {}
	_Merge("_Merge", Range(0.01,2)) = 1
	[Toggle(CLIP_ALPHA)] _ALPHA("Clip Alpha", Float) = 0
	}
    
		Category{
		Tags{ "RenderType" = "Opaque"
		"LightMode" = "ForwardBase"
		"Queue" = "Geometry"
	}
		LOD 200
		ColorMask RGBA


		SubShader{
		Pass{



		CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fog
#include "UnityLightingCommon.cginc" 
#include "Lighting.cginc"
#include "AutoLight.cginc"
#include "Assets/Tools/SHARED/VertexDataProcessInclude.cginc"

#pragma multi_compile_fwdbase 
#pragma multi_compile  ___ MODIFY_BRIGHTNESS 
#pragma multi_compile  ___ COLOR_BLEED
#pragma multi_compile  ___ WATER_FOAM
#pragma multi_compile  ___ _BUMP_NONE  _BUMP_COMBINED 
		/*_BUMP_REGULAR*/
#pragma multi_compile  ___ CLIP_ALPHA

	sampler2D _MainTex;
	sampler2D _Map;

	struct v2f {
		float4 pos : POSITION;

		UNITY_FOG_COORDS(1)
		float3 viewDir : TEXCOORD2;
		float3 wpos : TEXCOORD3;
		float3 tc_Control : TEXCOORD4;
#if WATER_FOAM
		float4 fwpos : TEXCOORD5;
#endif
		SHADOW_COORDS(6)
		float2 texcoord : TEXCOORD7;
#if _BUMP_NONE
		float3 normal : TEXCOORD8;
#else
		float3 tspace0 : TEXCOORD8; 
		float3 tspace1 : TEXCOORD9; 
		float3 tspace2 : TEXCOORD10; 
#endif
	};

	v2f vert(appdata_full v) {
		v2f o;

		float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
		o.tc_Control.xyz = (worldPos.xyz - _mergeTeraPosition.xyz) / _mergeTerrainScale.xyz;

		o.pos = UnityObjectToClipPos(v.vertex);
		o.wpos = worldPos;
		o.viewDir.xyz = (WorldSpaceViewDir(v.vertex));

		o.texcoord = v.texcoord;
		UNITY_TRANSFER_FOG(o, o.pos);
		TRANSFER_SHADOW(o);

		float3 worldNormal = UnityObjectToWorldNormal(v.normal);

#if WATER_FOAM

		o.fwpos = foamStuff(o.wpos);      

#endif

		half3 wNormal = worldNormal;

#if _BUMP_NONE
		o.normal.xyz = UnityObjectToWorldNormal(v.normal);
#else
		half3 wTangent = UnityObjectToWorldDir(v.tangent.xyz);
		half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
		half3 wBitangent = cross(wNormal, wTangent) * tangentSign;

		o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
		o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
		o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);
#endif

		return o;
	}


	float4 frag(v2f i) : COLOR{
		i.viewDir.xyz = normalize(i.viewDir.xyz);
	float dist = length(i.wpos.xyz - _WorldSpaceCameraPos.xyz);

	float4 col = tex2D(_MainTex, i.texcoord.xy);
	
#if CLIP_ALPHA
	clip(col.a - 0.5);
	col.a = 0.1;
#endif

#if _BUMP_NONE
	float3 worldNormal = i.normal;
	float4 bumpMap = float4(0, 0, 1, 1);
#else

	float4 bumpMap = tex2D(_Map, i.texcoord.xy);
	float3 tnormal;
/*#if _BUMP_REGULAR
	tnormal = UnpackNormal(bumpMap);
	bumpMap = float4(0, 0, 1, 1);
#else*/
	bumpMap.rg = (bumpMap.rg - 0.5) * 2;
	tnormal = float3(bumpMap.r, bumpMap.g, 1);
//#endif

	float3 worldNormal;
	worldNormal.x = dot(i.tspace0, tnormal);
	worldNormal.y = dot(i.tspace1, tnormal);
	worldNormal.z = dot(i.tspace2, tnormal);
#endif

	float4 terrainN = 0;

	Terrain_Trilanear(i.tc_Control, i.wpos, dist, worldNormal, col, terrainN, bumpMap);

	float shadow = SHADOW_ATTENUATION(i);

	float Metalic = 0;

	Terrain_Light(i.tc_Control, terrainN, worldNormal, i.viewDir.xyz, col, shadow, Metalic,
#if WATER_FOAM
		i.fwpos
#else
	0
#endif
		);

	UNITY_APPLY_FOG(i.fogCoord, col);

	return col;
	}


		ENDCG
	}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
	}
}
