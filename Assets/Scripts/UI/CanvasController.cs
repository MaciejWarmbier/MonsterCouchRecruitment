using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace UI
{
    public class CanvasController : MonoBehaviour
    {

        [SerializeField] MenuController menuController;
        [SerializeField] SettingsController settingsController;

        private Action _onPlayClick;
        private Action _onExitClick;

        private void Awake()
        {
            menuController.OnExitClick += OnExitClicked;
            menuController.OnPlayClick += OnPlayClicked;
            menuController.OnSettingsClick += OnSettingsClicked;
            settingsController.OnBackClick += OnBackClicked;
        }

        public void Init(Action onPlayClick, Action onExitClick)
        {
            _onExitClick = onExitClick;
            _onPlayClick = onPlayClick;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnExitClicked();
            }
        }

        private void OnPlayClicked()
        {
            _onPlayClick?.Invoke();
        }

        private void OnSettingsClicked()
        {
            menuController.gameObject.SetActive(false);
            settingsController.gameObject.SetActive(true);
        }
        
        private void OnBackClicked()
        {
            menuController.gameObject.SetActive(true);
            settingsController.gameObject.SetActive(false);
        }

        private void OnExitClicked()
        {
            _onExitClick?.Invoke();
        }
    }
}
