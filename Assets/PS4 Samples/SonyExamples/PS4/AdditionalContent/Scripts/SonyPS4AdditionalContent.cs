using UnityEngine;
using UnityEngine.PS4;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SonyPS4AdditionalContent : MonoBehaviour, IScreen
{
	MenuStack menuStack = null;
	MenuLayout menuMain;

	void Start()
	{
		menuMain = new MenuLayout(this, 450, 34);
		menuStack = new MenuStack();
		menuStack.SetMenu(menuMain);
	}

	void Update()
	{
	}

	void MenuMain()
	{
		menuMain.Update();

		if (menuMain.AddItem("Find Installed Content"))
		{
			EnumerateDRMContent();
		}
	}

	public void Process(MenuStack stack)
	{
		MenuMain();
	}

	void OnGUI()
	{
		MenuLayout activeMenu = menuStack.GetMenu();
		activeMenu.GetOwner().Process(menuStack);
	}


	void EnumerateDRMContentFiles(string entitlementLabel)
	{
		OnScreenLog.Add("Entitlement label = " + entitlementLabel);

		if ( PS4DRM.ContentOpen(entitlementLabel) == true )
		{
			string mountPoint;
		
			if ( PS4DRM.ContentGetMountPoint(entitlementLabel, out mountPoint) == true )
			{	
				string filePath = mountPoint;

				OnScreenLog.Add("Found content folder: " + filePath);

				string[] files = Directory.GetFiles(filePath);
				OnScreenLog.Add(" containing " + files.Length + " files");
				foreach (string file in files)
				{
					OnScreenLog.Add("  " + file);
					if (file.Contains(".unity3d"))
					{
						AssetBundle bundle = AssetBundle.LoadFromFile(file);

						Object[] assets = bundle.LoadAllAssets();
						OnScreenLog.Add("  Loaded " + assets.Length + " assets from asset bundle.");

						bundle.Unload(false);
					}
				}
			}
			else
			{
				OnScreenLog.Add("Can't mount entitlement");
			}

			PS4DRM.ContentClose(entitlementLabel);
		}
		else
		{
			OnScreenLog.Add("Can't open entitlement");
		}

	}

	void EnumerateDRMContent()
	{
		PS4DRM.DrmContentFinder finder = new PS4DRM.DrmContentFinder();
		finder.serviceLabel = 0;
		bool found = false;
		if (PS4DRM.ContentFinderOpen(ref finder))
		{
			found = true;
			EnumerateDRMContentFiles(finder.entitlementLabel);
			while (PS4DRM.ContentFinderNext(ref finder))
			{
				EnumerateDRMContentFiles(finder.entitlementLabel);
			};
			PS4DRM.ContentFinderClose(ref finder);
		}
		if (!found)
		{
			OnScreenLog.Add("No content found");
		}
	}

}
