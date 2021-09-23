using System;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Ambient
{
    [Serializable]
    public class AmbientPlaylist
    {
        public string m_Name;
        public List<AmbientSound> m_Sounds;
    }
}