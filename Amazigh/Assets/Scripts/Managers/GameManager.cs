using UnityEngine;
using UnityEngine.SceneManagement;

namespace AmazighGame.Core
{
    /// <summary>
    /// États possibles du jeu
    /// </summary>
    public enum GameState
    {
        MainMenu,
        ExploringHub,
        PlayingLevel,
        Paused
    }

    /// <summary>
    /// GameManager principal - Singleton persistant qui gère l'état global du jeu
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [Header("État du Jeu")]
        [SerializeField] private GameState currentState = GameState.MainMenu;
        
        [Header("Configuration des Scènes")]
        [Tooltip("Nom de la scène Hub")]
        [SerializeField] private string hubSceneName = "Hub";
        
        [Tooltip("Nom de la scène du menu principal")]
        [SerializeField] private string mainMenuSceneName = "MainMenu";
        
        [Header("Héros Actuel")]
        [Tooltip("Données du héros actuellement sélectionné")]
        public HeroDataSO currentHero;
        
        public GameState CurrentState => currentState;
        
        private void Awake()
        {
            // Singleton persistant
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeGame();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// Initialise le jeu au démarrage
        /// </summary>
        private void InitializeGame()
        {
            Debug.Log("[GameManager] Initialisation du jeu - Légendes Amazighes");
            ChangeState(GameState.MainMenu);
        }
        
        /// <summary>
        /// Change l'état du jeu
        /// </summary>
        public void ChangeState(GameState newState)
        {
            if (currentState == newState) return;
            
            Debug.Log($"[GameManager] Changement d'état: {currentState} → {newState}");
            currentState = newState;
            
            // Logique selon l'état
            switch (newState)
            {
                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;
                    
                case GameState.ExploringHub:
                case GameState.PlayingLevel:
                    Time.timeScale = 1f;
                    break;
            }
        }
        
        /// <summary>
        /// Charge la scène Hub de manière propre
        /// </summary>
        public void LoadHub()
        {
            Debug.Log($"[GameManager] Chargement du Hub: {hubSceneName}");
            ChangeState(GameState.ExploringHub);
            SceneManager.LoadScene(hubSceneName);
        }
        
        /// <summary>
        /// Charge une scène de niveau spécifique
        /// </summary>
        public void LoadLevel(string levelSceneName)
        {
            if (string.IsNullOrEmpty(levelSceneName))
            {
                Debug.LogError("[GameManager] Nom de scène invalide!");
                return;
            }
            
            Debug.Log($"[GameManager] Chargement du niveau: {levelSceneName}");
            ChangeState(GameState.PlayingLevel);
            SceneManager.LoadScene(levelSceneName);
        }
        
        /// <summary>
        /// Charge le menu principal
        /// </summary>
        public void LoadMainMenu()
        {
            Debug.Log($"[GameManager] Retour au menu principal: {mainMenuSceneName}");
            ChangeState(GameState.MainMenu);
            SceneManager.LoadScene(mainMenuSceneName);
        }
        
        /// <summary>
        /// Met le jeu en pause
        /// </summary>
        public void PauseGame()
        {
            if (currentState == GameState.PlayingLevel || currentState == GameState.ExploringHub)
            {
                ChangeState(GameState.Paused);
            }
        }
        
        /// <summary>
        /// Reprend le jeu depuis la pause
        /// </summary>
        public void ResumeGame()
        {
            if (currentState == GameState.Paused)
            {
                // Retourner à l'état précédent (à améliorer si nécessaire)
                ChangeState(GameState.PlayingLevel);
            }
        }
        
        /// <summary>
        /// Définit le héros actuel
        /// </summary>
        public void SetCurrentHero(HeroDataSO hero)
        {
            currentHero = hero;
            Debug.Log($"[GameManager] Héros sélectionné: {hero.heroName}");
        }
        
        /// <summary>
        /// Quitte le jeu
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("[GameManager] Fermeture du jeu");
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
