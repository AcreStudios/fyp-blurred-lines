using UnityEngine;
using System.Collections;

public class CAM_ElectroVision : MonoBehaviour
{
	public Color electroVisionColor;
	public float electroVisionIntensity = 1f;
	public float changeVisionSpeed = .5f;
	public float changeVisionDelay = 1f;

	public static bool visionActive = false;

	public void VisionToggle()
	{
		if(!visionActive)
		{
			StopAllCoroutines();
			StartCoroutine(StartVision());
			visionActive = true;
		}
		else
		{
			StopAllCoroutines();
			StartCoroutine(EndVision());
			visionActive = false;
		}
	}

	IEnumerator StartVision()
	{
		RenderSettings.ambientIntensity = 0f;
		RenderSettings.reflectionIntensity = 0f;
		RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
		RenderSettings.ambientEquatorColor = electroVisionColor;

		while(RenderSettings.ambientIntensity < electroVisionIntensity)
		{
			RenderSettings.ambientIntensity += changeVisionSpeed * Time.deltaTime;
			RenderSettings.reflectionIntensity -= changeVisionSpeed * Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator EndVision()
	{
		RenderSettings.ambientIntensity = 0f;
		RenderSettings.reflectionIntensity = 0f;
		RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;

		while(RenderSettings.ambientIntensity < electroVisionIntensity)
		{
			RenderSettings.ambientIntensity += changeVisionSpeed * Time.deltaTime;
			RenderSettings.reflectionIntensity += changeVisionSpeed * Time.deltaTime;
			yield return null;
		}
	}
}
