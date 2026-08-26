using System.Collections.Generic;
using System;
using UnityEngine;

public class NPCDialogue : ScriptableObject
{
    public string EntryNodeID;
    public List<NPCDialogueNode> AllNodes=new List<NPCDialogueNode>();

}

[Serializable]
public class NPCDialogueNode 
{
    public string NodeID;
    public string SpeakerName;
    public string DialogueText;
    public List<DecisionData>Decision = new List<DecisionData>();
    public string NextNodeID;
  
}

[Serializable]
public class DecisionData
{
    public string DescisionText;
    public string DestinationNodeID;
}