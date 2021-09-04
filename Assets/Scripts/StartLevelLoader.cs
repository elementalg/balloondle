using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Balloondle
{
    /// <summary>
    /// Proceeds to load the next first run scene, or the scene containing the next level, or the last one if all levels
    /// have been completed.
    /// </summary>
    public class StartLevelLoader : MonoBehaviour
    {
        private const string IsFirstRunKey = "first-run";

        [SerializeField, Tooltip("Circle mask used for creating a transition to the loaded scene.")] 
        private RectTransform m_CircleMask;

        private void Start()
        {
            // 0 -> not the first run, 1 -> first run
            int isFirstRun = PlayerPrefs.GetInt(IsFirstRunKey, 1);

            if (isFirstRun == 1)
            {
                LoadFirstRun();
            }
            else
            {
                LoadNextLevel();
            }
        }

        private void LoadFirstRun()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(SceneNames.FIRST_RUN, LoadSceneMode.Additive);
            operation.completed += ApplyTransition;
        }

        private void LoadNextLevel()
        {
            throw new NotImplementedException();
        }

        private void ApplyTransition(AsyncOperation operation)
        {
            if (!operation.isDone)
            {
                return;
            }

            GameObject[] loadingUI = GameObject.FindGameObjectsWithTag("LoadingUI");

            foreach (GameObject loadingUIObject in loadingUI)
            {
                Destroy(loadingUIObject);
            }
            
            Animator animator = m_CircleMask.GetComponent<Animator>();

            if (animator == null)
            {
                throw new InvalidOperationException("Missing animator from CircleMask.");
            }
            
            animator.enabled = true;
        }
    }
}