using System;
using Characters.Animations;
using Characters.Systems.Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Characters.NPC
{
    public class NpcMainScript : GameCharacter, ICombatTarget
    {
        [FormerlySerializedAs("mobObject")] public NpcObject npcObject;

        public void Start() {
            AnimatorController = new NpcAnimatorController(GetComponent<Animator>());
        }

        public void TakeDamage(DamageProperty damage) {
            npcObject.Health.AddPoints(-damage.value);
            AnimatorController.OnTakeDamage();
            if (npcObject.Health.CurrentPoints == 0) {
                AnimatorController.OnDie();
                GetComponent<CharacterController>().enabled = false;
                transform.GetChild(0).GetComponent<MeshCollider>().enabled = false;
            }
        }
    }
}
