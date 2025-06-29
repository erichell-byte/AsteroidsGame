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
        private SaveLoadManager saveLoadManager;
        public GameState State;

        private List<IGameListener> gameListeners = new();

        [Inject]
        private void Construct(SaveLoadManager saveLoadManager)
        {
            this.saveLoadManager = saveLoadManager;
            State = GameState.Off;
        }

        public void Initialize()
        {
            State = GameState.Off;
        }

        public void AddListener(IGameListener listener)
        {
            gameListeners.Add(listener);
        }

        public void StartGame()
        {
            if (State == GameState.Playing || State == GameState.Pause)
            {
                Debug.Log("Game is already started!");
                return;
            }

            State = GameState.Playing;

            for (int i = 0; i < gameListeners.Count; i++)
            {
                if (gameListeners[i] is IGameStartListener gameStartListener)
                {
                    gameStartListener.OnStartGame();
                }
            }
        }

        public void FinishGame()
        {
            saveLoadManager.SaveGame();

            if (State == GameState.Off || State == GameState.Finished)
            {
                Debug.Log("Game is already finished!");
                return;
            }

            State = GameState.Finished;

            for (int i = 0; i < gameListeners.Count; i++)
            {
                if (gameListeners[i] is IGameFinishListener gameFinishListener)
                {
                    gameFinishListener.OnFinishGame();
                }
            }
        }

        public void PauseGame()
        {
            if (State == GameState.Pause)
            {
                Debug.Log("Game is already paused!");
                return;
            }

            if (State == GameState.Off || State == GameState.Finished)
            {
                Debug.Log("Game is not started or already finished!");
                return;
            }

            State = GameState.Pause;

            for (int i = 0; i < gameListeners.Count; i++)
            {
                if (gameListeners[i] is IGamePauseListener gamePauseListener)
                {
                    gamePauseListener.OnPauseGame();
                }
            }
        }

        public void ResumeGame()
        {
            if (State == GameState.Playing)
            {
                Debug.Log("Game is already playing!");
                return;
            }

            if (State == GameState.Off || State == GameState.Finished)
            {
                Debug.Log("Game is not started or already finished!");
                return;
            }

            State = GameState.Playing;

            for (int i = 0; i < gameListeners.Count; i++)
            {
                if (gameListeners[i] is IGameResumeListener gameResumeListener)
                {
                    gameResumeListener.OnResumeGame();
                }
            }
        }
    }
}