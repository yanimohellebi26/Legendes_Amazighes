using UnityEngine;

namespace AmazighGame.Core
{
    /// <summary>
    /// ScriptableObject définissant les données d'un héros/ancêtre jouable
    /// (Jugurtha, Dihya, etc.)
    /// </summary>
    [CreateAssetMenu(fileName = "NewHero", menuName = "Amazigh Game/Hero Data")]
    public class HeroDataSO : ScriptableObject
    {
        [Header("Identité")]
        [Tooltip("Nom du héros (ex: Jugurtha, Dihya)")]
        public string heroName;
        
        [Tooltip("Description pour le menu de sélection")]
        [TextArea(3, 5)]
        public string description;
        
        [Header("Modèle 3D")]
        [Tooltip("Prefab du modèle 3D du héros")]
        public GameObject modelPrefab;
        
        [Header("Statistiques de Gameplay")]
        [Tooltip("Vitesse de déplacement de base")]
        public float baseMoveSpeed = 5f;
    }
}
