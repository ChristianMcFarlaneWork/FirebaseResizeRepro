using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine;

public class ImageUploader : MonoBehaviour
{

	#region Variables

	[Header("Required")]
	[SerializeField] private ScreenshotCamera _screenshotCamera;

	[Header("Settings")]
	[SerializeField] private LogLevel _loggingLevel = LogLevel.Error;
	[Space]
	[SerializeField] private string _bucketPath = "gs://YOUR-FIREBASE-BUCKET/";
	[SerializeField] private string _destinationPath = "uploadedImage.png";
	[SerializeField] private Vector2Int _screenshotRes = new Vector2Int(256, 256);

	public FirebaseStorage storage { get; private set; }
	public StorageReference bucket { get; private set; }

	#endregion

	#region Start

	public void Start()
	{
		InitializeFirebase();
	}

	#endregion

	#region Initialize Firebase + Storage

	private void InitializeFirebase()
	{
		// Initialize Firebase
		FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {

			// If we are available, Upload the image
			if (task.Result == DependencyStatus.Available)
				InitializeStorage();
			else
				Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
		});
	}

	private void InitializeStorage()
	{
		// Attempt to initialize our storage settings
		storage = FirebaseStorage.DefaultInstance;
		bucket = storage.GetReferenceFromUrl(_bucketPath);
		storage.LogLevel = _loggingLevel;

		// Then Immediately begin our upload
		UploadScreenshot();
	}

	#endregion

	#region Image Uploading

	private void UploadScreenshot()
	{
		// Get our texture for uploading
		Texture2D temporaryScreenshot = _screenshotCamera.TakeScreenshot(_screenshotRes.x, _screenshotRes.y);

		// Then just upload our image to server
		StorageReference imgStorageRef = bucket.Child(_destinationPath);
		MetadataChange imageMetadata = new MetadataChange() { ContentType = "image/png" };

		// Put our file
		Debug.Log("Starting upload of file to server");
		imgStorageRef.PutBytesAsync(temporaryScreenshot.EncodeToPNG(), imageMetadata).ContinueWithOnMainThread(task => {

			// Then just log if we completed or failed
			if (task.IsCanceled || task.IsFaulted)
				Debug.LogError($"Failed to put file, encountered error '{task.Exception}'");
			else
				Debug.Log($"Successfully uploaded file to servers");

			// Then Dispose the task and the texture
			task.Dispose();
			Destroy(temporaryScreenshot);
		});
	}

	#endregion

}
