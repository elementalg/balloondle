using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Ambient
{
    [CreateAssetMenu(fileName = "AmbientSoundsDefinition", menuName = "Ambient/Sounds Definition", order = 0)]
    public class AmbientPlaylistsDefinition : ScriptableObject
    {
        public List<AmbientPlaylist> m_AmbientPlaylists;
    }
}