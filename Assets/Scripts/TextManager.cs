using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MT;

public class TextManager : MonoBehaviour {
	static int _index = 0;
	static List<string> _info = new List<string> () {
		"perfab",
		"sence"
	};

	string CurrKey {
		get { return _info [_index]; }
	}

	IEnumerator Start () {
		AssetBundleManager abManager = AssetBundleManager.Curr;
		// load info
		for(int i = 0; i < _info.Count; i++) {
			string key = _info [i];
			abManager.UpdateAssetBundleInfo(key, new AssetBundleInfo("/AssetBundles/Bundles/"+key, ""), false);
			yield return abManager.LoadAssetBundle (key);
		}


		if (CurrKey == "perfab") {
			AssetBundleRequest abRequst = abManager.GetAssetBundle(CurrKey).LoadAssetAsync<GameObject> (CurrKey);
			yield return abRequst;
			GameObject go = Instantiate(abRequst.asset) as GameObject;
			go.transform.SetParent (GameObject.FindObjectOfType<Canvas>().transform);
			go.transform.localPosition = Vector3.zero;
		}
	}

	void OnGUI() {
		for(int i = 0; i < _info.Count; i++) {
			int index = i;
			if (GUI.Button (new Rect (10, 10 + 100*i, 150, 100), _info [index])) {
				SwitchText (index);
			}
		}
	}

	void SwitchText(int index) {
		_index = index;

		UnityEngine.SceneManagement.SceneManager.LoadScene (CurrKey);
	}
}
