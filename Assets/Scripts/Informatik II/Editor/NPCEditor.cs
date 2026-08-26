using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;



[CustomEditor(typeof(NPC))]
public class NPCEditor : Editor
{
    public DialogueGraph dialogueGraph;
    public override void OnInspectorGUI()
    {
        NPC npc = (NPC)target;

        #region Npc informations

        GUILayout.BeginHorizontal();
        GUILayout.Label("Name", GUILayout.Width(120));
        npc.name = GUILayout.TextField(npc.name, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Age", GUILayout.Width(120));
        npc.age = GUILayout.TextField(npc.age, GUILayout.Width(30));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Sprite", GUILayout.Width(120));

        npc.npcSprite = (Sprite)EditorGUILayout.ObjectField( npc.npcSprite, typeof(Sprite), false, GUILayout.Width(200), GUILayout.Height(18) );

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        GUILayout.Label("Description", GUILayout.Width(120));
        npc.npcDescription = GUILayout.TextArea(npc.npcDescription, GUILayout.Height(200));

        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        #endregion

        #region Movement
        GUILayout.Label("Movement ", GUILayout.Width(100));
        npc.canMove = EditorGUILayout.Toggle("Can NPC Move", npc.canMove);
        GUILayout.Space(10);
        NpcMovement movement = npc.GetComponent<NpcMovement>();

        if (npc.canMove && movement == null)
        {
            npc.gameObject.AddComponent<NpcMovement>();
        } 
        if (!npc.canMove && movement != null )
        {
            Undo.DestroyObjectImmediate(movement);
        }
        #endregion

        #region Talking to Player

        GUILayout.Label("Talking ", GUILayout.Width(100));
        npc.canTalk = EditorGUILayout.Toggle("Can NPC Talk", npc.canTalk);
        GUILayout.Space(10);

        if (npc.canTalk)
        {
            npc.talkPanel = (GameObject)EditorGUILayout.ObjectField("Talk Panel", npc.talkPanel, typeof(GameObject), true);
            npc.nameText = (TMP_Text)EditorGUILayout.ObjectField("Name Text", npc.nameText, typeof(TMP_Text), true);
            npc.portraitImage = (Image)EditorGUILayout.ObjectField("Portrait Image", npc.portraitImage, typeof(Image), true);
            npc.talkText = (TMP_Text)EditorGUILayout.ObjectField("Talk Text", npc.talkText, typeof(TMP_Text), true);
            npc.typingSpeed = EditorGUILayout.FloatField("Typing Speed", npc.typingSpeed);
            GUILayout.Space(10);
            npc.talkType = (TalkType)EditorGUILayout.EnumPopup("Type of talking", npc.talkType);

            switch (npc.talkType)
            {
                case TalkType.monologe:
                    npc.monologeData = (NPCMonologe)EditorGUILayout.ObjectField("Monologe", npc.monologeData, typeof(NPCMonologe), false);
                    break;

                case TalkType.dialogue:
                case TalkType.decision:

                    npc.dialogue = (NPCDialogue)EditorGUILayout.ObjectField( "Dialogue", npc.dialogue, typeof(NPCDialogue), false);

                    if (npc.talkType == TalkType.decision)
                    {
                        npc.DecisionButtonContainer = (Transform)EditorGUILayout.ObjectField("button container", npc.DecisionButtonContainer, typeof(Transform), true);
                        npc.DecisionButtonPrefab = (Button)EditorGUILayout.ObjectField("button prefab", npc.DecisionButtonPrefab, typeof(Button), true);
                    }
                    break;
            }
        }
        GUILayout.Space(10);
        #endregion

        #region Npc Type

        GUILayout.Label("NPC Type ", GUILayout.Width(100));
        npc.typeofNPC = (TypeofNPC)EditorGUILayout.EnumPopup("Type", npc.typeofNPC);
        GUILayout.Space(10);

        if (npc.typeofNPC != TypeofNPC.friendly)
        {
            NPCStateMachine stateMachine = npc.GetComponent<NPCStateMachine>();
            EnemyHealth enemyHealth = npc.GetComponent<EnemyHealth>();
            ShrineHealth shrine = npc.GetComponent<ShrineHealth>();
            ShrineStateMachine shrineStateMachine = npc.GetComponent<ShrineStateMachine>();

            if (npc.GetComponent<NPCStateMachine>() == null)
            {
                npc.gameObject.AddComponent<NPCStateMachine>();
            }
            if (npc.GetComponent<EnemyHealth>() == null && npc.entityType == TypeofEntity.Oni)
            {
                npc.gameObject.AddComponent<EnemyHealth>();
            }
            if (npc.GetComponent<ShrineStateMachine>() == null)
            {
                npc.gameObject.AddComponent<ShrineStateMachine>();
            }
            if (npc.GetComponent<ShrineHealth>() == null && npc.entityType == TypeofEntity.Shrine)
            {
                npc.gameObject.AddComponent<ShrineHealth>();
            }
            if (npc.GetComponent<NpcMovement>()!=null ) 
            { 
                npc.gameObject.GetComponent<NPCStateMachine>().npcMovement = npc.gameObject.GetComponent<NpcMovement>();
            
            }
            npc.gameObject.GetComponent<NPCStateMachine>().enemyHealth = npc.gameObject.GetComponent<EnemyHealth>();
            npc.gameObject.GetComponent<ShrineStateMachine>().shrineHealth = npc.gameObject.GetComponent<ShrineHealth>();
        }
        
        else if (npc.GetComponent<NPCStateMachine>() != null)
        {
            npc.GetComponent<NPCStateMachine>().enemyHealth = null;
            npc.GetComponent<NPCStateMachine>().npcMovement = null;

            Undo.DestroyObjectImmediate(npc.GetComponent<NPCStateMachine>());
        } 
        else if (npc.GetComponent<EnemyHealth>() != null) 
        { 
            Undo.DestroyObjectImmediate(npc.GetComponent<EnemyHealth>()); 
        } 
        else if (npc.GetComponent<ShrineStateMachine>() != null) 
        {
            npc.GetComponent<ShrineStateMachine>().shrineHealth=null;
            Undo.DestroyObjectImmediate (npc.GetComponent<ShrineStateMachine>()); 
        }
        else if (npc.GetComponent<ShrineHealth>() != null) 
        { 
            Undo.DestroyObjectImmediate(npc.GetComponent<ShrineHealth>()); 
        }
       
        switch (npc.typeofNPC)
        {
            case TypeofNPC.friendly:
                
                break;

            case TypeofNPC.neutral:
                npc.activationStateType = (TypeofActivation)EditorGUILayout.EnumPopup("Aktivation of Behavior", npc.activationStateType);
                switch (npc.activationStateType) 
                {
                    case TypeofActivation.Collision:
                        npc.tagName = EditorGUILayout.TagField("Tag", npc.tagName);
                        break;

                    case TypeofActivation.Time:  npc.atktimer = EditorGUILayout.FloatField("Atk timer", npc.atktimer);
                        npc.cooldown = EditorGUILayout.FloatField("Cooldown", npc.cooldown);
                        break;

                    case TypeofActivation.Input: 
                        npc.keyCode =(KeyCode)EditorGUILayout.EnumPopup("keyCode",npc.keyCode);
                        break;
                }

                GUILayout.Space(10);
                npc.entityType = (TypeofEntity)EditorGUILayout.EnumPopup("Entity", npc.entityType);
                break;

            case TypeofNPC.agressive:
                npc.GetComponent<NPCStateMachine>().enabled = true;
                npc.GetComponent<ShrineStateMachine>().enabled = true;
                npc.entityType = (TypeofEntity)EditorGUILayout.EnumPopup("Entity", npc.entityType);
                break;
        }

        #region neutral or agressive
        if (npc.typeofNPC!= TypeofNPC.friendly) {
            switch (npc.entityType)
            {
                #region ONi
                case TypeofEntity.Oni:
                    npc.oniType = (TypeofOni)EditorGUILayout.EnumPopup("Oni type", npc.oniType);
                    if (npc.GetComponent<ShrineHealth>() != null) { Undo.DestroyObjectImmediate(npc.GetComponent<ShrineHealth>()); }
                    if (npc.GetComponent<ShrineStateMachine>() != null) { Undo.DestroyObjectImmediate(npc.GetComponent<ShrineStateMachine>()); }
                    npc.canMove = true;
                    switch (npc.oniType)
                    {
                        case TypeofOni.Tank:
                            npc.gameObject.tag = "Tank";
                            break;
                        case TypeofOni.Ranged: npc.gameObject.tag = "RangedCombat"; break;
                        case TypeofOni.closed: npc.gameObject.tag = "CloseCombat"; break;
                            case TypeofOni.none: npc.gameObject.tag = "Untagged"; break;
                    }
                    break;
                #endregion

                #region Shrine
                case TypeofEntity.Shrine:
                    npc.canMove = false;
                    npc.typeofShrine = (TypeofShrine)EditorGUILayout.EnumPopup("shrine type", npc.typeofShrine);
                    npc.ShrinetypeChilder();

                    if (npc.GetComponent<EnemyHealth>() != null)
                    {
                        Undo.DestroyObjectImmediate(npc.GetComponent<EnemyHealth>()); 
                    }
                    if (npc.GetComponent<NPCStateMachine>() != null) 
                    { 
                        Undo.DestroyObjectImmediate(npc.GetComponent<NPCStateMachine>()); 
                    }
                    npc.gameObject.tag = "shrine";
                    break;
                #endregion

                #region None
                case TypeofEntity.none:
                    npc.gameObject.tag = "Untagged";
                    if (npc.GetComponent<EnemyHealth>() != null) 
                    { 
                        Undo.DestroyObjectImmediate(npc.GetComponent<EnemyHealth>()); 
                    }
                    if (npc.GetComponent<NPCStateMachine>() != null)
                    {
                        Undo.DestroyObjectImmediate(npc.GetComponent<NPCStateMachine>());
                        
                    }
                    if (npc.GetComponent<ShrineHealth>() != null) 
                    {
                        Undo.DestroyObjectImmediate(npc.GetComponent<ShrineHealth>());
                    }
                    if (npc.GetComponent<ShrineStateMachine>() != null)
                    {
                        Undo.DestroyObjectImmediate(npc.GetComponent<ShrineStateMachine>());
                    }
                        break;
                #endregion
            }
        #endregion
        }
        #region friendly or not Shrine
        if (npc.typeofNPC == TypeofNPC.friendly || npc.entityType != TypeofEntity.Shrine)
        {
            GameObject objectpool = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Scripts/other/NPCStateMachine/Shrine/shrinePrefabs/objectpool.prefab");
            GameObject rockManager = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Scripts/other/NPCStateMachine/Shrine/shrinePrefabs/rockmanager.prefab");

            Transform rockManagerChild = npc.transform.Find(rockManager.name);
            Transform objectPoolChild = npc.transform.Find(objectpool.name);
            RockManager rm = npc.GetComponentInChildren<RockManager>(true);

            if (rm != null)
            {
                rm.RockPool = null;
            }

            ShrineStateMachine shrine = npc.GetComponent<ShrineStateMachine>();

            if (shrine != null)
            {
                shrine.rockmanager = null;
            }

            if (rockManagerChild != null)
            {
                Undo.DestroyObjectImmediate(rockManagerChild.gameObject);
            }

            if (objectPoolChild != null)
            {
                Undo.DestroyObjectImmediate(objectPoolChild.gameObject);
            }
        }
        #endregion
        GUILayout.Space(10);
        #endregion
    }
}