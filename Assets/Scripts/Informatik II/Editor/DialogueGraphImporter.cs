using UnityEditor.AssetImporters;
using UnityEngine;
using Unity.GraphToolkit.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
[ScriptedImporter(1, DialogueGraph.AssetExtention)]
public class DialogueGraphImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext Importer)
    {
        DialogueGraph editorGraph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(Importer.assetPath);
        NPCDialogue npcDialogue = ScriptableObject.CreateInstance<NPCDialogue>();
        Dictionary<INode, string> nodeIDMap = new Dictionary<INode, string>();

        foreach (INode node in editorGraph.GetNodes())
        {
            nodeIDMap[node] = Guid.NewGuid().ToString();
        }

        StartNode startNode = editorGraph.GetNodes().OfType<StartNode>().FirstOrDefault();

        if (startNode != null)
        {
            IPort entryPort = startNode.GetOutputPorts().FirstOrDefault()?.FirstConnectedPort;

            if (entryPort != null)
            {
                npcDialogue.EntryNodeID = nodeIDMap[entryPort.GetNode()];
            }
        }

        foreach (INode iNode in editorGraph.GetNodes())
        {
            if (iNode is StartNode || iNode is EndNode)
            {
                continue;
            }

            NPCDialogueNode npcNode = new NPCDialogueNode
            {
                NodeID = nodeIDMap[iNode]
            };

            if (iNode is DialogueNode dialogueNode)
            {
                ProcessDialogueNode(dialogueNode, npcNode, nodeIDMap);
            }
            else if (iNode is DecisionNode decisionNode)
            {
                ProcessDecisionNode(decisionNode, npcNode, nodeIDMap);
            }
            npcDialogue.AllNodes.Add(npcNode);
        }
        Importer.AddObjectToAsset("Dialoguedata", npcDialogue);
        Importer.SetMainObject(npcDialogue);
    }
    private void ProcessDialogueNode(DialogueNode node, NPCDialogueNode npcNode, Dictionary<INode, string> nodeIDMap)
    {
        npcNode.SpeakerName = GetPortValue<string>(node.GetInputPortByName("Speaker"));
        npcNode.DialogueText = GetPortValue<string>(node.GetInputPortByName("Text"));
        IPort nextNodePort = node.GetOutputPortByName("out")?.FirstConnectedPort;

        if (nextNodePort != null)
        {
            npcNode.NextNodeID = nodeIDMap[nextNodePort.GetNode()];
        }
    }
    private void ProcessDecisionNode(DecisionNode node,NPCDialogueNode dialogue,Dictionary<INode,string> nodeIDMap)
    {
        dialogue.SpeakerName = GetPortValue<string>(node.GetInputPortByName("Speaker"));
        dialogue.DialogueText = GetPortValue<string>(node.GetInputPortByName("Text"));
        IEnumerable<IPort> decisionOutputPorts = node.GetOutputPorts().Where(p => p.Name.StartsWith("Decision"));

        foreach (IPort outputPort in decisionOutputPorts) 
        {
            string index = outputPort.Name.Substring("Decision".Length);
            IPort textPort = node.GetInputPortByName("Decision Text" + index);
            string value = GetPortValue<string>(textPort);
            DecisionData decisiondata = new DecisionData { DescisionText = GetPortValue<string>(textPort), DestinationNodeID = outputPort.FirstConnectedPort != null ? nodeIDMap[outputPort.FirstConnectedPort.GetNode()]: null };
            dialogue.Decision.Add(decisiondata);
        }
    }

    private T GetPortValue<T>(IPort port)
    {
        if (port == null)
        {
            return default;
        }
        if (port.IsConnected)
        {
            if (port.FirstConnectedPort.GetNode() is IVariableNode variableNode)
            {
                T value;
                variableNode.Variable.TryGetDefaultValue(out value);
                return value;
            }
        }

        T fallbackValue;
        port.TryGetValue(out fallbackValue);
        return fallbackValue;
    }
}