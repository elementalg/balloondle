using System.Collections;
using UnityEngine;

namespace Balloondle.Client
{
    public class CharacterAnimator : MonoBehaviour
    {
        /// <summary>
        /// Rigid body of the balloon.
        /// </summary>
        [SerializeField]
        private Rigidbody2D balloon;

        /// <summary>
        /// Rigid body of the weapon.
        /// </summary>
        [SerializeField]
        private Rigidbody2D weapon;

        /// <summary>
        /// Maximum amount of force applied to the balloon.
        /// </summary>
        [SerializeField]
        private float maxBalloonForce = 50f;

        /// <summary>
        /// Maximum amount of negative force applied to the balloon.
        /// </summary>
        [SerializeField]
        private float negativeMaxBalloonForce = -50f;

        /// <summary>
        /// Maximum amount of force applied to the weapon.
        /// </summary>
        [SerializeField]
        private float maxWeaponForce = 50f;

        /// <summary>
        /// Maximum amount of negative force applied to the weapon.
        /// </summary>
        [SerializeField]
        private float negativeMaxWeaponForce = -50f;

        /// <summary>
        /// Start coroutines, in order to avoid freezing the main thread, and only run the apply force methods
        /// after certain periods of time.
        /// </summary>
        void Start()
        {
            StartCoroutine(ApplyForcesToBalloon());
            StartCoroutine(ApplyForcesToWeapon());
        }

        /// <summary>
        /// Applies a random force to a balloon.
        /// </summary>
        /// <returns></returns>
        IEnumerator ApplyForcesToBalloon()
        {
            while (true)
            {
                Vector2 randomForce = new Vector2(Random.Range(negativeMaxBalloonForce, maxBalloonForce),
                                Random.Range(negativeMaxBalloonForce, maxBalloonForce));
                balloon.AddForce(randomForce);

                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }

        /// <summary>
        /// Applies a random force to a weapon.
        /// </summary>
        /// <returns></returns>
        IEnumerator ApplyForcesToWeapon()
        {
            while (true)
            {
                Vector2 randomForce = new Vector2(Random.Range(negativeMaxWeaponForce, maxWeaponForce),
                                Random.Range(negativeMaxWeaponForce, maxWeaponForce));
                weapon.AddForce(randomForce);

                yield return new WaitForSeconds(Random.Range(7f, 12f));
            }
        }
    }
}