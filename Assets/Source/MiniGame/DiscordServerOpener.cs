using UnityEngine;

namespace Balloondle.MiniGame
{
    public class DiscordServerOpener : MonoBehaviour
    {
        public string m_DiscordURL;

        public void OnDiscordClick()
        {
            Application.OpenURL(m_DiscordURL);
        }
    }
}