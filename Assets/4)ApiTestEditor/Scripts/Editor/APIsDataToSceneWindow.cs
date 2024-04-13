using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Immersed.General;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Immersed.Task4
{
    //Get all method with ExposeToAPIEditor Attribute in api class to show in editor window  
    public class APIsDataToSceneWindow : EditorWindow
    {
        private RequestManager m_requestManager;

        private DataManager m_dataManager;

        //used for create RequestBundleClass and send request
        private ServerAccess m_serverAccess;

        //store all methods with ExposeToAPIEditor attribute
        private List<MethodInfo> m_apisMethod;

        //store all apis names
        private string[] m_apisName;

        //store api parameter with type and index of it  
        private Dictionary<int, Dictionary<Type, string>> m_apiParameter;

        //api dropdown selection index
        private int selectedApisIndex = -1;
        private int m_lastselectedApisIndex = -1;

        private string resultText;

        //store status of server response
        bool serverResponseStatus;

        private APIVersion m_apiVersion;

        private RequestName m_requestName;

        //instance of UI manager in GameSceneTask4 scene
        private UIManager m_uiManager;

        [MenuItem("Custom/APIs Data To Scene Window")]
        private static void ShowWindow()
        {
            const int width = 400;
            const int height = 700;


            var window = GetWindow<APIsDataToSceneWindow>();
            window.titleContent = new GUIContent("Prefab Data Changer");
            var x = (Screen.currentResolution.width - width) / 2;
            var y = (Screen.currentResolution.height - height) / 2;
            window.position = new Rect(x, y, width, height);
            window.Show();
        }

        private void OnEnable()
        {
            //Get all method in APIs class with return type of RequestClass<UserModel>
            m_apisMethod = GetMethodsOfReturnType(typeof(APIs), typeof(RequestClass<UserModel>));
            //select all apis name from m_apisMethod
            m_apisName = m_apisMethod.Select(x => x.Name).ToArray();
            //Open GameSceneTask4 if isn't open
            if (EditorSceneManager.GetActiveScene().name != "GameSceneTask4")
                EditorSceneManager.OpenScene("Assets/4)ApiTestEditor/Scenes/GameSceneTask4.unity");

            m_serverAccess = FindObjectOfType<ServerAccess>();
            m_uiManager = FindObjectOfType<UIManager>();
            m_requestManager = FindObjectOfType<RequestManager>();
            m_requestManager.Initialize();
        }


        private void OnGUI()
        {
            GUILayout.Space(10);
            if (m_apisMethod != null)
            {
                //Create drop down list from apis name
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("API Name", GUILayout.Width(70));
                selectedApisIndex = EditorGUILayout.Popup(selectedApisIndex, m_apisName);
                EditorGUILayout.EndHorizontal();
                //---------------------------------------

                //if user select one of the list draw the api editor
                if (selectedApisIndex != -1)
                {
                    //get parameters of selected api method
                    List<ParameterInfo> parameterInfo = m_apisMethod[selectedApisIndex].GetParameters()
                        .Where(x => x.ParameterType == typeof(string) | x.ParameterType == typeof(APIVersion)).ToList();

                    //only run for new api selection to instance api parameter dictionary and fill it
                    if (selectedApisIndex != m_lastselectedApisIndex)
                    {
                        m_apiParameter = new Dictionary<int, Dictionary<Type, string>>();
                        int counter = 0;
                        parameterInfo.ForEach((x) =>
                        {
                            m_apiParameter.Add(counter, new Dictionary<Type, string>());
                            m_apiParameter[counter].Add(x.ParameterType, x.Name);
                            counter++;
                        });
                    }

                    //draw apis parameters
                    int apiParameterCounter = 0;
                    GUILayout.Space(10);
                    EditorGUILayout.LabelField("APIs Parameter : ");
                    GUILayout.Space(5);
                    parameterInfo.ForEach(x =>
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(x.Name, GUILayout.Width(70));

                        switch (x.ParameterType)
                        {
                            case Type intType when intType == typeof(APIVersion):
                            {
                                m_apiVersion = (APIVersion) EditorGUILayout.EnumPopup(m_apiVersion);
                            }
                                break;
                            case Type intType when intType == typeof(string):
                            {
                                m_apiParameter[apiParameterCounter][x.ParameterType]
                                    = EditorGUILayout.TextField(
                                        m_apiParameter[apiParameterCounter][x.ParameterType].Equals("deviceid")
                                            ? SystemInfo.deviceUniqueIdentifier
                                            : m_apiParameter[apiParameterCounter][x.ParameterType]);
                            }
                                break;
                        }

                        apiParameterCounter++;
                        EditorGUILayout.EndHorizontal();
                    });
                    //----------------------------------------------------------------
                    GUILayout.Space(10);

                    EditorGUILayout.BeginHorizontal();

                    //Draw send button
                    if (GUILayout.Button("Send"))
                    {
                        APIs.GenericCallback<UserModel> callback = ((status, userModel, responseText, message, code) =>
                        {
                            serverResponseStatus = status;
                            if (status)
                            {
                                m_uiManager.SetUserModelToUI(userModel.UserClass, userModel);

                                if ((RequestName) Enum.Parse(typeof(RequestName),
                                    m_apisName[m_lastselectedApisIndex]) == RequestName.AnonymousLogin)
                                {
                                    Debug.Log("Is Guest Register");
                                }
                                else if ((RequestName) Enum.Parse(typeof(RequestName)
                                    , m_apisName[m_lastselectedApisIndex]) == RequestName.EmailLogin)
                                {
                                    Debug.Log("Is Email Login");
                                    
                                    if (!string.IsNullOrEmpty(userModel.ProfilePicUrl))
                                        GetProfilePicture(userModel.ProfilePicUrl);
                                    else
                                        m_uiManager.SetProfileImage(null);
                                        
                                }
                                else if((RequestName) Enum.Parse(typeof(RequestName)
                                    , m_apisName[m_lastselectedApisIndex]) == RequestName.Register)
                                {
                                    m_uiManager.HideAllView();
                                }
                            }

                            resultText = responseText.FormatJson();
                        });
                        List<object> parameters = new List<object> {m_apiVersion, callback, false, false};


                        foreach (var apiParam in m_apiParameter.Values.Where(x => x.ContainsKey(typeof(string))))
                        {
                            parameters.Add(apiParam[typeof(string)]);
                        }

                        RequestClass<UserModel> requestClass = (RequestClass<UserModel>) m_apisMethod[selectedApisIndex]
                            .Invoke(new APIs(DataManager.Data), parameters.ToArray());

                        RequestBundle bndleReq = new RequestBundle(requestClass);
                        m_serverAccess.SendBatchRequest(bndleReq);
                    }

                    //draw cancel button
                    if (GUILayout.Button("Cancel"))
                    {
                    }

                    EditorGUILayout.EndHorizontal();

                    //draw result text filed
                    GUILayout.Space(20);
                    EditorGUILayout.BeginHorizontal();
                    GUIStyle s = new GUIStyle(EditorStyles.textField);
                    s.normal.textColor = Color.red;
                    if (serverResponseStatus)
                        s.normal.textColor = Color.green;
                    EditorGUILayout.LabelField("Result", GUILayout.Width(70));
                    resultText = EditorGUILayout.TextField(resultText, s, GUILayout.Height(500));
                    EditorGUILayout.EndHorizontal();
                    m_lastselectedApisIndex = selectedApisIndex;
                }
            }
        }

        public void GetProfilePicture(string profilePictureUrl)
        {
            Request requestClass = m_requestManager.GetProfilePicRequest(profilePictureUrl,
                ((status, profilePicBytes, text, message, code) =>
                {
                    Texture2D texture2d = new Texture2D(150, 150);
                    texture2d.LoadImage(profilePicBytes);
                    m_uiManager.SetProfileImage(texture2d);
                }));

            RequestBundle bndleReq = new RequestBundle(requestClass);
            m_serverAccess.SendBatchRequest(bndleReq);
        }

        public List<MethodInfo> GetMethodsOfReturnType(Type cls, Type ret)
        {
            // Did you really mean to prohibit public methods? I assume not
            var methods = cls.GetMethods(BindingFlags.NonPublic |
                                         BindingFlags.Public |
                                         BindingFlags.Instance | BindingFlags.DeclaredOnly);
            List<MethodInfo> retMethods = methods
                .Where(m => m.GetCustomAttributes(typeof(ExposeToAPIEditorAttribute), false).Length > 0)
                .Where(m => m.ReturnType.IsAssignableFrom(ret))
                .Select(m => m).ToList();

            return retMethods;
        }
    }
}