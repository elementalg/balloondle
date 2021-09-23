using Balloondle.Ambient;
using Balloondle.Gameplay.World;
using Balloondle.UI.Controllers;
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
                
                AmbientPlayer player = FindObjectOfType<AmbientPlayer>();
                player.Play("Aggressive");
            }
        }

        public override void OnEntryEnd(int entryId)
        {
            if (entryId == 5)
            {
                if (FindObjectOfType<JoystickPointerListeningSurface>() != null)
                {
                    FindObjectOfType<JoystickPointerListeningSurface>().DestroyJoystick();
                }

                AmbientPlayer player = FindObjectOfType<AmbientPlayer>();
                player.Play("Relaxing");
            }
        }
    }
}