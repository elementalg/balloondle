using Balloondle.Gameplay.World;
using Balloondle.Gameplay.World.Effects;
using UnityEngine;

namespace Balloondle.Script.Handlers.FirstRun.EntriesEnds
{
    [CreateAssetMenu(fileName = "FirstRunEndCongratulations",
        menuName = "Script/Entries Ends Handler/First Run End Congratulations", order = 1)]
    public class FirstRunEndCongratulations : EntriesEndsHandler
    {
        public override void OnEntryStart(int entryId)
        {
            if (entryId == 1)
            {
                EffectPlayer effectPlayer = FindObjectOfType<EffectPlayer>();
                effectPlayer.Play("Success0", FindObjectOfType<Player>().gameObject,
                    Vector3.zero, Quaternion.identity, 0.8f);
            }
        }

        public override void OnEntryEnd(int entryId)
        {
            
        }
    }
}