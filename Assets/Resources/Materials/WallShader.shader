﻿Shader "Custom/WallShader" {
	Properties 
	{ 
		_Color ("Main Color", Color) = (1,1,1,1) 
		//_Color2 ("Non-Wall Color", Color) = (1,1,1,1) 
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {} 
	}
	Category 
 	{
		 SubShader 
		 { 
		     Tags { "Queue"="Overlay+1" }
		     Pass
		     {
		         ZTest Greater
		         Lighting Off
		         Color [_Color]
		     }
		     Pass 
		     {
		         ZTest Less 
		         Lighting Off
		         //Color [_Color2]         
		         SetTexture [_MainTex] {combine texture}
		     }

		 }
 	}
 FallBack "Specular", 1
}
