using UnityEngine;

namespace Balloondle.Script.Handlers.ScriptEnds
{
    [CreateAssetMenu(fileName = "FirstRunTutorialScriptEndsHandler", 
        menuName = "Script/Ends Handler/First Run Tutorial", order = 1)]
    public class FirstRunTutorialScriptEndsHandler : ScriptEndsHandler
    {
        [SerializeField] 
        private ScriptPreset m_FirstRunEndPreset;

        public override void OnScriptStart()
        {

        }

        public override void OnScriptEnd()
        {
            ScriptDirector scriptDirector = FindObjectOfType<ScriptDirector>();
            scriptDirector.StartScript(m_FirstRunEndPreset);
        }
    }
}