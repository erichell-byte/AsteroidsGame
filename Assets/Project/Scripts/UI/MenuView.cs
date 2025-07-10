using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button buyAdsButton;
    [SerializeField] private Button exitGameButton;
    
    public event Action StartClicked;
    public event Action BuyAdsClicked;
    public event Action ExitGameClicked;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartClicked);
        buyAdsButton.onClick.AddListener(OnBuyAdsClicked);
        exitGameButton.onClick.AddListener(OnExitGameClicked);
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(OnStartClicked);
        buyAdsButton.onClick.RemoveListener(OnBuyAdsClicked);
        exitGameButton.onClick.RemoveListener(OnExitGameClicked);
    }

    private void OnStartClicked() => StartClicked?.Invoke();
    private void OnBuyAdsClicked() => BuyAdsClicked?.Invoke();
    private void OnExitGameClicked() => ExitGameClicked?.Invoke();

    public void DisableBuyButton()
    {
        buyAdsButton.gameObject.SetActive(false);
    }
}
