using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class ButtonView : MonoBehaviour
	{
		private Button _button;

		private void Awake()
		{
			_button = GetComponent<Button>();
			_button.onClick.AddListener(Click);
		}

		private void OnDestroy()
		{
			_button.onClick.RemoveListener(Click);
		}

		public event Action OnClick;

		private void Click()
		{
			OnClick?.Invoke();
		}
	}
}