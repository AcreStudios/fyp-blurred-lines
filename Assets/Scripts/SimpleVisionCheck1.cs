using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleVisionCheck1 : MonoBehaviour 
{
	MeshRenderer rend;

	void Awake()
	{
		rend = GetComponent<MeshRenderer>();
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
