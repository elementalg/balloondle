﻿using System;
using Balloondle.Ambient;
using Balloondle.Gameplay;
using Balloondle.Gameplay.World;
using Balloondle.Gameplay.World.Effects;
using UnityEngine;

namespace Balloondle.Script.Handlers.FirstRun.EntriesEnds
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
                
                AmbientPlayer player = FindObjectOfType<AmbientPlayer>();
                player.Volume = 0.05f;
                player.Play("Relaxing");
            }
        }

        public override void OnEntryEnd(int entryId)
        {
            if (entryId == 0)
            {
                EffectPlayer effectPlayer = FindObjectOfType<EffectPlayer>();
                effectPlayer.Play("Success1", FindObjectOfType<Player>().gameObject,
                    Vector3.zero, Quaternion.identity, 0.4f);
            }
        }
    }
}