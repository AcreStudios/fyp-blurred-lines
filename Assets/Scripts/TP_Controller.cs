using UnityEngine;
using System.Collections;

public class TP_Controller : MonoBehaviour 
{
	#region Declare variables
	// String for input axes
	public string horizontalAxis = "Horizontal";
	public string verticalAxis = "Vertical";
	public string jumpButton = "Jump";

	public float interactRange = 10f;
	#endregion

	#region Cache components
	public static CharacterController characterController;
	public static TP_Controller instance;

	CAM_ElectroVision vision;
	#endregion

	// Awake is called before Start
	void Awake()
	{
		characterController = GetComponent<CharacterController>();
		instance = this;

		vision = Camera.main.GetComponent<CAM_ElectroVision>();
		// Using the old camera system
		//TP_Camera.UseExistingOrCreateNewMainCamera();
	}

	// Update is called once per frame
	void Update()
	{
		if(Camera.main == null) // To prevent crashes
			return;

		GetLocomotionInput();
		HandleActionInput();

		TP_Movement.instance.MovementUpdate();
	}

	void GetLocomotionInput()
	{
		float deadZone = 0.1f;

		TP_Movement.instance.verticalVelocity = TP_Movement.instance.moveVector.y;
		TP_Movement.instance.moveVector = Vector3.zero;

		if(Input.GetAxis(verticalAxis) > deadZone || Input.GetAxis(verticalAxis) < -deadZone)
			TP_Movement.instance.moveVector += new Vector3(0, 0, Input.GetAxis(verticalAxis));

		if(Input.GetAxis(horizontalAxis) > deadZone || Input.GetAxis(horizontalAxis) < -deadZone)
			TP_Movement.instance.moveVector += new Vector3(Input.GetAxis(horizontalAxis), 0, 0);
	}

	void HandleActionInput()
	{
		if(Input.GetButton(jumpButton))
			DoJump();

		// Electrovision
		if(Input.GetKeyDown(KeyCode.LeftShift))
			vision.enabled = true;
		else if(Input.GetKeyUp(KeyCode.LeftShift))
			vision.enabled = false;

		// Interact
		if(Input.GetKeyDown(KeyCode.E))
		{
			RaycastHit hit;
			Ray ray = new Ray(Camera.main.transform.position, transform.forward);

			Debug.DrawRay(Camera.main.transform.position, transform.forward * interactRange, Color.green, 1f);
			if(Physics.Raycast(ray, out hit, interactRange))
			{
				Debug.Log(hit.transform.name);
				if(hit.collider.transform.parent.CompareTag("Door"))
				{
					hit.collider.transform.parent.GetComponent<Door_SimpleSlide>().DoorInteract();
				}
			}
		}
	}

	void DoJump()
	{
		TP_Movement.instance.Jump();
	}
}
