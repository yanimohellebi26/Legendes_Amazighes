using UnityEngine;
using AmazighGame.Core;

namespace AmazighGame.Gameplay
{
    /// <summary>
    /// Contrôleur générique du joueur avec mouvement isométrique
    /// Les stats sont injectées via HeroDataSO - pas de valeurs en dur!
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Données du Héros")]
        [Tooltip("Données du héros - à assigner via InitializeHero() ou l'Inspector")]
        public HeroDataSO heroData;
        
        [Header("Configuration")]
        [Tooltip("Vitesse de rotation du personnage")]
        [SerializeField] private float rotationSpeed = 10f;
        
        [Tooltip("Force de gravité appliquée")]
        [SerializeField] private float gravity = 9.81f;
        
        [Header("Références")]
        [Tooltip("Transform où le modèle 3D sera instancié")]
        [SerializeField] private Transform modelParent;
        
        // Composants
        private CharacterController characterController;
        
        // État
        private float currentMoveSpeed;
        private GameObject currentModel;
        private Vector3 moveDirection;
        private Vector3 velocity;
        
        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            
            // Si modelParent n'est pas assigné, utiliser this.transform
            if (modelParent == null)
            {
                modelParent = transform;
            }
            
            // Vérifier le tag
            if (!gameObject.CompareTag("Player"))
            {
                Debug.LogWarning("[PlayerController] Tag 'Player' manquant!");
            }
        }
        
        private void Start()
        {
            // Si heroData est déjà assigné dans l'Inspector, initialiser
            if (heroData != null)
            {
                InitializeHero(heroData);
            }
        }
        
        private void Update()
        {
            if (heroData == null)
            {
                Debug.LogWarning("[PlayerController] Aucun HeroData assigné!");
                return;
            }
            
            HandleMovement();
        }
        
        /// <summary>
        /// Initialise le héros avec les données du ScriptableObject
        /// </summary>
        public void InitializeHero(HeroDataSO data)
        {
            if (data == null)
            {
                Debug.LogError("[PlayerController] HeroDataSO null passé à InitializeHero!");
                return;
            }
            
            heroData = data;
            currentMoveSpeed = data.baseMoveSpeed;
            
            Debug.Log($"[PlayerController] Héros initialisé: {data.heroName} (Vitesse: {currentMoveSpeed})");
            
            // Changer le modèle 3D si nécessaire
            if (data.modelPrefab != null)
            {
                ChangeModel(data.modelPrefab);
            }
        }
        
        /// <summary>
        /// Change le modèle 3D du héros
        /// </summary>
        private void ChangeModel(GameObject newModelPrefab)
        {
            // Détruire l'ancien modèle
            if (currentModel != null)
            {
                Destroy(currentModel);
            }
            
            // Instancier le nouveau modèle
            currentModel = Instantiate(newModelPrefab, modelParent);
            currentModel.transform.localPosition = Vector3.zero;
            currentModel.transform.localRotation = Quaternion.identity;
            
            Debug.Log($"[PlayerController] Modèle changé: {newModelPrefab.name}");
        }
        
        /// <summary>
        /// Gère le déplacement du joueur avec mouvement isométrique
        /// </summary>
        private void HandleMovement()
        {
            // Récupérer les inputs (WASD ou flèches)
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            // Calculer la direction de mouvement alignée avec la caméra
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;
            
            // Projeter sur le plan horizontal (Y = 0)
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();
            
            // Direction de mouvement
            moveDirection = (forward * vertical + right * horizontal).normalized;
            
            // Appliquer le mouvement
            if (moveDirection.magnitude > 0.1f)
            {
                // Déplacer le personnage
                characterController.Move(moveDirection * currentMoveSpeed * Time.deltaTime);
                
                // Faire tourner le personnage dans la direction du mouvement
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            
            // Appliquer la gravité
            if (characterController.isGrounded)
            {
                velocity.y = -2f; // Petite force pour rester au sol
            }
            else
            {
                velocity.y -= gravity * Time.deltaTime;
            }
            
            characterController.Move(velocity * Time.deltaTime);
        }
        
        /// <summary>
        /// Obtient la vitesse actuelle de déplacement
        /// </summary>
        public float GetCurrentSpeed()
        {
            return currentMoveSpeed;
        }
        
        /// <summary>
        /// Modifie temporairement la vitesse (ex: pour des power-ups)
        /// </summary>
        public void SetSpeedMultiplier(float multiplier)
        {
            if (heroData != null)
            {
                currentMoveSpeed = heroData.baseMoveSpeed * multiplier;
            }
        }
        
        /// <summary>
        /// Réinitialise la vitesse à la valeur de base du héros
        /// </summary>
        public void ResetSpeed()
        {
            if (heroData != null)
            {
                currentMoveSpeed = heroData.baseMoveSpeed;
            }
        }
    }
}
