using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.UIElements;

namespace DropBallEditor
{
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : Editor
    {
        LevelManager levelManager;
        private int selectedLevelNumber;
        string strSelectedLevel = "";

        public override VisualElement CreateInspectorGUI()
        {
            levelManager = target as LevelManager;
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ShowArrayProperty(serializedObject.FindProperty("Levels"));

            if (GUILayout.Button("Add new Level"))
            {
                levelManager.Levels.Add(new Level());
            }

            if (GUILayout.Button("Load Data"))
            {
                levelManager.Levels = JsonConvert.DeserializeObject<List<Level>>(
                    File.ReadAllText(Path.Combine(GetResourcesDirectory(), "LevelsData.json")));
            }

            if (GUILayout.Button("Save Data"))
            {
                string data = JsonConvert.SerializeObject(levelManager.Levels,
                    new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    });

                File.WriteAllText(Path.Combine(GetResourcesDirectory(), "LevelsData.json"), data);
                AssetDatabase.Refresh();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Level Index",  GUILayout.Width(100));
            strSelectedLevel = GUILayout.TextField(strSelectedLevel);
            GUILayout.EndHorizontal();

            int.TryParse(strSelectedLevel, out selectedLevelNumber);

            if (GUILayout.Button("Place Levels"))
            {
                GameManager.Instance.ResetPlatforms();
                GameManager.Instance.Initialize(selectedLevelNumber);
            }

            if (GUILayout.Button("Reset Levels"))
            {
                GameManager.Instance.ResetPlatforms();
            }
        }

        public void ShowArrayProperty(SerializedProperty list)
        {
            EditorGUI.indentLevel += 1;

            serializedObject.Update();

            list.arraySize = EditorGUILayout.IntField("Level Numbers", list.arraySize);

            EditorGUI.indentLevel += 2;
            for (int i = 0; i < list.arraySize; i++)
            {
                SerializedProperty levelProperty = list.GetArrayElementAtIndex(i);
                SerializedProperty platformsDataArray = levelProperty.FindPropertyRelative("PlatformData");

                if (EditorGUILayout.PropertyField(levelProperty, new GUIContent("Level" + (i + 1).ToString()), false))
                {
                    EditorGUI.indentLevel += 1;

                    Level level = levelManager.Levels[i];
                    level.BackgroundColor.Color =
                        EditorGUILayout.ColorField("Background Color", level.BackgroundColor.Color);
                    level.MainBarColor.Color = EditorGUILayout.ColorField("Main Bar Color", level.MainBarColor.Color);
                    level.GoalPlatformColor.Color =
                        EditorGUILayout.ColorField("Goal Platform Color", level.GoalPlatformColor.Color);
                    level.FoulPlatformColor.Color =
                        EditorGUILayout.ColorField("Foul Platform  Color", level.FoulPlatformColor.Color);
                    level.NeutralPlatformColor.Color =
                        EditorGUILayout.ColorField("Neutral Platform Color", level.NeutralPlatformColor.Color);
                    level.BallColor.Color = EditorGUILayout.ColorField("Ball Color", level.BallColor.Color);

                    level.BallMass = EditorGUILayout.IntField("Ball Mass", level.BallMass);

                    platformsDataArray.arraySize = EditorGUILayout.IntField("Platform Data Size", platformsDataArray.arraySize);

                    for (int j = 0; j < platformsDataArray.arraySize; j++)
                    {
                        SerializedProperty platformData = platformsDataArray.GetArrayElementAtIndex(j);
                        EditorGUILayout.PropertyField(platformData, new GUIContent("Platform Data " + j), true);
                    }

                    EditorGUI.indentLevel -= 1;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        public static string GetResourcesDirectory()
        {
            string resourcesPath = Directory.GetDirectories(Application.dataPath).ToList()
                .Find(x => x.Contains(@"\Resources"));

            if (resourcesPath == null)
            {
                resourcesPath = Path.Combine(Application.dataPath, "Resources");
                Directory.CreateDirectory(resourcesPath);
            }

            return resourcesPath;
        }
    }
}
