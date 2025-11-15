using Enemies;
using UI;
using Unity.VisualScripting;
using UnityEngine;

namespace GameController
{
     public class GameManager : MonoBehaviour
     {
          [SerializeField] private CanvasController canvasController;
          [SerializeField] private PlayManager.PlayManager playManager;

          private Vector2 _screenBounds;

          void Start()
          {
               canvasController.Init(OnGameStart, CloseApp);
          }

          private void OnGameStart()
          {
               canvasController.gameObject.SetActive(false);
               playManager.gameObject.SetActive(true);
               playManager.StartGame(HandleOnExitToMenu);
          }

          private void HandleOnExitToMenu()
          {
               canvasController.gameObject.SetActive(true);
               playManager.gameObject.SetActive(false);
          }

          private void CloseApp()
          {
               Application.Quit();
          }
     }
}
