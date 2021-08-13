using UnityEngine;

/// <summary>
/// Class which just references a camera, and forces a screenshot. Returning the result as a Texture2D
/// </summary>
public class ScreenshotCamera : MonoBehaviour
{

	#region Variables

	[Header("Required")]
	[SerializeField] private Camera _camera;

	#endregion

	#region OnEnable / OnDisable

	private void OnEnable()
	{
		if (_camera == null)
			_camera = GetComponent<Camera>();

		_camera.enabled = false;
	}

	#endregion

	#region Take Screenshot

	/// <summary>
	/// Takes a screenshot of the provided camera with the provided settings
	/// </summary>
	/// <returns></returns>
	public Texture2D TakeScreenshot(int width, int height)
	{
		// Setup RenderTexture
		RenderTexture screenshotTemp = RenderTexture.GetTemporary(new RenderTextureDescriptor(width, height, RenderTextureFormat.ARGB32, 24));
		RenderTexture cachedTex = RenderTexture.active;
		RenderTexture.active = screenshotTemp;

		// Clear our Render Texture
		GL.Clear(true, true, Color.clear);

		// Setup camera
		_camera.targetTexture = screenshotTemp;
		_camera.aspect = width / (float)height;

		// Render
		_camera.Render();

		// Reset our camera
		_camera.targetTexture = null;
		_camera.ResetAspect();

		// Then cast our data and return as a texture 2D
		Texture2D screenshotTex = new Texture2D(width, height, TextureFormat.ARGB32, false);
		screenshotTex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		screenshotTex.Apply();

		// Reset our Render Texture
		RenderTexture.active = cachedTex;
		RenderTexture.ReleaseTemporary(screenshotTemp);

		// THen return our texture
		return screenshotTex;
	}

	#endregion

}
