using System;
using UnityEngine;

namespace game.models
{
    [Serializable]
    public class MetaData
    {
        public string version = "v" + Application.version;
    }
}