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

        
    }
}