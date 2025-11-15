using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class SettingsController : MonoBehaviour
    {
        public event Action OnBackClick;

        [Header("UI Elements")]
        [SerializeField] private Toggle checkbox1;
        [SerializeField] private Toggle checkbox2;
        [SerializeField] private Button backButton;

        private Selectable[] _uiElements;
        private int _selectedIndex = 0;

        private void Awake()
        {
            backButton.onClick.AddListener(OnBackButtonClick);

            _uiElements = new Selectable[] { checkbox1, checkbox2, backButton };
        }

        private void OnDestroy()
        {
            backButton.onClick.RemoveListener(OnBackButtonClick);
        }

        private void Start()
        {
            SelectElement(_selectedIndex);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _selectedIndex = (_selectedIndex - 1 + _uiElements.Length) % _uiElements.Length;
                SelectElement(_selectedIndex);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _selectedIndex = (_selectedIndex + 1) % _uiElements.Length;
                SelectElement(_selectedIndex);
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                var current = _uiElements[_selectedIndex];

                if (current is Button button)
                {
                    button.onClick.Invoke();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnBackButtonClick();
            }
        }

        private void SelectElement(int index)
        {
            EventSystem.current.SetSelectedGameObject(_uiElements[index].gameObject);
        }

        private void OnBackButtonClick()
        {
            OnBackClick?.Invoke();
        }
    }
}
