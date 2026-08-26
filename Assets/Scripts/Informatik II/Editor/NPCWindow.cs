using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class NPCWindow : EditorWindow

{
    #region variables
    public TypeofNPC typeofNPC;
    public TalkType talkType;
    public TypeofEntity entityType;
    public TypeofOni oniType;
    public TypeofShrine typeofShrine;
    public TypeofActivation activationStateType;

    int npcNummber;
    bool npcType = false;
    bool npcName;
    bool npcMove = false;
    bool npcTalk = false;
    string NPCname;
    #endregion

    [MenuItem("Window/NPC")]
    public static void ShowWindow() { GetWindow<NPCWindow>("NPC"); }

    void OnGUI()
    {
        GUILayout.Label("Select to get Npc vriables and change all of the selected once");
        GUILayout.Space(10);

        npcNummber = EditorGUILayout.IntField(" nummber of NPCs", npcNummber);
        NumberOfNPCs();

        GUILayout.Space(10);
        GUILayout.Label("Search NPC by/If:");
        GUILayout.Space(10);

        npcName = EditorGUILayout.Toggle("Name", npcName);
        if (npcName == true)
        {
            NPCname = GUILayout.TextField(NPCname, GUILayout.Width(100));
        }
        else
        {
            GUILayout.Space(10);
            npcMove = EditorGUILayout.Toggle("can Move", npcMove);

            GUILayout.Space(10);
            npcTalk = EditorGUILayout.Toggle("can Talk", npcTalk);

            npcType = EditorGUILayout.Toggle("NpC type", npcType);

            if (npcType)
            {
                typeofNPC = (TypeofNPC)EditorGUILayout.EnumPopup("Type", typeofNPC);
            }
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Search NPCS"))
        {
            SearchNPC();
        }
    }
    void NumberOfNPCs() { npcNummber = Selection.count; }

    void SearchNPC()
    {
        NPC[] npcs = Resources.FindObjectsOfTypeAll<NPC>();
        List<GameObject> npcobject = new List<GameObject>();

        foreach (NPC npc in npcs)
        {
            GameObject obj = npc.gameObject;
            NPC npcscript = obj.GetComponent<NPC>();

            switch (true)
            {
                case true when npcName:
                    obj = GameObject.Find(NPCname);
                    if (obj.GetComponent<NPC>() != null)
                    {
                        npcobject.Add(obj);
                    }
                    else { }
                    break;
                
                case true when npcMove:
                case true when npcTalk:
                case true when npcType:
                    #region NPC move,talk& type
                    if (npcMove && npcTalk && npcType || npcType && npcTalk)
                    {
                        if (npcscript != null && npc.canTalk)
                        {
                            switch (typeofNPC)
                            {
                                case TypeofNPC.friendly:
                                    if (npcscript.typeofNPC == TypeofNPC.friendly)
                                    {
                                        if (npcMove)
                                        {
                                            if (!EditorUtility.IsPersistent(obj) && obj.GetComponent<NpcMovement>() != null)
                                            {
                                                npcobject.Add(obj);
                                            }
                                        }

                                        else if (!npcMove)
                                        {
                                            if (!EditorUtility.IsPersistent(obj) && obj.GetComponent<NpcMovement>() == null)
                                            {
                                                npcobject.Add(obj);
                                            }
                                        }
                                    }
                                    break;

                                case TypeofNPC.neutral:
                                    if (npcscript.typeofNPC == TypeofNPC.neutral)
                                    {
                                        if (npcMove)
                                        {
                                            if (!EditorUtility.IsPersistent(obj) && obj.GetComponent<NpcMovement>() != null)
                                            {
                                                npcobject.Add(obj);
                                            }
                                        }
                                        else if (!npcMove)
                                        {
                                            if (!EditorUtility.IsPersistent(obj) && obj.GetComponent<NpcMovement>() == null)
                                            {
                                                npcobject.Add(obj);
                                            }
                                        }
                                    }
                                    break;

                                case TypeofNPC.agressive:
                                    if (npcscript.typeofNPC == TypeofNPC.agressive)
                                    {
                                        if (npcMove)
                                        {
                                            if (!EditorUtility.IsPersistent(obj) && obj.GetComponent<NpcMovement>() != null)
                                            {
                                                npcobject.Add(obj);
                                            }
                                        }
                                        else if (!npcMove)
                                        {
                                            if (!EditorUtility.IsPersistent(obj) && obj.GetComponent<NpcMovement>() == null)
                                            {
                                                npcobject.Add(obj);
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    else if (npcTalk && !npcType)
                    {
                        if (npc.canTalk)
                        {
                            if (npcMove)
                            {
                                if (!EditorUtility.IsPersistent(obj) && obj.GetComponent<NpcMovement>() != null)
                                {
                                    npcobject.Add(obj);
                                }
                            }
                            else if (!npcMove)
                            {
                                if (!EditorUtility.IsPersistent(obj) && obj.GetComponent<NpcMovement>() == null)
                                {
                                    npcobject.Add(obj);
                                }
                            }
                        }
                    }
                    else if (npcType && !npc.canTalk)
                    {
                        switch (typeofNPC)
                        {
                            case TypeofNPC.friendly:
                                if (npcscript.typeofNPC == TypeofNPC.friendly)
                                {
                                    if (npcMove)
                                    {
                                        if (!EditorUtility.IsPersistent(obj) && obj.GetComponent<NpcMovement>() != null)
                                        {
                                            npcobject.Add(obj);
                                        }
                                    }
                                    else if (!npcMove)
                                    {
                                        if (!EditorUtility.IsPersistent(obj) && obj.GetComponent<NpcMovement>() == null)
                                        {
                                            npcobject.Add(obj);
                                        }
                                    }
                                }
                                break;

                            case TypeofNPC.neutral:
                                if (npcscript.typeofNPC == TypeofNPC.neutral)
                                {
                                    if (npcMove)
                                    {
                                        if (!EditorUtility.IsPersistent(obj) && obj.GetComponent<NpcMovement>() != null)
                                        {
                                            npcobject.Add(obj);
                                        }
                                    }

                                    else if (!npcMove) { if (!EditorUtility.IsPersistent(obj) && obj.GetComponent<NpcMovement>() == null) npcobject.Add(obj); }
                                }
                                break;
                            case TypeofNPC.agressive:
                                if (npcscript.typeofNPC == TypeofNPC.agressive)
                                {
                                    if (npcMove)
                                    {
                                        if (!EditorUtility.IsPersistent(obj) && obj.GetComponent<NpcMovement>() != null)
                                        { npcobject.Add(obj); }
                                    }

                                    else if (!npcMove) { if (!EditorUtility.IsPersistent(obj) && obj.GetComponent<NpcMovement>() == null) npcobject.Add(obj); }
                                }
                                break;
                        }
                    }
                    break;
                    #endregion

                default:
                    if (!EditorUtility.IsPersistent(obj) && obj.GetComponent<NPC>() != null)
                    {
                        npcobject.Add(obj);
                    }
                    break;
            }
        }

        Selection.objects = npcobject.ToArray();
    }

}