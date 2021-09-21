﻿using Balloondle.Gameplay.World;
using UnityEngine;

namespace Balloondle.Script.Handlers.EntriesEnds
{
    [CreateAssetMenu(fileName = "DestroyObjectEntryEndsHandler",
        menuName = "Script/Entries Ends Handler/Destroy Object", order = 1)]
    public class DestroyObjectEntryEndsHandler : EntriesEndsHandler
    {
        public override void OnEntryStart(int entryId)
        {
            if (entryId == 4)
            {
                WorldEntitySpawner worldEntitySpawner = FindObjectOfType<WorldEntitySpawner>();
                WorldEntity barrel = worldEntitySpawner.Spawn("Barrel", new Vector3(52.78f, -16.22f, 0f), 
                    Quaternion.identity);

                barrel.OnPreDestroy += (damage) =>
                {
                    FindObjectOfType<ScriptDirector>().TryTriggerExpireEvent("player-destroy-object");
                };
            }
        }

        public override void OnEntryEnd(int entryId)
        {
            
        }
    }
}