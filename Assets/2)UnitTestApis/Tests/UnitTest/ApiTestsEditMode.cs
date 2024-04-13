using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using Immersed.General;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ApiTestsEditMode
{
    private static APIs m_apIs;
    private RequestManager m_requestManager;
    private ServerAccess m_serverAccess;
    private DataManager m_dataManager;

    [SetUp]
    [LoadScene("Assets/Scenes/APIsTestsScene.unity")]
    public void SetUp()
    {
        Debug.unityLogger.LogFormat(LogType.Log, $"<color=#00AAFF>Check RequestManager Component</color>");
        m_requestManager = Object.FindObjectOfType<RequestManager>();
        Assert.IsNotNull(m_requestManager, "RequestManager isn't in APIsTests Scene");
        m_requestManager.Initialize();

        Debug.unityLogger.LogFormat(LogType.Log, $"<color=#00AAFF>Check ServerAccess Component</color>");
        m_serverAccess = Object.FindObjectOfType<ServerAccess>();
        Assert.IsNotNull(m_requestManager, "ServerAccess isn't in APIsTests Scene");

        Debug.unityLogger.LogFormat(LogType.Log, $"<color=#00AAFF>Check DataManager  Component</color>");
        m_dataManager = Object.FindObjectOfType<DataManager>();
        Assert.IsNotNull(m_dataManager, "DataManager isn't in APIsTests Scene");

        Debug.unityLogger.LogFormat(LogType.Log, $"<color=#00AAFF>Check base url validation</color>");
        Regex regex = new Regex(@"^(https:\/\/)[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");
        Assert.IsTrue(regex.IsMatch(RequestHelper.Instance.BaseUrl), "Base Url has problem");
    }

    // A Test behaves as an ordinary method
    [UnityTest]
    public IEnumerator GuestLoginTest()
    {
        string serverResponse = null;

        m_requestManager.OnResponseUserEvent = new OnResponseEvent<string, UserModel, bool>();
        m_requestManager.OnResponseUserEvent.OnResponse += (dataString, user, arg3) => { serverResponse = dataString; };

        Debug.unityLogger.LogFormat(LogType.Log,
            "<color=#FFAAFF>Guest Login Request Started --------------------------------------------------------</color>");

        Request guestLoginRequest = m_requestManager.GetRequest(new RequestNameWithApiVersion(RequestName.AnonymousLogin,APIVersion.V1));
        guestLoginRequest.AddBody("deviceid", SystemInfo.deviceUniqueIdentifier);
        Debug.unityLogger.LogFormat(LogType.Log, $"<color=#00AAFF>Created Request bundle class</color>");
        RequestBundle bndleReq = new RequestBundle(guestLoginRequest);
        m_serverAccess.SendBatchRequest(bndleReq);

        yield return new WaitWhile(() => serverResponse == null);

        Debug.Log($"<color=#00FF00>Response Text: {serverResponse}</color>");
        Assert.IsNotEmpty(serverResponse, "Server response is null");

        Debug.Log($"<color=#00FF00>deserialize Response to User Model :------------------</color>");

        DeSerializeServerResponseToUserModel(serverResponse);
    }

    [UnityTest]
    public IEnumerator RegisterTest()
    {
        string serverResponse = null;

        m_requestManager.OnResponseUserEvent = new OnResponseEvent<string, UserModel, bool>();
        m_requestManager.OnResponseUserEvent.OnResponse += (dataString, user, arg3) => { serverResponse = dataString; };

        Debug.unityLogger.LogFormat(LogType.Log,
            "<color=#FFAAFF>Register Request Started --------------------------------------------------------</color>");

        Request requestClass = m_requestManager.GetRequest(new RequestNameWithApiVersion(RequestName.Register,APIVersion.V1));

        string randomEmail = null;
        string randomPassword = null;
        string randomNickname = null;

        string emailCharacter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        StringBuilder salt = new StringBuilder();
        randomEmail += "test@";

        while (salt.Length < 4)
        {
            // length of the random string.
            int index = (int) (Random.Range(0, emailCharacter.Length));
            salt.Append(emailCharacter[index]);
        }

        randomEmail += salt.ToString() + ".com";
        randomNickname = salt.ToString();

        string passwordCharacter = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        StringBuilder res = new StringBuilder();
        for (int i = 0; i < 8; i++)
        {
            res.Append(passwordCharacter[Random.Range(0, passwordCharacter.Length)]);
        }

        randomPassword = res.ToString();

        Debug.unityLogger.LogFormat(LogType.Log, $"<color=#00AAFF>Random Email is {randomEmail}</color>");
        Debug.unityLogger.LogFormat(LogType.Log, $"<color=#00AAFF>Random Password is {randomPassword}</color>");
        Debug.unityLogger.LogFormat(LogType.Log, $"<color=#00AAFF>Random Nickname is {randomNickname}</color>");

        requestClass.AddBody("email", randomEmail);
        requestClass.AddBody("password", randomPassword);
        requestClass.AddBody("nickname", randomNickname);
        requestClass.AddBody("deviceid", SystemInfo.deviceUniqueIdentifier);
        RequestBundle bndleReq = new RequestBundle(requestClass);
        Debug.unityLogger.LogFormat(LogType.Log, $"<color=#00AAFF>Created Request bundle class</color>");
        m_serverAccess.SendBatchRequest(bndleReq);

        yield return new WaitWhile(() => serverResponse == null);

        Debug.Log($"<color=#00FF00>Response Text: {serverResponse}</color>");
        Assert.IsNotEmpty(serverResponse, "Server response is null");

        Debug.Log($"<color=#00FF00>deserialize Response to User Model :------------------</color>");

        DeSerializeServerResponseToUserModel(serverResponse);
    }

    [UnityTest]
    public IEnumerator LoginWithToken()
    {
        string serverResponse = null;

        m_requestManager.OnResponseUserEvent = new OnResponseEvent<string, UserModel, bool>();
        m_requestManager.OnResponseUserEvent.OnResponse += (dataString, user, arg3) => { serverResponse = dataString; };
        string authenticationToken = null;
        if (PlayerPrefs.HasKey(DataManager.Data.PREFS_SECRET_KEY))
        {
            string encrypted = PlayerPrefs.GetString(DataManager.Data.PREFS_SECRET_KEY);
            authenticationToken = AES.Decrypt(encrypted);
        }


        Debug.unityLogger.LogFormat(LogType.Log,
            "<color=#FFAAFF>Login with Authentication Token Request Started --------------------------------------------------------</color>");

        Request requestClass = m_requestManager.GetRequest(new RequestNameWithApiVersion(RequestName.GetDataWithAuthToken,APIVersion.V1));

        Debug.unityLogger.LogFormat(LogType.Log,
            $"<color=#00AAFF>Saved Authentication Token is {authenticationToken}</color>");

        RequestBundle bndleReq = new RequestBundle(requestClass);
        Debug.unityLogger.LogFormat(LogType.Log, $"<color=#00AAFF>Created Request bundle class</color>");
        requestClass.AddHeader("Host", $"x8ki-letl-twmt.n7.xano.io");
        requestClass.AddHeader("Authorization", $"Bearer {authenticationToken}");
        m_serverAccess.SendBatchRequest(bndleReq);

        yield return new WaitWhile(() => serverResponse == null);

        Debug.Log($"<color=#00FF00>Response Text: {serverResponse}</color>");
        Assert.IsNotEmpty(serverResponse, "Server response is null");

        Debug.Log($"<color=#00FF00>deserialize Response to User Model :------------------</color>");

        DeSerializeServerResponseToUserModel(serverResponse);
    }


    [UnityTest]
    public IEnumerator LoginV1Test()
    {
        string serverResponse = null;

        m_requestManager.OnResponseUserEvent = new OnResponseEvent<string, UserModel, bool>();
        m_requestManager.OnResponseUserEvent.OnResponse += (dataString, user, arg3) => { serverResponse = dataString; };

        Debug.unityLogger.LogFormat(LogType.Log,
            "<color=#FFAAFF>Login with email and pass Request Started --------------------------------------------------------</color>");

        Request requestClass = m_requestManager.GetRequest(new RequestNameWithApiVersion(RequestName.EmailLogin,APIVersion.V1));

        string email = "test@test.com";
        string password = "123456";

        Debug.unityLogger.LogFormat(LogType.Log, $"<color=#00AAFF>Email is {email}</color>");
        Debug.unityLogger.LogFormat(LogType.Log, $"<color=#00AAFF>Password is {password}</color>");

        requestClass.AddBody("email", email);
        requestClass.AddBody("password", password);

        RequestBundle bndleReq = new RequestBundle(requestClass);
        Debug.unityLogger.LogFormat(LogType.Log, $"<color=#00AAFF>Created Request bundle class</color>");
        m_serverAccess.SendBatchRequest(bndleReq);

        yield return new WaitWhile(() => serverResponse == null);

        Debug.Log($"<color=#00FF00>Response Text: {serverResponse}</color>");
        Assert.IsNotEmpty(serverResponse, "Server response is null");

        Debug.Log($"<color=#00FF00>deserialize Response to User Model :------------------</color>");

        DeSerializeServerResponseToUserModel(serverResponse);
    }

    [UnityTest]
    public IEnumerator LoginV2Test()
    {
        string serverResponse = null;

        m_requestManager.OnResponseUserEvent = new OnResponseEvent<string, UserModel, bool>();
        m_requestManager.OnResponseUserEvent.OnResponse += (dataString, user, arg3) => { serverResponse = dataString; };

        Debug.unityLogger.LogFormat(LogType.Log,
            "<color=#FFAAFF>Login with email and pass Request Started --------------------------------------------------------</color>");

        Request requestClass = m_requestManager.GetRequest(new RequestNameWithApiVersion(RequestName.EmailLogin,APIVersion.V2));

        string email = "test@test.com";
        string password = "123456";

        Debug.unityLogger.LogFormat(LogType.Log, $"<color=#00AAFF>Email is {email}</color>");
        Debug.unityLogger.LogFormat(LogType.Log, $"<color=#00AAFF>Password is {password}</color>");

        requestClass.AddBody("email", email);
        requestClass.AddBody("password", password);

        RequestBundle bndleReq = new RequestBundle(requestClass);
        Debug.unityLogger.LogFormat(LogType.Log, $"<color=#00AAFF>Created Request bundle class</color>");
        m_serverAccess.SendBatchRequest(bndleReq);

        yield return new WaitWhile(() => serverResponse == null);

        Debug.Log($"<color=#00FF00>Response Text: {serverResponse}</color>");
        Assert.IsNotEmpty(serverResponse, "Server response is null");

        Debug.Log($"<color=#00FF00>deserialize Response to User Model :------------------</color>");

        DeSerializeServerResponseToUserModel(serverResponse);
    }
    

    void DeSerializeServerResponseToUserModel(string serverResponse)
    {
        bool jsonParsSuccess = serverResponse.TryParseJson<UserModel>(DataManager.Data.UserModelSchemaJson) != null;

        if (!jsonParsSuccess)
            Debug.Log($"<color=#FF0000>Failed to Deserialize response to User Model</color>");

        Assert.True(jsonParsSuccess, "Failed to parse Server Response to UserModel");
        Debug.Log($"<color=#00FF00>Deserialize to User Model Success</color>");
    }
}

public class LoadSceneAttribute : NUnitAttribute, IOuterUnityTestAction
{
    private string scene;

    public LoadSceneAttribute(string scene)
    {
        this.scene = scene;
    }

    IEnumerator IOuterUnityTestAction.BeforeTest(ITest test)
    {
        Debug.Assert(scene.EndsWith(".unity"));
        Scene loadedScene = EditorSceneManager.OpenScene(this.scene);
        EditorSceneManager.SetActiveScene(loadedScene);
        yield return null;
    }

    IEnumerator IOuterUnityTestAction.AfterTest(ITest test)
    {
        yield return null;
    }
}