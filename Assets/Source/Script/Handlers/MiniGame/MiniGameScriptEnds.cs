using System;
using Balloondle.MiniGame;
using UnityEngine;

namespace Balloondle.Script.Handlers.MiniGame
{
    [CreateAssetMenu(fileName = "MiniGameScriptEndsHandler", menuName = "Script/Ends Handler/Mini Game", order = 0)]
    public class MiniGameScriptEnds : ScriptEndsHandler
    {
        public override void OnScriptStart()
        {
            if (m_MiniGamePrefab == null)
            {
                throw new InvalidOperationException("Missing mini-game prefab.");
            }
            
            FindObjectOfType<MiniGameController>().StartGame();   
        }

        public override void OnScriptEnd()
        {
            FindObjectOfType<MiniGameController>().EndGame();
        }
    }
}