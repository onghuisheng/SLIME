using UnityEngine;
using UnityEditor;

public class PS4Setup : MonoBehaviour 
{
	[MenuItem("PS4 Setup/Setup Input Manager")]
	public static void SetupInputManager()
	{
	    if (EditorUtility.DisplayDialog("Setup Input Manager?", "This will delete the current manager. Are you sure you want to replace your current Input Manager? ", "Yes", "No"))
	    {
	        string sourceFile = Application.dataPath + "/InputModules/Examples/InputManager.txt";
	        if (!System.IO.File.Exists(sourceFile))
	        {
	            EditorUtility.DisplayDialog("Failure", string.Format("Source file does not exist! Tried to find: {0}", sourceFile), "OK");
	            return;
	        }

	        string targetFile = Application.dataPath;
	        targetFile = targetFile.Replace("/Assets", "/ProjectSettings/InputManager.asset");
	        if (!System.IO.File.Exists(targetFile))
	        {
	            EditorUtility.DisplayDialog("Failure", string.Format("Target file does not exist! Tried to find: {0}", targetFile), "OK");
	            return;
	        }

	        System.IO.File.Delete(targetFile);
	        System.IO.File.Copy(sourceFile, targetFile);

	        AssetDatabase.Refresh();

	        EditorUtility.DisplayDialog("Success", "Input Manager replaced!", "OK");
	    }
    }

    [MenuItem("PS4 Setup/Setup Publish Settings")]
    public static void SetupPublishSettings()
    {
        PlayerSettings.PS4.attribUserManagement = true;
    }
}
