﻿using UnityEngine;
using System.Collections;

public class TP_Movement : MonoBehaviour
{
	#region Declare variables
	public float moveSpeed = 5f;
	public float jumpSpeed = 6f;

	public float gravity = 21f; // Fall acceleration
	public float terminalVelocity = 20f; // Max falling speed

	public Vector3 moveVector { get; set; }
	public float verticalVelocity { get; set; }

	bool isAttacking = false;
	#endregion

	#region Cache components
	Transform trans;

	public static TP_Movement instance;
	#endregion

	// Awake is called before Start
	void Awake()
	{
		trans = GetComponent<Transform>();

		instance = this;
	}

	public void MovementUpdate()
	{
		if(!isAttacking)
			CalculateMovement();
		//SnapAlignCharacterWithCamera();
	}

	void CalculateMovement()
	{
		// Convert moveVector into world space
		moveVector = trans.TransformDirection(moveVector);

		// Normalize moveVector if > 1
		if(moveVector.magnitude > 1)
			moveVector = Vector3.Normalize(moveVector);

		// Multiply moveVector by moveSpeed BUT not deltaTime yet
		moveVector *= moveSpeed;

		// Reapply verticalVelocity to moveVector.y
		moveVector = new Vector3(moveVector.x, verticalVelocity, moveVector.z);

		// Apply gravity
		ApplyGravity();

		// Move the character and multiply by deltaTime here instead
		TP_Controller.characterController.Move(moveVector * Time.deltaTime);

	}

	void ApplyGravity()
	{
		if(moveVector.y > -terminalVelocity)
			moveVector = new Vector3(moveVector.x, moveVector.y - gravity * Time.deltaTime, moveVector.z);

		if(TP_Controller.characterController.isGrounded && moveVector.y < -1)
			moveVector = new Vector3(moveVector.x, -1, moveVector.z);
	}

	void SnapAlignCharacterWithCamera()
	{
		if(moveVector.x != 0 || moveVector.z != 0)
		{
			trans.rotation = Quaternion.Euler(trans.eulerAngles.x, Camera.main.transform.eulerAngles.y, trans.eulerAngles.z);
		}
	}

	public void Jump()
	{
		if(TP_Controller.characterController.isGrounded)
			verticalVelocity = jumpSpeed;
	}

	public void StartCrouch()
	{
		TP_Controller.characterController.height *= .5f;
		moveSpeed *= .25f;
	}

	public void StopCrouch()
	{
		TP_Controller.characterController.height *= 2f;
		moveSpeed *= 4f;
	}

	public void AttackFast(GameObject enemy)
	{
		// Teleport
		trans.position = enemy.transform.position - enemy.transform.TransformDirection(0f, 0f, 1.5f);
		StartCoroutine(PerformAttack(enemy, 1f));

		// Face enemy
		trans.GetChild(0).GetComponent<FP_Camera>().mouseLook.x = enemy.transform.eulerAngles.y;
	}

	public void AttackSlow(GameObject enemy)
	{
		// Teleport
		trans.position = enemy.transform.position - enemy.transform.TransformDirection(0f, 0f, 1f);
		StartCoroutine(PerformAttack(enemy, 5f));

		// Face enemy
		trans.GetChild(0).GetComponent<FP_Camera>().mouseLook.x = enemy.transform.eulerAngles.y;
	}

	IEnumerator PerformAttack(GameObject enemy, float delay)
	{
		isAttacking = true;
		yield return new WaitForSeconds(delay);
		isAttacking = false;
		print("AttackEnd");
		enemy.GetComponent<AITemplate>().Death();
	}
}
