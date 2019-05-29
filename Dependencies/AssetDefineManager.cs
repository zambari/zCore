/*
	Author: Jon Kenkel (nonathaj)
	Created: 1/23/2016
*/
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
 
[InitializeOnLoad]
public class AssetDefineManager : AssetPostprocessor
{
	/// <summary>
	/// Custom defines to add based on the file to detect the asset by, and the desired platforms
	/// </summary>
		static BuildTargetGroup[] destkop= new BuildTargetGroup[1]{
			BuildTargetGroup.Standalone
		};
	
	private static List<AssetDefine> CustomDefines = new List<AssetDefine>
	{
	
		//desktop[0]=BuildTargetGroup.
		//new AssetDefine("AssetDefineManager.cs", null, "AssetCustomDefine"),
	//	new AssetDefine("AssetDefineManager.cs", null, "AssetCustomDefine", "MyOtherDefine"),
			 
			 new AssetDefine("ISHowHide.cs",destkop , "ISHOWHIDE"),
			 new AssetDefine("IAnimateInOut.cs",destkop , "iAnimateInOut"),
		    new AssetDefine("zLog.cs",destkop , "zLog"),
			new AssetDefine("zOSC.cs",destkop , "zOSC"),
			new AssetDefine("PlayerPrefsX.cs",destkop , "PlayerPrefsX"),
			new AssetDefine("MidiMaster.cs",destkop , "MIDI"),
	   	new AssetDefine("zKeyMap.cs",destkop , "zKeyMap"),
		   new AssetDefine("zKeyMap.cs",destkop , "zKeyMap"),
		   new AssetDefine("EventsWithParameters.cs",destkop , "paramEvents"),
		   new AssetDefine("VideoControllerBasic.cs",destkop , "zVideo"),
		   
	};
 
	private struct AssetDefine
	{
		public readonly string assetDetectionFile;              //the file used to detect if the asset exists
		public readonly string[] assetDefines;                  //series of defines for this asset
		public readonly BuildTargetGroup[] definePlatforms;     //platform this define will be used for (null is all platforms)
 
		public AssetDefine(string fileToDetectAsset, BuildTargetGroup[] platformsForDefine, params string[] definesForAsset)
		{
			assetDetectionFile = fileToDetectAsset;
			definePlatforms = platformsForDefine;
			assetDefines = definesForAsset;
		}
 
		public bool IsValid { get { return assetDetectionFile != null && assetDefines != null; } }
		public static AssetDefine Invalid = new AssetDefine(null, null, null);
 
		public void RemoveAllDefines()
		{
			foreach (string define in assetDefines)
				RemoveCompileDefine(define, definePlatforms);
		}
 
		public void AddAllDefines()
		{
			foreach (string define in assetDefines)
				AddCompileDefine(define, definePlatforms);
		}
	}
 
	static AssetDefineManager()
	{
		ValidateDefines();
	}
 
	private static void ValidateDefines()
	{
		foreach(AssetDefine def in CustomDefines)
		{
			string[] fileCodes = AssetDatabase.FindAssets(Path.GetFileNameWithoutExtension(def.assetDetectionFile));
			foreach(string fileCode in fileCodes)
			{
				string fileName = Path.GetFileName(AssetDatabase.GUIDToAssetPath(fileCode));
				if(fileName == def.assetDetectionFile)
				{
					if (def.IsValid)        //this is an asset we are tracking for defines
						def.AddAllDefines();
				}
			}
		}		
	}
 
	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string deletedFile in deletedAssets)
		{
			AssetDefine def = AssetDefine.Invalid;
         {
				string file = Path.GetFileName(deletedFile);
				foreach (AssetDefine define in CustomDefines)
				{
					if (define.assetDetectionFile == file)
					{
						def = define;
						break;
					}
				}
			}
 
			if (def.IsValid)			//this is an asset we are tracking for defines
				def.RemoveAllDefines();
		}
	}
 
	/// <summary>
	/// Attempts to add a new #define constant to the Player Settings
	/// </summary>
	/// <param name="newDefineCompileConstant">constant to attempt to define</param>
	/// <param name="targetGroups">platforms to add this for (null will add to all platforms)</param>
	public static void AddCompileDefine(string newDefineCompileConstant, BuildTargetGroup[] targetGroups = null)
	{
		if (targetGroups == null)
			targetGroups = (BuildTargetGroup[])Enum.GetValues(typeof(BuildTargetGroup));
 
		foreach (BuildTargetGroup grp in targetGroups)
		{
			if (grp == BuildTargetGroup.Unknown)        //the unknown group does not have any constants location
				continue;
 
			string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(grp);
			if (!defines.Contains(newDefineCompileConstant))
			{
				if (defines.Length > 0)         //if the list is empty, we don't need to append a semicolon first
					defines += ";";
 
				defines += newDefineCompileConstant;
				PlayerSettings.SetScriptingDefineSymbolsForGroup(grp, defines);
			}
		}
	}
 
	/// <summary>
	/// Attempts to remove a #define constant from the Player Settings
	/// </summary>
	/// <param name="defineCompileConstant"></param>
	/// <param name="targetGroups"></param>
	public static void RemoveCompileDefine(string defineCompileConstant, BuildTargetGroup[] targetGroups = null)
	{
		if (targetGroups == null)
			targetGroups = (BuildTargetGroup[])Enum.GetValues(typeof(BuildTargetGroup));
 
		foreach (BuildTargetGroup grp in targetGroups)
		{
			string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(grp);
			int index = defines.IndexOf(defineCompileConstant);
			if (index < 0)
				continue;           //this target does not contain the define
			else if (index > 0)
				index -= 1;         //include the semicolon before the define
									//else we will remove the semicolon after the define
 
			//Remove the word and it's semicolon, or just the word (if listed last in defines)
			int lengthToRemove = Math.Min(defineCompileConstant.Length + 1, defines.Length - index);
 
			//remove the constant and it's associated semicolon (if necessary)
			defines = defines.Remove(index, lengthToRemove);
 
			PlayerSettings.SetScriptingDefineSymbolsForGroup(grp, defines);
		}
	}
}
#endif