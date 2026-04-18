using UnityEngine;

namespace LegendesAmazighes.Core
{
    /// <summary>
    /// ScriptableObject pour gérer les dialogues
    /// </summary>
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Légendes Amazighes/Dialogue Data")]
    public class DialogueData : ScriptableObject
    {
        [System.Serializable]
        public class DialogueLine
        {
            [Tooltip("Nom du personnage qui parle")]
            public string characterName;
            
            [Tooltip("Texte du dialogue")]
            [TextArea(2, 4)]
            public string text;
            
            [Tooltip("Portrait du personnage (optionnel)")]
            public Sprite characterPortrait;
        }
        
        [Header("Configuration du dialogue")]
        [Tooltip("Lignes de dialogue dans l'ordre")]
        public DialogueLine[] dialogueLines;
    }
}
