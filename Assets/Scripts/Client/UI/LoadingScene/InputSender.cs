using UnityEngine;
using UnityEngine.UI;

namespace Balloondle.Client
{
    public class InputSender : MonoBehaviour
    {
        /// <summary>
        /// Communicator which requires the input from the form.
        /// </summary>
        [SerializeField]
        private GameObject lobbyCommunicator;

        /// <summary>
        /// Name input field from where we must retrieve the name.
        /// </summary>
        [SerializeField]
        private GameObject nameInputField;

        /// <summary>
        /// Retrieves the text inserted into the input field, and sends it
        /// to the lobby communicator.
        /// </summary>
        public void RetrieveInputFromNameInputField()
        {
            if (nameInputField == null)
            {
                return;
            }

            string name = nameInputField.GetComponent<InputField>().text;

            if (name.Length == 0)
            {
                return;
            }

            lobbyCommunicator.GetComponent<LobbyCommunicatorFunctionality>().SignUp(name);
        }
    }
}