## Assignment:
You need to incorporate data from a new UserInfo endpoint from the Immersed API into the Immersed Unity application.

Data (JSON) for a given user (UserInfo) looks something like...

```csharp
{
“uid” : 123456789,
“name” : “Bob Smith”,
“nickname” : “bsmith2002”,
“links” : {“https://www.linkedin.com/in/bsmith2002”, “...”, “...”},
“profilePic” : “https://www.datastorage.com/bsmith2002.png”,
“paidUser” : true,
.
}
```

You do not need to implement the Web Request layer, but you may need to create a system to serve mock JSON data in an offline-testing context.

1) Write the systems necessary to display (runtime), create, fetch, store/cache (local file storage is fine), and validate user information. Assume the data’s structure can change in future API updates. (For example, missing or invalid fields) UserInfo must be encrypted to maintain privacy and security.

2) Write the Unit Tests necessary to ensure system functionality and flexibility for handling bad data, failed requests, etc. Be explicit and create tests around any potential edge cases. Unit Tests should also be preventative - they should ensure the base functionality of the application in the case of any major architecture changes to the API.

3) Create a scene featuring a way to access and display UserInfo in VR (does not have to run in VR, can just be a 3D scene.

4) Create an editor tool to quickly mock and review UserInfo for testing the scene while it runs in the editor. Runtime viewing of data through GameObjects and the inspector view is also preferable to avoid unnecessary breakpoints and additional overhead to verify data systems are functioning.


## Idea:

The main idea is to create a generic request class to handle the deserialization of response type and can send parallel requests to the server if the server support gets separate parameters, I created all of the Requests for communicating to a server in APIs class as methods, need to set the API version and Request Name in each request, the only API has two versions is EmailLogin, the second version have an extra coin field in the response, to show how I handle the APIs update field in different versions, can get a request from a method in RequestManager class, I created ScriptsGeneral folder in the Assets folder and all of the scripts used is more than one Task in it and each folder have different AssemblyDefinition on it for using them in the Unit test. I created a scriptable object to store the Host base URL and PlayerPrefs keys for saving/loading local data of player and UserModel JSON Schema for validating the JSON from the server, also save the profile picture of Player to Application.persistentDataPath.

Task 1: I created APIs as unity UI to test them on different platforms.</br>

[![Video Title](http://i3.ytimg.com/vi/NrUhsbxx66o/hqdefault.jpg)](https://youtu.be/NrUhsbxx66o)
--------------------------------------------------------------------------------------------------------------

Task 2: I create Unit Test in edit mode for test and validate the server response JSON.</br>

[![Video Title](http://i3.ytimg.com/vi/VpLRsJ-_nvQ/hqdefault.jpg)](https://youtu.be/VpLRsJ-_nvQ)
--------------------------------------------------------------------------------------------------------------

Task 3: I created unity UI in 3d scene and send a login request to a server and show the server response on it.</br>

[![Video Title](http://i3.ytimg.com/vi/hDCkmdhuqx0/hqdefault.jpg)](https://youtu.be/hDCkmdhuqx0)
--------------------------------------------------------------------------------------------------------------

Task 4: I created an Editor window in "Custom/APIs Data To Scene Window" to send a request to the server and show the response in a scene UI elements on edit mode, I get all of the requests has Expose attribute with reflection from APIs class and create Editor elements with them.

[![Video Title](http://i3.ytimg.com/vi/5MqbEeMFIu4/hqdefault.jpg)](https://youtu.be/5MqbEeMFIu4)
--------------------------------------------------------------------------------------------------------------


## Documentation:

RequestClass: created generic RequestClass to create request depending on data we got from the server, in the project the server response is UserModel the response JSON deserialize to it, the request class has inherited from RequestBase class, I used an HTTPRequest as request object in RequestBase class.

RequestBundle: can create a list of Requests and send them to the server and each of them has a callback, for example, if we can get parameters separately from the server we can use it, especially for no-SQL databases  

server access: for sending RequestBundle to the server, able to create multi request to the server and each of them have callback execute when server responded

APIs: created all of the Requests as methods to can get them with a RequestName enum, each of request method creates a RequestClass with UserModel type as a callback return type
RequestBuilder: for creating RequestClass depend on the request verb(Get, Post) and URL body part, headers, and body of it

GenericCallback<T>: used for creating call back and setting the response type of it.