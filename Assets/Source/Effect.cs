using System;
using UnityEngine;

namespace Balloondle
{
    [Serializable]
    public struct Effect
    {
        public string Name;
        public bool IsOnlySound;
        public GameObject Prefab;
        public AudioClip Sound;
    }
}