using Balloondle.Gameplay.World;
using UnityEngine;

namespace Balloondle.Script.Handlers.EntriesEnds
{
    [CreateAssetMenu(fileName = "GrabWeaponEntryEndsHandler",
        menuName = "Script/Entries Ends Handler/Grab Weapon", order = 1)]
    public class GrabWeaponEntryEndsHandler : EntriesEndsHandler
    {
        public override void OnEntryStart(int entryId)
        {
            if (entryId == 3)
            {
                WorldEntitySpawner worldEntitySpawner = FindObjectOfType<WorldEntitySpawner>();
                WorldEntity weapon = worldEntitySpawner.Spawn("GreenVenom", new Vector3(44.529f, -18.249f), 
                    Quaternion.Euler(0f, 0f, 280.952f));

                Player player = FindObjectOfType<Player>();
                player.OnWeaponGiven += () =>
                {
                    FindObjectOfType<ScriptDirector>().TryTriggerExpireEvent("player-weapon-grabbed");
                };
            }
        }

        public override void OnEntryEnd(int entryId)
        {
            
        }
    }
}