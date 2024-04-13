using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace Immersed.General
{
    public class UserModel
    {
        [JsonProperty("at")]
        public string AuthToken { get; set; }

        [JsonRequired]
        [JsonProperty("uid", Required = Required.Always)]
        public long UserID { get; set; }

        [JsonRequired]
        [JsonProperty("us", Required = Required.Always)]
        public bool UserStatus { get; set; }

        [JsonRequired]
        [JsonConverter(typeof(EnumerationConverter))]
        [JsonProperty("uc", Required = Required.Always)]
        public UserClass UserClass { get; set; }

        [JsonProperty("nn")] public string Nickname { get; set; }

        [JsonProperty("lnk")] public string[] Links { get; set; }

        [DefaultValue(false)]
        [JsonProperty("pu")]
        public bool PaidUser { get; set; }

        [JsonProperty("ppu")] public string ProfilePicUrl;
        [JsonProperty("e")] public string email { get; set; }

        [JsonProperty("c", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Coins { get; set; }
    }
}