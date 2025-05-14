using Systems;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIController : MonoBehaviour, IGameStartListener, IGameFinishListener
    {
        [SerializeField] private Button startGameButton;

        public void OnStartGame()
        {
            startGameButton.gameObject.SetActive(false);
        }

        public void OnFinishGame()
        {
            startGameButton.gameObject.SetActive(true);
        }
    }
}