using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Sever.Gridder.Editor
{
    public class IncrementBuildNumber
    {
        [PostProcessBuild(1080)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            int build;
            if(Int32.TryParse(PlayerSettings.macOS.buildNumber, out build))
            {
                build++;
                Debug.Log($"OnPostProcess build #{build}");
                PlayerSettings.macOS.buildNumber = build.ToString();
                return;
            }

            Debug.LogError($"Error incrementing build number: provided string could not be converted to int");
        }
    }
}