using System;
using UnityEditor;

namespace Editor
{
    public class BuildScript
    {
        public static void PerformBuild()
        {
            var outputPath = Environment.GetEnvironmentVariable("BUILD_PATH") ?? "/tmp";
            string[] scenes = {"Assets/Scenes/MainMenu.unity", "Assets/Scenes/Game.unity"};
            BuildPipeline.BuildPlayer(scenes, outputPath, BuildTarget.WebGL, BuildOptions.None);
        }
    }
}