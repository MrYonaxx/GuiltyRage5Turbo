/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "AttackData", order = 1)]
    public class AttackData : ScriptableObject
    {




        [HorizontalGroup("CardBasicInfo", Width = 96)]
        [HideLabel]
        [PreviewField(ObjectFieldAlignment.Left, Height = 96)]
        [SerializeField]
        private Sprite cardSprite;
        public Sprite CardSprite
        {
            get { return cardSprite; }
        }


        [HorizontalGroup("CardBasicInfo")]
        [VerticalGroup("CardBasicInfo/Right")]
        [SerializeField]
        string attackName;

        [VerticalGroup("CardBasicInfo/Right")]
        [SerializeField]
        [ValueDropdown("SelectCardType")]
        string cardType;
        public string CardType
        {
            get { return cardType; }
        }
        [VerticalGroup("CardBasicInfo/Right")]
        [SerializeField]
        [ValueDropdown("SelectCardElement")]
        string[] cardElement;
        public string[] CardElement
        {
            get { return cardElement; }
        }

        [HorizontalGroup("CardBasicInfo")]
        [VerticalGroup("CardBasicInfo/Right")]
        [SerializeField]
        string animationName;
        public string AnimationName
        {
            get { return animationName; }
        }



        [Space]
        [Space]
        [SerializeField]
        AttackData canComboTo;
        public AttackData CanComboTo
        {
            get { return canComboTo; }
        }

        [Space]
        [Title("Attack")]
        [SerializeField]
        [HideLabel]
        AttackController customAttackController;
        public AttackController CustomAttackController
        {
            get { return customAttackController; }
        }

        [SerializeField]
        [HideLabel]
        AttackBehavior attackBehavior;
        public AttackBehavior AttackBehavior
        {
            get { return attackBehavior; }
        }



    } 

} // #PROJECTNAME# namespace