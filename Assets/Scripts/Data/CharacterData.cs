﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData", order = 1)]
    public class CharacterData : ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */


        [HorizontalGroup("CharacterBasic", Width = 96, PaddingLeft = 10)]
        [HideLabel]
        [PreviewField(ObjectFieldAlignment.Left, Height = 96)]
        [SerializeField]
        Sprite characterFace;
        public Sprite CharacterFace
        {
            get { return characterFace; }
        }

        [HorizontalGroup("CharacterBasic", PaddingLeft = 10)]
        [VerticalGroup("CharacterBasic/Right")]
        [SerializeField]
        string characterName;
        public string CharacterName
        {
            get { return characterName; }
        }

        [SerializeField]
        [HideLabel]
        CharacterStat characterStat;
        public CharacterStat CharacterStat
        {
            get { return characterStat; }
        }

        [Space]
        [Space]
        [Space]
        [Title("Audio")]
        [SerializeField]
        private AudioClip[] hitVoice;
        public AudioClip[] HitVoice
        {
            get { return hitVoice; }
        }
        [SerializeField]
        private AudioClip[] guardVoice;
        public AudioClip[] GuardVoice
        {
            get { return guardVoice; }
        }
        [SerializeField]
        private AudioClip[] deadVoice;
        public AudioClip[] DeadVoice
        {
            get { return deadVoice; }
        }


        [SerializeField]
        private AudioClip[] downSound;
        public AudioClip[] DownSound
        {
            get { return downSound; }
        }


        #endregion

    }

} // #PROJECTNAME# namespace
