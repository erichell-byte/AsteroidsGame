using System;
using Purchasing;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _buyAdsButton;
        [SerializeField] private Button _exitGameButton;
    
        public event Action StartClicked;
        public event Action BuyAdsClicked;
        public event Action ExitGameClicked;

        private void Awake()
        {
            _startButton.onClick.AddListener(OnStartClicked);
            _buyAdsButton.onClick.AddListener(OnBuyAdsClicked);
            _exitGameButton.onClick.AddListener(OnExitGameClicked);
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(OnStartClicked);
            _buyAdsButton.onClick.RemoveListener(OnBuyAdsClicked);
            _exitGameButton.onClick.RemoveListener(OnExitGameClicked);
        }

        private void OnStartClicked() => StartClicked?.Invoke();
        private void OnBuyAdsClicked() => BuyAdsClicked?.Invoke();
        private void OnExitGameClicked() => ExitGameClicked?.Invoke();

        public void DisableBuyButton(TypeOfPurchase typeOfPurchase)
        {
            if (typeOfPurchase == TypeOfPurchase.NoAds)
            {
                _buyAdsButton.gameObject.SetActive(false);
            }
        }
    }
}