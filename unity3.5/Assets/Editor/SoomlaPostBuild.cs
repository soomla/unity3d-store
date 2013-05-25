using UnityEngine;
using System.Collections;
using UnityEditor.Callbacks;
using UnityEditor;
using System.Diagnostics;

public class PostProcessScriptStarter : MonoBehaviour {
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
		if (target == BuildTarget.iPhone) {
			//ProcessStartInfo psi = new ProcessStartInfo();
			//psi.FileName = Appilcation.dataPath + "/Editor/PostprocessBuildPlayerScriptForSoomla";
			//psi.UseShellExecute = false;
			//psi.RedirectStandardOutput = false;
			Process proc = new System.Diagnostics.Process();
			proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.RedirectStandardOutput = true;
			proc.StartInfo.RedirectStandardError = true;
			proc.EnableRaisingEvents=false; 
			proc.StartInfo.FileName = Application.dataPath + "/Editor/PostprocessBuildPlayerScriptForSoomla";
			proc.StartInfo.Arguments = Application.dataPath.Replace(" ", "_;@#") + " " + pathToBuiltProject.Replace(" ", "_;@#");
			proc.Start();
			string output = proc.StandardOutput.ReadToEnd();
			string err = proc.StandardError.ReadToEnd();
			proc.WaitForExit();
			UnityEngine.Debug.Log("out: " + output);
			UnityEngine.Debug.Log("error: " + err);
//			UnityEngine.Debug.Log(pathToBuiltProject);
		}
    }
}
