using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonView : MonoBehaviour
    {
        private Button _button;
    
        public event Action OnClick;
    
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Click);
        }

        private void Click()
        {
            OnClick?.Invoke();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(Click);
        }
    }
}