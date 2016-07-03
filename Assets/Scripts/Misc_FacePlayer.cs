using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Misc_FacePlayer : MonoBehaviour 
{
	public Canvas eText;

	void Start()
	{
		eText = transform.GetChild(0).GetComponent<Canvas>();

		eText.enabled = false;
	}
	
	void OnTriggerStay(Collider col)
	{
		if(col.CompareTag("Player"))
		{
			// Turn on
			eText.enabled = true;
/*
			// Rotate on Y-axis only
			Vector3 temp = col.transform.position;
			temp.y = transform.position.y;
			trans.LookAt(temp);
            */
		}
			
	}

	void OnTriggerExit(Collider col)
	{
		if(col.CompareTag("Player"))
		{
			// Turn off
			eText.enabled = false;
		}
	}
}
