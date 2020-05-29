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
using Sirenix.Serialization;

namespace VoiceActing
{
    public enum Logique
    {
        AND,
        OR
    }

    [System.Serializable]
    public struct EnemyBehaviorSequence
    {
        [VerticalGroup(PaddingTop = 20)]
        [SerializeField]
        [HideLabel]
        public string SequenceID;

        [VerticalGroup("Pattern", PaddingBottom = 30)]
        [OdinSerialize]
        [ListDrawerSettings(Expanded = true, AlwaysAddDefaultValue = true)]
        public List<EnemyBehavior> behaviours;
    }

    [System.Serializable]
    public struct EnemyPattern
    {

        [VerticalGroup(PaddingTop = 20)]
        [SerializeField]
        [HideLabel]
        public string PatternID;

        [SerializeField]
        [VerticalGroup("Pattern", PaddingBottom = 30)]
        [HorizontalGroup("Pattern/Right", LabelWidth = 100, Width = 200)]
        [ListDrawerSettings(Expanded = true, AlwaysAddDefaultValue = true)]
        public List<EnemyCondition> conditions;

        [OdinSerialize]
        [HorizontalGroup("Pattern/Right")]
        [ListDrawerSettings(Expanded = true, AlwaysAddDefaultValue = true)]
        public List<EnemyBehavior> behaviours;

    }

    [System.Serializable]
    public struct EnemyCondition
    {
        [HorizontalGroup]
        public CharacterState targetState;
        /*[HorizontalGroup]
        public Logique logique;*/

    }
    public class EnemyController : SerializedMonoBehaviour, ICharacterController
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Enemy")]
        [SerializeField]
        float timeEnemyAppear = 0.5f;
        [SerializeField]
        float timeEnemyDisappear = 2f;

        [OdinSerialize]
        [TabGroup("Decision Tree")]
        [System.NonSerialized]
        EnemyPattern[] patterns = new EnemyPattern[0];

        [OdinSerialize]
        [TabGroup("Sequence")]
        [System.NonSerialized]
        EnemyBehaviorSequence[] behaviorSequences;

        [OdinSerialize]
        [System.NonSerialized]
        [SerializeField]
        EnemyBehavior startUpPattern;


        EnemyBehavior currentPattern;

        float enemyActionTime = 1;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        private void Start()
        {
            currentPattern = startUpPattern;
            EnemyAppear(true);
        }

        public void EnemyAppear(bool b)
        {
            if (b == true)
            {
                StartCoroutine(EnemyAppearCoroutine());
            }
            else
            {
                StartCoroutine(EnemyDisappearCoroutine());
            }
        }

        private IEnumerator EnemyAppearCoroutine()
        {
            Vector3 startPosition = new Vector3(1, 0, 1);
            Vector3 finalPosition = new Vector3(1, 1, 1);
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / timeEnemyAppear;
                this.transform.localScale = Vector3.Lerp(startPosition, finalPosition, t);
                yield return null;
            }
        }

        private IEnumerator EnemyDisappearCoroutine()
        {
            Vector3 startPosition = new Vector3(1, 1, 1);
            Vector3 finalPosition = new Vector3(1, 0, 1);
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / timeEnemyDisappear;
                this.transform.localScale = Vector3.Lerp(startPosition, finalPosition, t);
                yield return null;
            }
            Destroy(this.gameObject);
        }











        public void UpdateController(Character character)
        {
            EnemyDecision(character);
        }
        public void LateUpdateController(Character character)
        {
            CheckInterrupt(character);
        }
        private void CheckInterrupt(Character character)
        {
            if (character.State == CharacterState.Hit)
            {
                currentPattern = null;
            }
        }



        public void EnemyDecision(Character character)
        {
            if (character.State == CharacterState.Hit || character.State == CharacterState.Down || character.State == CharacterState.Dead)
                return;
            //patterns
            if (currentPattern == null)
            {
                SelectAction(character);
                enemyActionTime = currentPattern.StartBehavior(this, character);
            }

            enemyActionTime -= Time.deltaTime;
            currentPattern.UpdateBehavior(this, character);
            if (enemyActionTime <= 0)
                currentPattern = null;
        }

        private void SelectAction(Character character)
        {
            for(int i = 0; i < patterns.Length; i++)
            {
                for (int j = 0; j < patterns[i].conditions.Count; j++)
                {
                    if (patterns[i].conditions[j].targetState == character.Target.State)
                    {
                        currentPattern = patterns[i].behaviours[Random.Range(0, patterns[i].behaviours.Count)];
                        return;
                    }
                }
            }
        }

        public void StopPattern()
        {
            currentPattern = null;
            enemyActionTime = 0;
        }

        public void ForcePattern(EnemyBehavior behavior, Character character)
        {
            currentPattern = behavior;
            enemyActionTime = currentPattern.StartBehavior(this, character);

        }

        public void ForceBehavior(EnemyBehavior behavior, Character character)
        {
            currentPattern = behavior;
            enemyActionTime = currentPattern.StartBehavior(this, character);

        }

        public void PlaySequence(string sequenceID)
        {

        }

        #endregion

    } 

} // #PROJECTNAME# namespace