using UnityEngine;
using LegendesAmazighes.Core;
using LegendesAmazighes.Managers;

namespace LegendesAmazighes.Interactions
{
    /// <summary>
    /// Objet interactif pour sélectionner une histoire dans le Hub
    /// </summary>
    public class StoryInteractable : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("Données de l'histoire associée à cet objet")]
        public StoryData storyData;
        
        [Header("Feedback Visuel")]
        [Tooltip("Couleur de surbrillance quand le joueur est proche")]
        public Color highlightColor = Color.yellow;
        
        [Tooltip("Matériau original de l'objet")]
        private Material originalMaterial;
        
        [Tooltip("Renderer de l'objet")]
        private Renderer objectRenderer;
        
        private bool isPlayerNearby = false;
        
        private void Start()
        {
            // Récupérer le renderer pour le feedback visuel
            objectRenderer = GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                originalMaterial = objectRenderer.material;
            }
            
            // Vérifier que les données sont assignées
            if (storyData == null)
            {
                Debug.LogWarning($"StoryData manquant sur {gameObject.name}");
            }
        }
        
        private void Update()
        {
            // Si le joueur est proche et appuie sur E ou Espace
            if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }
        
        /// <summary>
        /// Appelé quand le joueur interagit avec l'objet
        /// </summary>
        public void Interact()
        {
            if (storyData == null)
            {
                Debug.LogWarning("Pas de StoryData assigné!");
                return;
            }
            
            if (!storyData.isUnlocked)
            {
                Debug.Log($"Histoire '{storyData.storyName}' encore verrouillée.");
                // Ici vous pouvez afficher un message UI
                return;
            }
            
            Debug.Log($"Lancement de l'histoire: {storyData.storyName}");
            
            // Utiliser le GameManager pour sélectionner et charger l'histoire
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SelectStory(storyData);
            }
        }
        
        /// <summary>
        /// Détecte quand le joueur entre dans la zone
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerNearby = true;
                Highlight(true);
                // Ici vous pouvez afficher un prompt UI "Appuyez sur E pour interagir"
            }
        }
        
        /// <summary>
        /// Détecte quand le joueur sort de la zone
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerNearby = false;
                Highlight(false);
                // Cacher le prompt UI
            }
        }
        
        /// <summary>
        /// Active/Désactive la surbrillance de l'objet
        /// </summary>
        private void Highlight(bool highlight)
        {
            if (objectRenderer == null)
                return;
            
            if (highlight)
            {
                objectRenderer.material.color = highlightColor;
            }
            else
            {
                objectRenderer.material = originalMaterial;
            }
        }
    }
}
