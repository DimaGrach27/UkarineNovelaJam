using TMPro;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ChapterNotes
{
    public class ChapterNotesPartView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI dialogText;

        public string Name
        {
            set
            {
                bool isEmptyName = string.IsNullOrEmpty(value);

                nameText.enabled = !isEmptyName;
                nameText.text = value;
            }
        }

        public string Dialog
        {
            set => dialogText.text = value;
        }
    }
}