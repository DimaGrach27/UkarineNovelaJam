using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.UI
{
    public class FocusUIManager : MonoBehaviour
    {
        private EventSystem EventSystem => EventSystem.current;
        private InputSystemUIInputModule m_InputSystemUIInputModule;

        public static FocusUIManager Instance { get; private set; }
        private FocusUIManager m_instance;

        private void Awake()
        {
            Instance = this;
            
            m_InputSystemUIInputModule = EventSystem.currentInputModule as InputSystemUIInputModule;
        }

        public void Initialize(Selectable firstSelectedObject)
        {
            if (EventSystem == null)
            {
                throw new MissingComponentException("Can't find EventSystem on scene!");
            }
            
            EventSystem.firstSelectedGameObject = firstSelectedObject.gameObject;
        }

        public void JumpSelectionToObject(Selectable selectable)
        {
            if (EventSystem == null)
            {
                throw new MissingComponentException("Can't find EventSystem on scene!");
            }
            
            EventSystem.SetSelectedGameObject(selectable.gameObject);
        }

        public void ResetSelectionObject()
        {
            if (EventSystem == null)
            {
                throw new MissingComponentException("Can't find EventSystem on scene!");
            }
            
            EventSystem.SetSelectedGameObject(null);
        }

        private void Update()
        {
            if (EventSystem == null)
            {
                throw new MissingComponentException("Can't find EventSystem on scene!");
            }

            // if (Cursor.visible && m_InputSystemUIInputModule != null)
            // {
                // Selectable currentFocusedObject = m_InputSystemUIInputModule.get;
            // }
        }
    }
}