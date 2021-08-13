# FirebaseResizeRepro
This is an example repro for the issue https://github.com/firebase/quickstart-unity/issues/1107.
Which outlines that when uploading an image from the Firebase Unity SDK, the specified metadata for the file isn't uploaded before object finalize, therefore the Resize Image Extension is never triggered.

## Setup Required
- A Firebase project which has the resize Image Extension activated and configured. [Our settings](https://user-images.githubusercontent.com/28091817/129295844-90819336-9681-497c-a07c-3ca99d908969.png)
- Unity 2019.4.28f1 - The version this issue is being testing on.
- Import the Firebase Storage Unity SDK ver 8.1.0 
- Import your `google-services.json` file and add it to the Assets Folder
- Open up `Scenes/Repro`
- Select the GameObject `ReproScript`, and setup the correct settings for both `Bucket Path` and `Destination Path`.
- Finally just run the scene.
After running note that the extension doesn't trigger and returns [these logs](https://user-images.githubusercontent.com/28091817/129296514-6a0cd109-b893-49a2-8815-5272ab4ae2d6.png).
