using UnityEngine;

namespace Balloondle.Gameplay
{
    /// <summary>
    /// Basic entity present in the game's world. It defines the basic stats of each destroyable in game entity:
    ///
    /// - Health.
    /// - Armor.
    ///
    /// All the default values are set to 0.
    /// </summary>
    public class WorldEntity : MonoBehaviour
    {
        public float Health { get; private set; } = 0f;
        public float Armor { get; private set; } = 0f;

        /// <summary>
        /// Only increases the entity's health. When positive infinity is reached, health's value is clamped to the
        /// maximum real <see cref="float"/> number.
        /// </summary>
        /// <param name="healAmount">Amount of health which will be added to the current health.</param>
        public void Heal(float healAmount)
        {
            if (healAmount <= 0f)
            {
                return;
            }

            // If the sum goes to infinite, proceed to limit it to the maximum real float number.
            if (healAmount + Health >= float.MaxValue)
            {
                Health = float.MaxValue;
                return;
            }

            Health += healAmount;
        }

        /// <summary>
        /// Only increases the entity's armor. When positive infinity is reached, armor's value is clamped to the
        /// maximum real <see cref="float"/> number.
        /// </summary>
        /// <param name="armorAmount"></param>
        public void RefillArmor(float armorAmount)
        {
            if (armorAmount <= 0f)
            {
                return;
            }

            // If the sum goes to infinite, proceed to limit it to the maximum real float number.
            if (armorAmount + Armor > float.MaxValue)
            {
                Armor = float.MaxValue;
                return;
            }

            Armor += armorAmount;
        }
    }
}
