using Unity.GraphToolkit.Editor;
using UnityEditor;
using System;
[Serializable]
[Graph(AssetExtention)]
public class DialogueGraph : Graph
{
    public const string AssetExtention = "dialoguegraph";
    [MenuItem("Assets/Create/Dialogue Graph",false)]
   private static void CreateAssetFile() 
    {
        GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueGraph>();
    }
}
