using UnityEditor;

public class AssetsBundleManager {
	[MenuItem ("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles ()
	{
		BuildPipeline.BuildAssetBundles ("Assets/AssetBundles/Bundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSXUniversal);
	}
}
