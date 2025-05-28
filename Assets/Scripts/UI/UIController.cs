using Character;
using Systems;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIController : MonoBehaviour, IGameStartListener
    {
        [SerializeField] private GameUIView gameUIView;

        private CharacterModel characterModel;
        private GameUIViewModel viewModel;
        
        [Inject]
        private void Construct(SpaceshipController shipController)
        {
            characterModel = shipController.CharacterModel;
        }

        public void OnStartGame()
        {
            gameUIView.Dispose();
            viewModel = new GameUIViewModel(characterModel);
            gameUIView.Initialize(viewModel);
        }
    }
}