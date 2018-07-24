using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ProjectBuild : Editor
{
		public static string[] Args=null;

		static string[] GetBuildScenes()
		{
					List<string> names = new List<string> ();

					foreach(EditorBuildSettingsScene e in EditorBuildSettings.scenes)
					{
							if (e == null) 
							{
									continue;
							}
							if (e.enabled) 
							{
									names.Add (e.path);
							}
					}

					return names.ToArray ();
		}

		public static string[] projectArgs
		{
				get
				{ 
						if (Args == null) 
						{
								foreach (string arg in System.Environment.GetCommandLineArgs()) 
								{
										if (arg.StartsWith ("project")) 
										{
												Args = arg.Split ("-" [0]);
												break;
										}
								}
						} 

						return Args;
				}
		}

		public static string projectName
		{
				get
				{ 

						return projectArgs [1];
				}
		}

		public static string projectDefineSymbols
		{
				get
				{ 
						return projectArgs [2];	
				}
		}

		static void BuildForIPhone()
		{

		PlayerSettings.bundleIdentifier="com.ddianle.lovedance."+projectName;
				PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.iPhone,projectDefineSymbols);

				BuildPipeline.BuildPlayer (GetBuildScenes (), projectName, BuildTarget.iPhone, BuildOptions.None);

		}
}
