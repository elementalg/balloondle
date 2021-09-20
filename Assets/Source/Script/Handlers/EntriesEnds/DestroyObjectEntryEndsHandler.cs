using Balloondle.Gameplay.World;
using UnityEngine;

namespace Balloondle.Script.Handlers.EntriesEnds
{
    [CreateAssetMenu(fileName = "DestroyObjectEntryEndsHandler",
        menuName = "Script/Entries Ends Handler/Destroy Object", order = 1)]
    public class DestroyObjectEntryEndsHandler : EntriesEndsHandler
    {
        public override void OnEntryStart(int entryId)
        {
            if (entryId == 3)
            {
                WorldEntitySpawner worldEntitySpawner = FindObjectOfType<WorldEntitySpawner>();
                WorldEntity barrel = worldEntitySpawner.Spawn("Barrel", new Vector3(44.529f, -18.249f), 
                    Quaternion.Euler(0f, 0f, 280.952f));

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