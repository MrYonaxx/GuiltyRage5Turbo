using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{
    [CreateAssetMenu(fileName = "BattleFeedbackManagerData", menuName = "BattleFeedbackManagerData", order = 1)]
    public class BattleFeedbackManagerData : ScriptableObject
    {


        [SerializeField]
        private List<Character> charactersScene;
        public List<Character> CharactersScene
        {
            get { return charactersScene; }
        }


        public void AddCharacter(Character character)
        {
            charactersScene.Add(character);
        }

        public void RemoveCharacter(Character character)
        {
            charactersScene.Remove(character);
        }


        /*public void CharacterGuard(Character character)
        {

        }
        public void GuardFeedback(Character character)
        {

        }
        public void DeadFeedback(Character character)
        {

        }*/


    }
}
