using UnityEngine;

namespace Balloondle
{
    public static class Utils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i">Number to be checked.</param>
        /// <returns><b>-1</b> if <i>i</i> is odd, <b>1</b> otherwise.</returns>
        public static int NegativeIfOdd(int i)
        {
            return (1 + 2 * (Mathf.Abs(i) % 2 * -1));
        }
    }
}