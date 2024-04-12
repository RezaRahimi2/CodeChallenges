using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace Assessment.EditorTools.Scripts.Editor
{
    public class PrefabsDataChanger : EditorWindow
    {
        [Serializable]
        struct Data
        {
            [SerializeField] public string text;
            [SerializeField] public Color Color;

            [SerializeField]
            public string color
            {
                set { ColorUtility.TryParseHtmlString(value, out Color); }
            }

            [SerializeField] public Texture2D Texture;

            [SerializeField]
            public string image
            {
                set
                {
                    Texture = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(@"Assets/Assessment/EditorTools/",
                        value));
                }
            }
        }

        private string path = "";
        private string jsonDataPath = "";
        private bool haveValidPath;
        private Texture openFolderIcon;
        private Type[] allType;
        private static string[] allTypeName;
        private int selectedTypeIndex = -1;
        private int lastSelectedPrefabIndex = -1;
        private bool preafabsSearched;

        private Data[] dataList;
        private bool selectedPrefabHaveTextComponents;
        private bool selectedPrefabHaveImageComponents;
        private string selectedPrefabTextValue;
        private Texture selectedPrefabImageValue;
        private Color selectedPrefabColorValue;
        private bool enableApplyRevertButtons;

        private Rect lastRect;

        private SerializedObject _serializedObject;
        public List<GameObject> foundPrefabs;
        Vector2 scrollPosition;

        [MenuItem("Custom/Prefab Data Changer")]
        private static void ShowWindow()
        {
            const int width = 400;
            const int height = 700;


            var window = GetWindow<PrefabsDataChanger>();
            window.titleContent = new GUIContent("Prefab Data Changer");
            var x = (Screen.currentResolution.width - width) / 2;
            var y = (Screen.currentResolution.height - height) / 2;
            window.position = new Rect(x, y, width, height);
            window.Show();
        }

        private void OnEnable()
        {
            string openFolderIconPath =
                AssetDatabase.FindAssets("open-folder-icon t:texture", new[] {"Assets/Assessment/EditorTools"})?[0];

            if (openFolderIconPath == null)
                throw new NullReferenceException("Couldn't found open-folder-icon Texture");

            if (haveValidPath)
                GetTypes();

            foundPrefabs = new List<GameObject>();
            dataList = null;
            openFolderIcon = AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(openFolderIconPath));
        }

        private void GetTypes()
        {
            var _allGraphicType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Graphic)) &&
                               type.IsAssignableFrom(type)).ToArray();

            var _allScriptType = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => (!assembly.FullName.Contains("Unity") && !assembly.FullName.Contains("Editor")))
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(MonoBehaviour)) &&
                               type.IsAssignableFrom(type)).ToArray();

            allType = new Type[_allGraphicType.Length + _allScriptType.Length];

            int counter = 0;

            for (var i = 0; i < _allScriptType.Length; i++)
            {
                allType[counter++] = _allScriptType[i];
            }

            for (var i = 0; i < _allGraphicType.Length; i++)
            {
                allType[counter++] = _allGraphicType[i];
            }

            allTypeName = allType.Select(x => x.Name).ToArray();
        }

        private void OnGUI()
        {
            GUILayout.Space(5);

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();

            GUI.skin.textField.fontSize = 12;
            GUI.skin.textField.alignment = TextAnchor.MiddleLeft;

            GUILayout.BeginVertical();
            GUILayout.Label("Search Directory");
            path = GUILayout.TextField(path, 2000, GUILayout.Width(Screen.width - 40), GUILayout.Height(30));
            GUILayout.Label("Json Data File Path");
            jsonDataPath = GUILayout.TextField(jsonDataPath, 2000, GUILayout.Width(Screen.width - 40),
                GUILayout.Height(30));
            GUILayout.EndVertical();

            GUILayout.BeginArea(new Rect(Screen.width - 35, 20, Screen.width, 30));
            if (GUILayout.Button(openFolderIcon, EditorStyles.miniButtonLeft, GUILayout.MaxHeight(30),
                GUILayout.MaxWidth(30)))
            {
                path = EditorUtility.OpenFolderPanel("Select a Folder to search", "", "");

                Regex regex = new Regex("(?=Assets).*");
                Match match = regex.Match(path);

                if (match.Success)
                {
                    path = match.Value;
                    haveValidPath = true;
                    GetTypes();
                }
            }

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(Screen.width - 35, 70, Screen.width, 30));
            if (GUILayout.Button(openFolderIcon, EditorStyles.miniButtonLeft, GUILayout.MaxHeight(30),
                GUILayout.MaxWidth(30)))
            {
                jsonDataPath = EditorUtility.OpenFilePanel("Select Data JSON file", "", "json");

                dataList = Newtonsoft.Json.JsonConvert.DeserializeObject<Data[]>(File.ReadAllText(jsonDataPath));
            }

            GUILayout.EndArea();
            GUILayout.EndHorizontal();

            if (dataList != null && dataList.Length > 0)
            {
                GUI.color = Color.green;
                GUILayout.Label("Data file Loaded");
                GUI.color = Color.white;


                float ypos = 0;
                foreach (var data in dataList)
                {
                    ypos += 25;
                }
            }

            if (haveValidPath)
                selectedTypeIndex = EditorGUILayout.Popup("Type", selectedTypeIndex, allTypeName);

            GUILayout.BeginHorizontal();
            GUI.skin.button.alignment = TextAnchor.MiddleCenter;
            GUI.enabled = haveValidPath;
            if (GUILayout.Button("Search Prefabs", EditorStyles.miniButton, GUILayout.MaxHeight(30),
                GUILayout.MaxWidth(100)))
            {
                preafabsSearched = true;
                foundPrefabs = new List<GameObject>();
                lastSelectedPrefabIndex = -1;
                string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] {path});

                foreach (string guid in guids)
                {
                    //Debug.Log(AssetDatabase.GUIDToAssetPath(guid));
                    string myObjectPath = AssetDatabase.GUIDToAssetPath(guid);
                    GameObject obj = AssetDatabase.LoadMainAssetAtPath(myObjectPath) as GameObject;

                    if (obj.GetComponentInChildren(allType[selectedTypeIndex]))
                    {
                        foundPrefabs.Add(obj);
                        Debug.Log($"Prefab name {obj.name} with {allTypeName[selectedTypeIndex]} Component");
                    }
                }
            }

            if (GUILayout.Button("Fill Prefabs with Data", EditorStyles.miniButton, GUILayout.MaxHeight(30),
                GUILayout.MaxWidth(150)))
            {
                foreach (var data in dataList)
                {
                    GameObject go = foundPrefabs.Find(x => x.name.Equals(data.text));
                    go.GetComponentInChildren<Text>().text = data.text;

                    RawImage image = go.GetComponentInChildren<RawImage>();
                    image.texture = data.Texture;
                    image.color = data.Color;
                }

                enableApplyRevertButtons = true;
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (haveValidPath && foundPrefabs != null && foundPrefabs.Count > 0)
            {
                scrollPosition = GUILayout.BeginScrollView(
                    scrollPosition, GUILayout.Width(100), GUILayout.Height(200));

                Color color_default = GUI.backgroundColor;
                Color color_selected = Color.gray;

                GUIStyle itemStyle = new GUIStyle(GUI.skin.button); //make a new GUIStyle

                itemStyle.alignment = TextAnchor.MiddleLeft; //align text to the left
                itemStyle.active.background = itemStyle.normal.background; //gets rid of button click background style.
                itemStyle.margin =
                    new RectOffset(0, 0, 0, 0);

                for (int i = 0; i < foundPrefabs.Count; i++)
                {
                    GUI.backgroundColor = (lastSelectedPrefabIndex == i) ? color_selected : Color.clear;

                    if (GUILayout.Button(foundPrefabs[i].name, itemStyle))
                    {
                        lastSelectedPrefabIndex = i;

                        Text textComponent = foundPrefabs[lastSelectedPrefabIndex].GetComponentInChildren<Text>();

                        RawImage imageComponent =
                            foundPrefabs[lastSelectedPrefabIndex].GetComponentInChildren<RawImage>();

                        if (textComponent != null)
                        {
                            selectedPrefabHaveTextComponents = true;
                            selectedPrefabTextValue = textComponent.text;
                        }

                        if (imageComponent != null)
                        {
                            selectedPrefabHaveImageComponents = true;
                            selectedPrefabImageValue = imageComponent.texture;
                            selectedPrefabColorValue = imageComponent.color;
                        }
                    }

                    GUI.backgroundColor = color_default; //this is to avoid affecting other GUIs outside of the list
                }

                GUILayout.EndScrollView();
            }


            if (lastSelectedPrefabIndex != -1)
            {
                EditorGUI.BeginChangeCheck();

                lastRect = GUILayoutUtility.GetLastRect();
                if (selectedPrefabHaveTextComponents)
                {
                    selectedPrefabTextValue =
                        EditorGUI.TextField(
                            new Rect(new Vector2(lastRect.x + lastRect.width + 50, lastRect.y),
                                new Vector2(200, 20)), selectedPrefabTextValue);
                }

                lastRect = GUILayoutUtility.GetLastRect();

                if (selectedPrefabHaveImageComponents)
                {
                    selectedPrefabColorValue = EditorGUI.ColorField(
                        new Rect(new Vector2(lastRect.x + lastRect.width + 50, lastRect.y + 20), new Vector2(100, 20)),
                        selectedPrefabColorValue);

                    selectedPrefabImageValue = (Texture) EditorGUI.ObjectField(
                        new Rect(new Vector2(lastRect.x + lastRect.width + 50, lastRect.y + 45), new Vector2(150, 200)),
                        "",
                        selectedPrefabImageValue,
                        typeof(Texture));
                }

                GUILayout.EndHorizontal();

                lastRect = GUILayoutUtility.GetLastRect();

                if (EditorGUI.EndChangeCheck())
                    enableApplyRevertButtons = true;

                GUI.enabled = enableApplyRevertButtons;
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Apply"))
                {
                    GUI.FocusControl(null);
                    using (var editScope =
                        new EditPrefabAssetScope(AssetDatabase.GetAssetPath(foundPrefabs[lastSelectedPrefabIndex])))
                    {
                        RawImage rawImage = editScope.prefabRoot.GetComponentInChildren<RawImage>();
                        rawImage.texture = selectedPrefabImageValue;
                        rawImage.color = selectedPrefabColorValue;
                        editScope.prefabRoot.GetComponentInChildren<Text>().text = selectedPrefabTextValue;
                    }

                    enableApplyRevertButtons = false;
                }

                if (GUILayout.Button("Revert"))
                {
                    GUI.FocusControl(null);

                    RawImage rawImage = foundPrefabs[lastSelectedPrefabIndex].GetComponentInChildren<RawImage>();

                    selectedPrefabImageValue =
                        rawImage.texture;

                    selectedPrefabColorValue = rawImage.color;

                    selectedPrefabTextValue =
                        foundPrefabs[lastSelectedPrefabIndex].GetComponentInChildren<Text>().text;

                    enableApplyRevertButtons = false;
                }

                GUILayout.EndHorizontal();

                if (GUILayout.Button("Apply All"))
                {
                    GUI.FocusControl(null);

                    for (var i = 0; i < foundPrefabs.Count; i++)
                    {
                        using (var editScope =
                            new EditPrefabAssetScope(AssetDatabase.GetAssetPath(foundPrefabs[i])))
                        {
                            if (i == lastSelectedPrefabIndex)
                            {
                                editScope.prefabRoot.GetComponentInChildren<RawImage>().texture =
                                    selectedPrefabImageValue;
                                editScope.prefabRoot.GetComponentInChildren<RawImage>().color =
                                    selectedPrefabColorValue;
                                editScope.prefabRoot.GetComponentInChildren<Text>().text = selectedPrefabTextValue;
                            }
                            else
                            {
                                editScope.prefabRoot.GetComponentInChildren<RawImage>().texture = dataList[i].Texture;
                                editScope.prefabRoot.GetComponentInChildren<RawImage>().color = dataList[i].Color;
                                editScope.prefabRoot.GetComponentInChildren<Text>().text = dataList[i].text;
                            }
                        }
                    }

                    enableApplyRevertButtons = false;
                }

                if (GUILayout.Button("Clear All Changes"))
                {
                    GUI.FocusControl(null);

                    for (var i = 0; i < foundPrefabs.Count; i++)
                    {
                        using (var editScope =
                            new EditPrefabAssetScope(AssetDatabase.GetAssetPath(foundPrefabs[i])))
                        {
                            editScope.prefabRoot.GetComponentInChildren<RawImage>().texture = null;
                            editScope.prefabRoot.GetComponentInChildren<RawImage>().color = Color.white;
                            editScope.prefabRoot.GetComponentInChildren<Text>().text = "";
                        }
                    }

                    enableApplyRevertButtons = false;
                }
            }

            GUILayout.EndVertical();
        }
    }
}

public class EditPrefabAssetScope : IDisposable
{
    public readonly string assetPath;
    public readonly GameObject prefabRoot;

    public EditPrefabAssetScope(string assetPath)
    {
        this.assetPath = assetPath;
        prefabRoot = PrefabUtility.LoadPrefabContents(assetPath);
    }

    public void Dispose()
    {
        PrefabUtility.SaveAsPrefabAsset(prefabRoot, assetPath);
        PrefabUtility.UnloadPrefabContents(prefabRoot);
    }
}