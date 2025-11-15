using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class MenuController : MonoBehaviour
    {
        public event Action OnSettingsClick;
        public event Action OnPlayClick;
        public event Action OnExitClick;

        [SerializeField] Button playButton;
        [SerializeField] Button settingsButton;
        [SerializeField] Button exitButton;

        private Button[] _buttons;
        private int _selectedIndex = 0;

        private void Awake()
        {
            playButton.onClick.AddListener(HandleOnPlayClick);
            settingsButton.onClick.AddListener(HandleOnSettingsClick);
            exitButton.onClick.AddListener(HandleOnExitClick);

            _buttons = new Button[] { playButton, settingsButton, exitButton };
        }

        private void Start()
        {
            EventSystem.current.SetSelectedGameObject(playButton.gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _selectedIndex = (_selectedIndex - 1 + _buttons.Length) % _buttons.Length;
                SelectButton(_selectedIndex);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _selectedIndex = (_selectedIndex + 1) % _buttons.Length;
                SelectButton(_selectedIndex);
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                _buttons[_selectedIndex].onClick.Invoke();
            }
        }

        private void SelectButton(int index)
        {
            EventSystem.current.SetSelectedGameObject(_buttons[index].gameObject);
        }

        private void OnDestroy()
        {
            playButton.onClick.RemoveListener(HandleOnPlayClick);
            settingsButton.onClick.RemoveListener(HandleOnPlayClick);
            exitButton.onClick.RemoveListener(HandleOnPlayClick);
        }

        private void HandleOnPlayClick()
        {
            OnPlayClick?.Invoke();
        }

        private void HandleOnSettingsClick()
        {
            OnSettingsClick?.Invoke();
        }

        private void HandleOnExitClick()
        {
            OnExitClick?.Invoke();
        }
    }
}
