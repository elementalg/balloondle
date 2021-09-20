using System;
using Balloondle.Gameplay;
using UnityEngine;

namespace Balloondle.Script.Handlers.EntriesEnds
{
    [CreateAssetMenu(fileName = "WaitForMovementEntriesEndsHandler",
        menuName = "Script/Entries Ends Handler/Wait For Movement", order = 1)]
    public class WaitForMovementEntriesEndsHandler : EntriesEndsHandler
    {
        [SerializeField, Tooltip("Prefab containing the joystick container.")]
        private GameObject m_JoystickContainerPrefab;
        
        public override void OnEntryStart(int entryId)
        {
            if (entryId == 0)
            {
                Canvas canvas = FindObjectOfType<Canvas>();

                if (canvas == null)
                {
                    throw new InvalidOperationException("Canvas could not be found.");
                }
                
                Instantiate(m_JoystickContainerPrefab, canvas.transform);
                
                FindObjectOfType<MovementController>().OnCharacterMove += () =>
                {
                    FindObjectOfType<ScriptDirector>().TryTriggerExpireEvent("player-moved");
                };
            }
        }

        public override void OnEntryEnd(int entryId)
        {
            
        }
    }
}