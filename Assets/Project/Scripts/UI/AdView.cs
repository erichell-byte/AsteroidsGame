using Project.Scripts.UI;
using UnityEngine;

public class AdView : MonoBehaviour
{
    [SerializeField] private ButtonView showRewardedAdButton;
    [SerializeField] private ButtonView skipRewardedAdButton;
 
    private AdViewModel adViewModel;
    public void Initialize(AdViewModel adViewModel)
    {
        this.adViewModel = adViewModel;
        showRewardedAdButton.OnClick += adViewModel.ShowRewardedButtonClicked;
        skipRewardedAdButton.OnClick += adViewModel.SkipRewardedButtonClicked;
    } 
    
    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public void Dispose()
    {
        showRewardedAdButton.OnClick -= adViewModel.ShowRewardedButtonClicked;
        skipRewardedAdButton.OnClick -= adViewModel.SkipRewardedButtonClicked;
    }
}
