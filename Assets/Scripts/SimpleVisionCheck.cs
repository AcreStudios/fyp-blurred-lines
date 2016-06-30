using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleVisionCheck : MonoBehaviour 
{
	SkinnedMeshRenderer rend;

	void Awake()
	{
		rend = GetComponent<SkinnedMeshRenderer>();
		rend.material.shader = Shader.Find("Custom/ObjectHighlight");
	}

	void Update () 
	{
		if(CAM_ElectroVision.visionActive)
		{
			rend.material.SetFloat("_UseHighlights", 1);
		}
		else
			rend.material.SetFloat("_UseHighlights", 0);
	}
}
