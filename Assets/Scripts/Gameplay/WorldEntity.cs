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
        public float health { get; private set; } = 0f;
        public float armor { get; private set; } = 0f;

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
            if (healAmount + health >= float.MaxValue)
            {
                health = float.MaxValue;
                return;
            }

            health += healAmount;
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
            if (armorAmount + armor > float.MaxValue)
            {
                armor = float.MaxValue;
                return;
            }

            armor += armorAmount;
        }
    }
}
