using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonView : MonoBehaviour
{
    public event Action OnClick;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Click);
    }

    private void Click()
    {
        OnClick?.Invoke();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(Click);
    }
}