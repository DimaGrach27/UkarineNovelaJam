using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Debug.Log($"OnClick {gameObject.name}");
        EventSystem.current.SetSelectedGameObject(null);
    }
}
