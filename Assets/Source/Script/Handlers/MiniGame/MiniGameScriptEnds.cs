using Balloondle.MiniGame;
using UnityEngine;

namespace Balloondle.Script.Handlers.MiniGame
{
    [CreateAssetMenu(fileName = "MiniGameScriptEndsHandler", menuName = "Script/Ends Handler/Mini Game", order = 0)]
    public class MiniGameScriptEnds : ScriptEndsHandler
    {
        public override void OnScriptStart()
        {
            FindObjectOfType<MiniGameController>().StartGame();
        }

        public override void OnScriptEnd()
        {
            FindObjectOfType<MiniGameController>().EndGame();
        }
    }
}