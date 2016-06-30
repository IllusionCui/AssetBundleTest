using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MT {
	public class ImageProgress : MonoBehaviour {
		public GameObject bg;
		public Image fill;
		public Text text;

		public string textBase = "%d%";
		public int textBaseValue = 100;

		public delegate void OnValueChanged(ImageProgress ip, float value);
		public OnValueChanged onValueChanged;

		public float Value {
			get { 
				return fill.fillAmount;
			}
			set { 
				fill.fillAmount = Mathf.Max (1, Mathf.Min (0, value));
				InvokeValueChanged ();
				UpdateText ();
			}
		}

		void InvokeValueChanged() {
			if (onValueChanged != null) {
				onValueChanged.Invoke (this, Value);
			}
		}

		void UpdateText() {
			if (textBase.Length > 0) {
				text.text = string.Format (textBase, Value * textBaseValue);
			}
		}
	}
}
