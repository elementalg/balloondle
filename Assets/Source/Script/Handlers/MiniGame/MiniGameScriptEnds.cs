using System;
using Balloondle.MiniGame;
using UnityEngine;

namespace Balloondle.Script.Handlers.MiniGame
{
    [CreateAssetMenu(fileName = "MiniGameScriptEndsHandler", menuName = "Script/Ends Handler/Mini Game", order = 0)]
    public class MiniGameScriptEnds : ScriptEndsHandler
    {
        [SerializeField] 
        private GameObject m_BlurPrefab;
        
        public override void OnScriptStart()
        {
            FindObjectOfType<MiniGameController>().StartGame();
        }

        public override void OnScriptEnd()
        {
            GameObject blurGameObject = GameObject.Instantiate(m_BlurPrefab);

            if (blurGameObject.GetComponent<Animator>() == null)
            {
                throw new InvalidOperationException("BlurPrefab is missing an Animator component.");
            }
            
            blurGameObject.GetComponent<Animator>().Play("BlurIn");
            FindObjectOfType<MiniGameController>().EndGame();
        }
    }
}