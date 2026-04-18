using UnityEngine;

namespace AmazighGame.Core
{
    /// <summary>
    /// Caméra isométrique simple qui suit une cible avec un offset fixe
    /// Pas de rotation - l'angle est maintenu constant pour une vue isométrique
    /// </summary>
    public class IsometricCamera : MonoBehaviour
    {
        [Header("Cible")]
        [Tooltip("La cible à suivre (généralement le joueur)")]
        [SerializeField] private Transform target;
        
        [Header("Configuration")]
        [Tooltip("Décalage de la caméra par rapport à la cible")]
        [SerializeField] private Vector3 offset = new Vector3(0, 10, -10);
        
        [Tooltip("Vitesse de suivi (0 = instantané, 1 = très lent)")]
        [Range(0f, 1f)]
        [SerializeField] private float smoothSpeed = 0.125f;
        
        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }
            
            FollowTarget();
        }
        
        /// <summary>
        /// Suit la cible avec un offset fixe
        /// </summary>
        private void FollowTarget()
        {
            // Position désirée = position de la cible + offset
            Vector3 desiredPosition = target.position + offset;
            
            // Interpolation pour un mouvement fluide
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            
            // La rotation reste fixe (définie dans l'Inspector)
            // Pas de rotation automatique pour maintenir la vue isométrique
        }
        
        /// <summary>
        /// Définit une nouvelle cible à suivre
        /// </summary>
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            Debug.Log($"[IsometricCamera] Nouvelle cible: {newTarget.name}");
        }
        
        /// <summary>
        /// Modifie l'offset de la caméra
        /// </summary>
        public void SetOffset(Vector3 newOffset)
        {
            offset = newOffset;
        }
        
        /// <summary>
        /// Obtient l'offset actuel
        /// </summary>
        public Vector3 GetOffset()
        {
            return offset;
        }
    }
}
