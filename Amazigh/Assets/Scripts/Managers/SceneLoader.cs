using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace LegendesAmazighes.Managers
{
    /// <summary>
    /// Gère le chargement et la transition entre les scènes
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }
        
        [Header("Configuration")]
        [Tooltip("Nom de la scène Hub")]
        public string hubSceneName = "Hub";
        
        [Tooltip("Durée du fade (optionnel)")]
        public float fadeDuration = 0.5f;
        
        private bool isLoading = false;
        
        private void Awake()
        {
            // Singleton simple
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// Charge une scène par son nom
        /// </summary>
        public void LoadScene(string sceneName)
        {
            if (!isLoading)
            {
                StartCoroutine(LoadSceneAsync(sceneName));
            }
        }
        
        /// <summary>
        /// Retourne au Hub
        /// </summary>
        public void ReturnToHub()
        {
            LoadScene(hubSceneName);
        }
        
        /// <summary>
        /// Charge une scène de manière asynchrone
        /// </summary>
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            isLoading = true;
            
            // Optionnel: Ajouter un effet de fade ici
            yield return new WaitForSeconds(fadeDuration);
            
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            
            // Attendre que la scène soit chargée
            while (!asyncLoad.isDone)
            {
                // Vous pouvez afficher une barre de progression ici
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                yield return null;
            }
            
            isLoading = false;
        }
        
        /// <summary>
        /// Recharge la scène actuelle
        /// </summary>
        public void ReloadCurrentScene()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            LoadScene(currentScene);
        }
    }
}
