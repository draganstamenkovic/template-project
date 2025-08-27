using System;
using Cameras;
using Data.Load;
using Gameplay.Level;
using Gameplay.Player;
using Input;
using Message;
using Message.Messages;
using R3;
using UnityEngine;
using VContainer;

namespace Gameplay
{
    public class GameManager : IGameManager
    {
        [Inject] private readonly InputManager _inputManager;
        [Inject] private readonly ICameraManager _cameraManager;
        [Inject] private readonly ILoadManager _loadManager;
        [Inject] private readonly IPlayerController _playerController;
        [Inject] private readonly ILevelManager _levelManager;

        [Inject] private readonly IMessageBroker _messageBroker;
        private IDisposable _disposableMessage;
        
        private Transform _gameplayParent;
        private bool _isPaused;
        
        public void Initialize()
        {
            CreateGameplayParent();
            _cameraManager.Initialize();
            _loadManager.Initialize();
            _playerController.Initialize(_gameplayParent);
            _inputManager.Initialize(_playerController);
            _levelManager.Initialize();
            CreateGameBounds();
            SubscribeToEvents();
        }

        private void OnNextLevelStarted()
        {
            _levelManager.LoadNextLevel();
            Play();
        }

        private void SubscribeToEvents()
        {
            _disposableMessage = _messageBroker.Receive<NextLevelMessage>().Subscribe(message =>
            {
                OnNextLevelStarted();
            });
        }

        public void Play()
        {
            _playerController.SetActive(true);
            _inputManager.SetActive(true);
        }

        public void Pause()
        {
            _isPaused = true;
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            if (!_isPaused) return;
            
            _isPaused = false;
            Time.timeScale = 1f;
        }

        public void Stop()
        {
            if (_isPaused)
            {
                Time.timeScale = 1f;
                _isPaused = false;
            }
            _playerController.SetActive(false);
            _inputManager.SetActive(false);
        }
        
        public void Quit()
        {
            Stop();
            Cleanup();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID || UNITY_IOS
            Application.Quit();
#endif
        }
        private void CreateGameplayParent()
        {
            var gameplayObj = new GameObject("_Gameplay_");
            _gameplayParent = gameplayObj.transform;
        }

        private void CreateGameBounds()
        {
            var bounds = new GameObject("Bounds");
            bounds.transform.SetParent(_gameplayParent);
            
            var camera = _cameraManager.GetMainCamera();
            
            var topBound = new GameObject("Top");
            topBound.transform.SetParent(bounds.transform);
            topBound.transform.localScale = new Vector3(10, 0.1f, 0.1f);
            topBound.transform.position = new Vector3(0, camera.orthographicSize + 1, 0);
            topBound.AddComponent<BoxCollider2D>();
        }

        private void Cleanup()
        {
            _disposableMessage?.Dispose();
        }
    }
}