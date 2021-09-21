using System.Collections.Generic;
using UnityEngine;

namespace Balloondle
{
    [CreateAssetMenu(fileName = "EffectsDefinition", menuName = "Effects/Effects Definition", order = 0)]
    public class EffectsDefinition : ScriptableObject
    {
        public List<Effect> m_Effects;
    }
}