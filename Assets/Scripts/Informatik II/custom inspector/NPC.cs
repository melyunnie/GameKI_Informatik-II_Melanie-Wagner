using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public enum TypeofNPC { friendly, neutral, agressive }
public enum TalkType { monologe, dialogue, decision }
public enum TypeofEntity { none, Oni, Shrine }
public enum TypeofOni { none, Tank, Ranged, closed }
public enum TypeofShrine { Oyamatsumi }//Hachiman, Susanoo
public enum TypeofActivation { Input, Time, Collision }

public class NPC : MonoBehaviour
{
    #region variables
    public TypeofNPC typeofNPC;
    public TalkType talkType;
    public TypeofEntity entityType;
    public TypeofOni oniType;
    public TypeofShrine typeofShrine;
    public TypeofActivation activationStateType;

    public bool canMove;
    public string age;
    public string npcDescription;
    public Sprite npcSprite;
    public KeyCode keyCode;
    public bool keypressed = false;
    public float atktimer;
    public float cooldown;
    public string tagName;

    #region talking
    public bool canTalk;
    
    public GameObject talkPanel;
    public TMP_Text talkText, nameText;
    public Image portraitImage;
    public float typingSpeed;
    private bool isTyping, isTalkingActive;

    public NPCMonologe monologeData;
    private int monologueIndex;
    

    public NPCDialogue dialogue;
    private Dictionary<string, NPCDialogueNode> dialogueNode = new Dictionary<string, NPCDialogueNode>();
    private NPCDialogueNode currentdialogueNode;

    public Button DecisionButtonPrefab;
    public Transform DecisionButtonContainer;
    #endregion

    #endregion

    void Start()
    {
        StartCoroutine(EventTimer());
    }
    void Update()
    {
        GetNPCType();
        InputEvent();
        StartCoroutine(EventTimer());
    }
    public void GetNPCType()
    {
        if (canTalk == true)
        {
            if (Input.GetKeyUp(KeyCode.E)) { NPCTalk(); }
            if (Input.GetKeyDown(KeyCode.Space) && talkPanel.activeSelf)
            {
                Nextline();
            }
        }
        if (typeofNPC == TypeofNPC.neutral)
        {
            switch (activationStateType)
            {
                case TypeofActivation.Collision: break;
                case TypeofActivation.Input:
                    if (Input.GetKeyDown(KeyCode.Space) && talkPanel.activeSelf)
                    {

                        Nextline();
                    }
                    break;
                case TypeofActivation.Time:

                    break;
            }
        }
    }
    void NPCTalk()
    {
        if (isTalkingActive) { Nextline(); } else StartTalking();
    }
    void StartTalking()
    {
        isTalkingActive = true;
        switch (talkType)
        {
            case TalkType.monologe:
                monologueIndex = 0;
                nameText.SetText(monologeData.npcName);
                
                StartCoroutine(TypeLine());
                break;

            case TalkType.dialogue:
            case TalkType.decision:

                foreach (NPCDialogueNode node in dialogue.AllNodes)
                {
                    dialogueNode[node.NodeID] = node;
                }

                if (!string.IsNullOrEmpty(dialogue.EntryNodeID))
                {
                    ShowNode(dialogue.EntryNodeID);
                }
                else
                {
                    TalkingEnd();
                }
                break;
        }
        talkPanel.SetActive(true);
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        talkText.SetText("");
        string line = "";

        switch (talkType)
        {
            case TalkType.monologe:
                line = monologeData.monologuelist[monologueIndex];
                break;

            case TalkType.dialogue:
            case TalkType.decision:
                line = currentdialogueNode.DialogueText;
                break;
        }

        foreach (char letter in line)
        {
            talkText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
    public void Nextline()
    {
        switch (talkType)
        {
            case TalkType.monologe:

                if (isTyping)
                {
                    StopAllCoroutines();
                    talkText.SetText(monologeData.monologuelist[monologueIndex]);
                    isTyping = false;
                    return;
                }

                monologueIndex++;

                if (monologueIndex < monologeData.monologuelist.Length)
                {
                    StartCoroutine(TypeLine());
                }
                else
                {
                    TalkingEnd();
                }
                break;

            case TalkType.dialogue:
            case TalkType.decision:
                if (isTyping)
                {
                    StopAllCoroutines();
                    talkText.SetText(currentdialogueNode.DialogueText);
                    isTyping = false;
                    return;
                }

                if (!string.IsNullOrEmpty(currentdialogueNode.NextNodeID))
                {
                    ShowNode(currentdialogueNode.NextNodeID);
                }
                else
                {
                    TalkingEnd();
                }

                break;
        }
    }
    public void TalkingEnd()
    {
        StopAllCoroutines();
        isTalkingActive = false;
        talkText.SetText("");
        talkPanel.SetActive(false);
        currentdialogueNode = null;

        foreach (Transform child in DecisionButtonContainer) 
        { 
            Destroy(child.gameObject); 
        }
    }

    private void ShowNode(string nodeID)
    {
        if (!dialogueNode.ContainsKey(nodeID))
        {
            TalkingEnd();
            return;
        }

        currentdialogueNode = dialogueNode[nodeID];
        talkPanel.SetActive(true);
        nameText.SetText(currentdialogueNode.SpeakerName);

        foreach (Transform child in DecisionButtonContainer)
            Destroy(child.gameObject);

            if (currentdialogueNode.Decision.Count > 0)
            {
                foreach (DecisionData decision in currentdialogueNode.Decision)
                {
                    Button button = Instantiate(DecisionButtonPrefab, DecisionButtonContainer);

                    TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

                    if (buttonText != null)
                    {
                        buttonText.text = decision.DescisionText;
                    }
                    else
                    {
                        Debug.LogError("Kein TMP_Text im Button gefunden!");
                    }

                    button.onClick.AddListener (delegate { ShowNode(decision.DestinationNodeID);});
                }
        }
        StartCoroutine(TypeLine());
    }
    private void InputEvent()
    {
        if (Input.GetKeyDown(keyCode) && keypressed != true)
        {
            if (GetComponent<ShrineStateMachine>() != null) 
            { 
                GetComponent<ShrineStateMachine>().enabled = true; 
            }
            else if (GetComponent<NPCStateMachine>() != null) 
            { 
                GetComponent<NPCStateMachine>().enabled = true; 
            }

            keypressed = true;
            
            if (GetComponent<ShrineStateMachine>() != null) 
            { 
                GetComponent<ShrineStateMachine>().enabled = true;
            } 
            else if (GetComponent<NPCStateMachine>() != null) 
            { 
                GetComponent<NPCStateMachine>().enabled = true; 
            }
        }
        else if (Input.GetKeyDown(keyCode) && keypressed == true) 
        { 
            keypressed = false; 
            if (GetComponent<ShrineStateMachine>() != null) 
            {
                GetComponent<ShrineStateMachine>().enabled = false; 
            }
            else if (GetComponent<NPCStateMachine>() != null) 
            { 
                GetComponent<NPCStateMachine>().enabled = false; 
            }
        }
    }

    IEnumerator EventTimer()
    {
        if (activationStateType == TypeofActivation.Time)
        {
            if (GetComponent<ShrineStateMachine>() != null)
            { GetComponent<ShrineStateMachine>().enabled = false; }
            else if (GetComponent<NPCStateMachine>() != null)
            { GetComponent<NPCStateMachine>().enabled = false; }

            yield return new WaitForSeconds(cooldown);

            if (GetComponent<ShrineStateMachine>() != null)
            { GetComponent<ShrineStateMachine>().enabled = true; }
            else if (GetComponent<NPCStateMachine>() != null)
            { GetComponent<NPCStateMachine>().enabled = true; }
            yield return new WaitForSeconds(atktimer);

            StartCoroutine(EventTimer());
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (activationStateType == TypeofActivation.Collision) { }
        if (other.CompareTag(tagName))
        {
            if (GetComponent<ShrineStateMachine>() != null)
                GetComponent<ShrineStateMachine>().enabled = true;
            else if (GetComponent<NPCStateMachine>() != null)
                GetComponent<NPCStateMachine>().enabled = true;
        }
    }
    public void ShrinetypeChilder()
    {
        GameObject objectpool = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Scripts/other/NPCStateMachine/Shrine/shrinePrefabs/objectpool.prefab");
        GameObject rockManager = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Scripts/other/NPCStateMachine/Shrine/shrinePrefabs/rockmanager.prefab");

     if (objectpool != null)
       {
            Transform childobjectpool = transform.Find(objectpool.name);

            if (childobjectpool == null)
           {
               GameObject objectPoolChild = (GameObject)PrefabUtility.InstantiatePrefab(objectpool, transform);
            }
      }

        if (rockManager != null)
        {
            Transform childrockManager = transform.Find(rockManager.name);

            if (childrockManager == null)
            {
                GameObject rockManagerChild = (GameObject)PrefabUtility.InstantiatePrefab(rockManager, transform);
            }
        }
        rockManager.SetActive(false);
        gameObject.GetComponent<ShrineStateMachine>().rockmanager = rockManager;
        Objectpool rookpool = objectpool.GetComponentInChildren<Objectpool>(true);
        rockManager.gameObject.GetComponent<RockManager>().RockPool = rookpool;
    }
}
