using System;
using System.Collections.Generic;
using UnityEngine;

namespace Immersed.General
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Data", order = 1)]
    public class DataScriptableObject : ScriptableObject
    {
        public string HostUrl = "https://sveltemind.backendless.app/api/services/";
        public string PREFS_SECRET_KEY = "game_secret_key";
        public string PREFS_PROFILE_DATA = "user_profile_data";
        public int GameID = -1;
        public Texture2D ProfileAvatarPlaceholder;

        public string UserModelSchemaJson = @"{
                                              'type': 'object',
                                              'properties': {
                                                'uid': {'type': 'integer'},
                                                'us': {'type': 'boolean'},
                                                'uc': {'type': 'string'}
                                              },
                                               'required': ['uid','us','uc']
                                            }";

        public string GetLocalProfilePicPath()
        {
            return Application.persistentDataPath + "/ProfilePic.jpg";
        }
    }
}