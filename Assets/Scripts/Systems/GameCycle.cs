using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Systems
{
    public enum GameState
    {
        Off = 0,
        Playing = 1,
        Pause = 2,
        Finished = 3,
    }

    public class GameCycle : MonoBehaviour
    {
        public GameState State;

        public readonly float Multiplier = 1.5f;

        private List<IGameListener> gameListeners = new();

        private List<IGameUpdateListener> gameUpdateListeners = new();
        private List<IGameFixedUpdateListener> gameFixedUpdateListeners = new();
        private List<IGameLateUpdateListener> gameLateUpdateListeners = new();

        private static GameCycle instance;
        public static GameCycle Instance => instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance == this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            Initialize();
        }

        private void Initialize()
        {
            gameListeners = GetComponentsInChildren<IGameListener>().ToList();

            foreach (var gameListener in gameListeners)
            {
                AddListenerToUpdateEvents(gameListener);
            }
        }

        private void Update()
        {
            if (State == GameState.Playing || State == GameState.Pause)
            {
                UpdateGame();
            }
        }

        private void FixedUpdate()
        {
            FixedUpdateGame();
        }

        private void LateUpdate()
        {
            LateUpdateGame();
        }

        public void AddListener(IGameListener listener)
        {
            gameListeners.Add(listener);
            AddListenerToUpdateEvents(listener);
        }

        private void AddListenerToUpdateEvents(IGameListener gameListener)
        {
            if (gameListener is IGameUpdateListener gameUpdateListener)
            {
                gameUpdateListeners.Add(gameUpdateListener);
            }

            if (gameListener is IGameFixedUpdateListener gameFixedUpdateListener)
            {
                gameFixedUpdateListeners.Add(gameFixedUpdateListener);
            }

            if (gameListener is IGameLateUpdateListener gameLateUpdateListener)
            {
                gameLateUpdateListeners.Add(gameLateUpdateListener);
            }
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


        private void PauseGame()
        {
            if (State == GameState.Off || State == GameState.Finished || State == GameState.Pause)
            {
                Debug.Log("Game is already finished!");
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


        private void ResumeGame()
        {
            if (State == GameState.Playing || State == GameState.Off)
            {
                Debug.Log("Game is not on pause!");
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

        public void UpdateGame()
        {
            float deltaTime = Time.deltaTime * Multiplier;

            for (int i = 0; i < gameUpdateListeners.Count; i++)
            {
                gameUpdateListeners[i].OnUpdate(deltaTime);
            }
        }

        public void FixedUpdateGame()
        {
            float deltaTime = Time.fixedDeltaTime;

            for (int i = 0; i < gameFixedUpdateListeners.Count; i++)
            {
                gameFixedUpdateListeners[i].OnFixedUpdate(deltaTime);
            }
        }

        public void LateUpdateGame()
        {
            float deltaTime = Time.deltaTime;

            for (int i = 0; i < gameLateUpdateListeners.Count; i++)
            {
                gameLateUpdateListeners[i].OnLateUpdate(deltaTime);
            }
        }
    }
}