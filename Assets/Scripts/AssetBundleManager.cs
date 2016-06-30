using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace MT {
	public class AssetBundleInfo {
		public string localUrl;
		public string remoteUrl;

		public AssetBundleInfo (string localUrl_, string remoteUrl_) {
			localUrl = localUrl_;
			remoteUrl = remoteUrl_;
		}
	}

	public class AssetBundleManager {
		private Dictionary<string, AssetBundleInfo> _assetBundleInfoDic = new Dictionary<string, AssetBundleInfo>();
		private Dictionary<string, AssetBundle> _assetBundleDic = new Dictionary<string, AssetBundle>();

		private static AssetBundleManager _curr;
		public static AssetBundleManager Curr {
			get { 
				if (_curr == null) {
					_curr = new AssetBundleManager ();
				}
				return _curr;
			}
		}

		public bool UpdateAssetBundleInfo(string key, AssetBundleInfo info, bool replace) {
			if (!_assetBundleInfoDic.ContainsKey (key)) {
				_assetBundleInfoDic.Add (key, info);
				return true;
			} else if (replace) {
				_assetBundleInfoDic [key] = info;
				return true;
			}
			return false;
		}

		public void RemoveAssetBundle(string key) {
			if (_assetBundleDic.ContainsKey(key)) {
				if (_assetBundleDic[key] != null) {
					_assetBundleDic[key].Unload(false);
				}
				_assetBundleDic.Remove (key);
			}
		}

		public void RemoveAllAssetBundle() {
			while(_assetBundleDic.Count > 0) {
				var dicEnum = _assetBundleDic.GetEnumerator ();
				dicEnum.MoveNext ();
				RemoveAssetBundle (dicEnum.Current.Key);
			}
		}

		public AssetBundle GetAssetBundle(string key) {
			AssetBundle res = null;
			_assetBundleDic.TryGetValue (key, out res);
			return res;
		}

		public IEnumerator LoadAssetBundle(string key) {
			if (!_assetBundleDic.ContainsKey (key) && _assetBundleInfoDic.ContainsKey(key)) {
				_assetBundleDic.Add (key, null);
				string url = _assetBundleInfoDic [key].localUrl;
				#if UNITY_EDITOR
				url = Application.dataPath + url;
				#else
				url = Application.persistentDataPath + url;
				#endif

				if (!File.Exists(url)) {
					yield return DownloadAssetBundle (key);
				}

				AssetBundleCreateRequest abcRequest = AssetBundle.LoadFromFileAsync(url);
				yield return abcRequest;
				_assetBundleDic[key] = abcRequest.assetBundle;
			} 
		}

		IEnumerator DownloadAssetBundle(string key) {
			if (_assetBundleInfoDic.ContainsKey(key)) {
				WWW www = new WWW(_assetBundleInfoDic[key].remoteUrl);
				yield return www;
				if (www.error == null || www.error.Length == 0) {
					File.WriteAllText (_assetBundleInfoDic[key].localUrl, www.data);
				} 
			}
		}
	}
}
