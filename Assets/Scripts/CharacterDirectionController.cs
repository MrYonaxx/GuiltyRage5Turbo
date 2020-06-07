using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{
    public class CharacterDirectionController : MonoBehaviour
    {
        [SerializeField]
        Character character;

        [SerializeField]
        AttackController attackController;

        public void Start()
        {
            character.Direction = attackController.Direction;
        }
    }
}
