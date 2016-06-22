using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CAM_ElectroVision : MonoBehaviour
{
	Camera cam;

	public Shader visionShader;
	public Color visionColor = new Color(0f, .05f, .05f, 1f);

	void Awake()
	{
		cam = GetComponent<Camera>();
	}

	void OnValidate()
	{
		Shader.SetGlobalColor("_VisionColor", visionColor);
	}

	void OnEnable()
	{
		if(visionShader != null)
			cam.SetReplacementShader(visionShader, "");

		cam.clearFlags = CameraClearFlags.SolidColor;
	}

	void OnDisable()
	{
		cam.ResetReplacementShader();

		cam.clearFlags = CameraClearFlags.Skybox;
	}
}
