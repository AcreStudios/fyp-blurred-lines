using UnityEngine;
using System.Collections;

public class FP_Camera : MonoBehaviour 
{
	#region Declare variables
	GameObject character;
	Vector2 mouseLook;
	Vector2 smoothV;

	public float lookSensitivity = 4f;
	public float lookSmoothing = 2f;

	public float yMax = 50f;

	public CursorLockMode cLock = CursorLockMode.Locked;
	#endregion

	#region Cache components
	Transform trans;
	#endregion

	void Awake()
	{
		// Cache the transform component of camera
		trans = GetComponent<Transform>();
	}

	void Start()
	{
		character = this.transform.parent.gameObject;

		Cursor.lockState = cLock;
	}
	
	void Update() 
	{
		Vector2 md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

		float c = lookSensitivity * lookSensitivity * Time.deltaTime;
		md = Vector2.Scale(md, new Vector2(c, c));
		smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / lookSmoothing);
		smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / lookSmoothing);
		mouseLook += smoothV;

		// Clamp camera angles
		if(mouseLook.y > yMax + 1f)
			mouseLook.y = yMax;
		if(mouseLook.y < -yMax + 1f)
			mouseLook.y = -yMax;

		trans.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
		character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
	}
}
