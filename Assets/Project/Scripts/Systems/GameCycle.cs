using System.Collections.Generic;
using SaveLoad;
using UnityEngine;
using Zenject;

namespace Systems
{
    public enum GameState
    {
        Off = 0,
        Playing = 1,
        Pause = 2,
        Finished = 3,
    }

    public class GameCycle : IInitializable
    {
        private readonly List<IGameListener> _gameListeners = new();
        
        private GameSaveService _gameSaveService;
        private GameState _state;
        
        [Inject]
        private void Construct(GameSaveService gameSaveService)
        {
            _gameSaveService = gameSaveService;
        }

        public void Initialize()
        {
            _state = GameState.Off;
        }

        public void AddListener(IGameListener listener)
        {
            _gameListeners.Add(listener);
        }

        public void StartGame()
        {
            if (_state == GameState.Playing || _state == GameState.Pause)
            {
                Debug.Log("Game is already started!");
                return;
            }

            _state = GameState.Playing;

            for (int i = 0; i < _gameListeners.Count; i++)
            {
                if (_gameListeners[i] is IGameStartListener gameStartListener)
                {
                    gameStartListener.OnStartGame();
                }
            }
        }

        public void FinishGame()
        {
            _gameSaveService.SaveGame();

            if (_state == GameState.Off || _state == GameState.Finished)
            {
                Debug.Log("Game is already finished!");
                return;
            }

            _state = GameState.Finished;

            for (int i = 0; i < _gameListeners.Count; i++)
            {
                if (_gameListeners[i] is IGameFinishListener gameFinishListener)
                {
                    gameFinishListener.OnFinishGame();
                }
            }
        }

        public void PauseGame()
        {
            if (_state == GameState.Pause)
            {
                Debug.Log("Game is already paused!");
                return;
            }

            if (_state == GameState.Off || _state == GameState.Finished)
            {
                Debug.Log("Game is not started or already finished!");
                return;
            }

            _state = GameState.Pause;

            for (int i = 0; i < _gameListeners.Count; i++)
            {
                if (_gameListeners[i] is IGamePauseListener gamePauseListener)
                {
                    gamePauseListener.OnPauseGame();
                }
            }
        }

        public void ResumeGame()
        {
            if (_state == GameState.Playing)
            {
                Debug.Log("Game is already playing!");
                return;
            }

            if (_state == GameState.Off || _state == GameState.Finished)
            {
                Debug.Log("Game is not started or already finished!");
                return;
            }

            _state = GameState.Playing;

            for (int i = 0; i < _gameListeners.Count; i++)
            {
                if (_gameListeners[i] is IGameResumeListener gameResumeListener)
                {
                    gameResumeListener.OnResumeGame();
                }
            }
        }
    }
}