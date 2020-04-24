using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.NPC
{
    public class NpcMainScript : GameCharacter
    {
        [FormerlySerializedAs("mobObject")] public NpcObject npcObject;
    }
}
