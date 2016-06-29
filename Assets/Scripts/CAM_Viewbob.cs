using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CAM_Viewbob : MonoBehaviour 
{
	public float bobSpeed = 10;
	float bobStepCounter;
	public float bobAmountX = 1f;
	public float bobAmountY = 0.5f;

	Vector3 parentLastPos;

	// Cache
	Transform trans;

	void Awake()
	{
		trans = GetComponent<Transform>();
	}

	void Start()
	{
		parentLastPos = trans.parent.position;
	}

	void LateUpdate()
	{
		if(TP_Controller.characterController.isGrounded)
			bobStepCounter += Vector3.Distance(parentLastPos, trans.parent.position) * bobSpeed * Time.deltaTime;

		Vector3 temp = trans.localPosition;
		temp.x = Mathf.Sin(bobStepCounter) * bobAmountX;
		temp.y = (Mathf.Cos(bobStepCounter * 2f) * bobAmountY * -1f);
		trans.localPosition = temp;

		parentLastPos = trans.parent.position;
	}
}
