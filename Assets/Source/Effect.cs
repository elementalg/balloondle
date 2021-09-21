using System;
using UnityEngine;

namespace Balloondle
{
    [Serializable]
    public struct Effect
    {
        public string m_Name;
        public bool m_IsOnlySound;
        public GameObject m_Prefab;
        public AudioClip m_Sound;
    }
}