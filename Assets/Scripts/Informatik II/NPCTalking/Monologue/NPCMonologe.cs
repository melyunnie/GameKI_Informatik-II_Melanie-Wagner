using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCMonologue", menuName = "NPC Monologue")]
public class NPCMonologe : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;
    public string[] monologuelist;
}