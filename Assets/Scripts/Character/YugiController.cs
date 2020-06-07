using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [System.Serializable]
    public class YugiCardData
    {
        [HideLabel]
        [HorizontalGroup("YugiCard")]
        public AttackController AttackController;

        [HorizontalGroup("YugiCard")]
        [HideLabel]
        public Sprite CardSprite;
        //public int CardLevel;

    }

    public class YugiController : PlayerController
    {
        [Title("DeckData")]
        [SerializeField]
        YugiCardData[] deckData;



        [Title("Deck Parameter")]
        [SerializeField]
        int handNumber = 3;

        [SerializeField]
        int handNeutral = 0;
        [SerializeField]
        int handAerial = 1;
        [SerializeField]
        int handForward = 2;

        [SerializeField]
        Animator animatorCard;
        [HorizontalGroup("Renderer")]
        [SerializeField]
        List<Animator> animators = new List<Animator>();
        [HorizontalGroup("Renderer")]
        [SerializeField]
        List<SpriteRenderer> handSprites = new List<SpriteRenderer>();

        List<YugiCardData> currentDeck = new List<YugiCardData>();
        List<YugiCardData> hand = new List<YugiCardData>(3);

        protected override void Start()
        {
            base.Start();
            hand = new List<YugiCardData>(handNumber);
            for (int i = 0; i < handNumber; i++)
            {
                hand.Add(null);
            }
            List<YugiCardData> tmp = new List<YugiCardData>(deckData.Length);
            for (int i = 0; i < deckData.Length; i++)
            {
                tmp.Add(deckData[i]);
            }
            int r = 0;
            while(tmp.Count > 0)
            {
                r = Random.Range(0, tmp.Count);
                currentDeck.Add(tmp[r]);
                tmp.RemoveAt(r);
            }        
        }

        public override void UpdateController(Character c)
        {
            base.UpdateController(c);
            CheckCardDirection();
        }

        protected override void NormalAttack(Character character)
        {
            DrawCard();
            base.NormalAttack(character);
        }

        protected override void SpecialAttack()
        {
            if (Mathf.Abs(Input.GetAxis(controllerLeftHorizontal)) > 0.2f)
            {
                if (PlayCard(handForward))
                    return;
            }
            else if (character.InAir == true)
            {
                if (PlayCard(handAerial))
                    return;
            }
            if (PlayCard(handNeutral))
                return;
            else if (PlayCard(handAerial))
                return;
            else if (PlayCard(handForward))
                return;
            base.SpecialAttack();
        }


        private void CheckCardDirection()
        {
            animatorCard.SetInteger("Direction", character.Direction);
        }


        public bool PlayCard(int index)
        {
            if (hand[index] != null)
            {
                character.Action(hand[index].AttackController);
                hand[index] = null;
                animators[index].SetTrigger("Disappear");
                return true;
            }
            return false;
        }


        public void DrawCard()
        {
            int r = Random.Range(0, currentDeck.Count / 2);
            for(int i = 0; i < hand.Count; i++)
            {
                if (hand[i] == null)
                {
                    hand[i] = currentDeck[r];
                    handSprites[i].sprite = hand[i].CardSprite;
                    animators[i].SetTrigger("Appear");
                    currentDeck.Add(currentDeck[r]);
                    currentDeck.RemoveAt(r);
                    break;
                }
            }
        }

    }
}
