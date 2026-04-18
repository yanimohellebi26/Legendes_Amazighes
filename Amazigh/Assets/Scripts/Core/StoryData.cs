using UnityEngine;

namespace LegendesAmazighes.Core
{
    /// <summary>
    /// ScriptableObject pour définir les données d'une histoire/mythe
    /// </summary>
    [CreateAssetMenu(fileName = "NewStory", menuName = "Légendes Amazighes/Story Data")]
    public class StoryData : ScriptableObject
    {
        [Header("Informations de l'histoire")]
        [Tooltip("Nom du mythe")]
        public string storyName;
        
        [Tooltip("Description courte pour le menu")]
        [TextArea(3, 5)]
        public string description;
        
        [Header("Scène")]
        [Tooltip("Nom de la scène Unity à charger")]
        public string sceneName;
        
        [Header("Visuel")]
        [Tooltip("Icône/Image pour représenter l'histoire dans le Hub")]
        public Sprite storyIcon;
        
        [Tooltip("Objet 3D représentant l'histoire dans le Hub (ex: une pierre gravée)")]
        public GameObject hubRepresentation;
        
        [Header("Progression")]
        [Tooltip("L'histoire a-t-elle été complétée?")]
        public bool isCompleted = false;
        
        [Tooltip("L'histoire est-elle déverrouillée?")]
        public bool isUnlocked = true;
    }
}
