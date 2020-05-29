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
    public struct EnemyPatternSequence
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
        [HideLabel]
        //[ListDrawerSettings(Expanded = true, AlwaysAddDefaultValue = true)]
        public EnemyCondition conditions;

        [OdinSerialize]
        [HorizontalGroup("Pattern/Right")]
        [ListDrawerSettings(Expanded = true, AlwaysAddDefaultValue = true)]
        public List<EnemyBehavior> behaviours;

    }

    [System.Serializable]
    public struct EnemyCondition
    {
        [SerializeField]
        public Vector3 Distance;

        [ListDrawerSettings(Expanded = true)]
        [SerializeField]
        public List<CharacterState> targetStates;

        public bool CheckCondition(Character character)
        {
            Vector3 dist = character.transform.position - character.Target.transform.position;
            if(Distance.x != 0)
            {
                if (Mathf.Abs(dist.x) > Distance.x)
                    return false;
            }
            if (Distance.y != 0)
            {
                if (Mathf.Abs(dist.y) > Distance.y)
                    return false;
            }
            if (Distance.z != 0)
            {
                if (character.Target.SpriteRenderer.transform.localPosition.y > Distance.z)
                    return false;
            }


            if(targetStates == null)
            {
                return true;
            }
            for (int j = 0; j < targetStates.Count; j++)
            {
                if (targetStates[j] == character.Target.State)
                {
                    return true;
                }
            }


            return false;
        }

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
        [ListDrawerSettings(Expanded = true)]
        EnemyPattern[] patterns;

        [OdinSerialize]
        [TabGroup("Decision Tree Wakeup")]
        [System.NonSerialized]
        [ListDrawerSettings(Expanded = true)]
        EnemyPattern[] wakeUpBehaviors;

        [OdinSerialize]
        [TabGroup("Sequence")]
        [System.NonSerialized]
        EnemyPatternSequence[] behaviorSequences;

        [OdinSerialize]
        [System.NonSerialized]
        [SerializeField]
        EnemyBehavior startUpPattern;



        List<EnemyBehavior> currentPattern = new List<EnemyBehavior>();

        float enemyActionTime = 1;
        bool isHit = false;

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
            currentPattern.Add(startUpPattern);
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




        public void EnemyDecision(Character character)
        {
            if (character.State == CharacterState.Hit || character.State == CharacterState.Down || character.State == CharacterState.Dead)
                return;
            //patterns
            if (currentPattern.Count == 0)
            {
                SelectAction(patterns, character);
            }
            if (enemyActionTime <= 0)
            {
                enemyActionTime = currentPattern[0].StartBehavior(this, character);
            }
            enemyActionTime -= Time.deltaTime;
            currentPattern[0].UpdateBehavior(this, character);
            if (enemyActionTime <= 0)
            {
                currentPattern[0].EndBehavior(this, character);
                currentPattern.RemoveAt(0);
            }
        }

        private void SelectAction(EnemyPattern[] enemyPatterns, Character character)
        {
            for(int i = 0; i < enemyPatterns.Length; i++)
            {
                if (enemyPatterns[i].conditions.CheckCondition(character) == true)
                {
                    currentPattern.Add(enemyPatterns[i].behaviours[Random.Range(0, enemyPatterns[i].behaviours.Count)]);
                    return;
                }
            }
        }

        public void StopPattern()
        {
            enemyActionTime = 0;
        }

        public void ForcePattern(EnemyBehavior behavior, Character character)
        {
            currentPattern[0] = behavior;
            enemyActionTime = currentPattern[0].StartBehavior(this, character);

        }

        public void ForceBehavior(EnemyBehavior behavior, Character character)
        {
            currentPattern[0] = behavior;
            enemyActionTime = currentPattern[0].StartBehavior(this, character);
        }

        public void PlaySequence(string sequenceID)
        {
            for(int i = 0; i < behaviorSequences.Length; i++)
            {
                if(behaviorSequences[i].SequenceID == sequenceID)
                {
                    for(int j = 0; j < behaviorSequences[i].behaviours.Count; j++)
                    {
                        currentPattern.Add(behaviorSequences[i].behaviours[j]);
                    }
                    currentPattern.RemoveAt(0);
                    return;
                }
            }
        }




        public void LateUpdateController(Character character)
        {
            CheckInterrupt(character);
        }
        private void CheckInterrupt(Character character)
        {
            if (character.State == CharacterState.Hit)
            {
                currentPattern.Clear();
                isHit = true;
            }
            else if (isHit == true && character.State == CharacterState.Idle)
            {
                if (wakeUpBehaviors != null)
                {
                    SelectAction(wakeUpBehaviors, character);
                    enemyActionTime = currentPattern[0].StartBehavior(this, character);
                }
                isHit = false;
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace