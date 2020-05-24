/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class BattleManager: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Title("Game Parameter")]
        [SerializeField]
        GameRunData runData;
        [SerializeField]
        CameraBattleController cameraController;

        [SerializeField]
        BattleFeedbackManager battleFeedbackManager;

        [SerializeField]
        List<PlayerData> enemiesDatas;


        [Title("Player")]
        [SerializeField]
        List<Character> players;
        [SerializeField]
        HealthBarDrawer playerHealthBar;

        [Title("Enemies")]
        [SerializeField]
        EnemyController enemyPrefab;
        [SerializeField]
        HealthBarDrawer enemyHealthBar;
        [SerializeField]
        List<TextMeshProUGUI> enemiesName;


        [Title("Event")]
        [SerializeField]
        UnityEvent OnEventBattleStart;
        [SerializeField]
        UnityEvent OnEventBattleEnd;

        Character enemyTarget;

        [Title("Debug")]
        [SerializeField]
        List<EnemyController> enemiesController;




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


        public void InitializePlayers()
        {
            for (int i = 0; i < players.Count; i++)
            {
                //tmp.Add(players[i]);
                players[i].SetCharacter(runData.PlayerCharacterData, runData.PlayerStats);
                players[i].SetActive(true);
            }
        }

        public void InitializeBattle()
        {
            enemiesController.Clear();
            enemyPrefab.gameObject.SetActive(true);
            List<Character> tmp = new List<Character>();
            for (int i = 0; i < players.Count; i++)
            {
                tmp.Add(players[i]);
                players[i].SetCharacter(runData.PlayerCharacterData, runData.PlayerStats);
                players[i].SetActive(true);
            }
            playerHealthBar.DrawCharacter(runData.PlayerCharacterData, runData.PlayerStats);
            for(int i = 0; i < enemiesDatas.Count; i++)
            {
                enemiesController.Add(Instantiate(enemyPrefab, new Vector3(0,0,0), Quaternion.identity));
                enemiesController[i].SetCharacter(enemiesDatas[i],new CharacterStatController(enemiesDatas[i]));
                tmp.Add(enemiesController[i]);
            }
            enemyPrefab.gameObject.SetActive(false);

            battleFeedbackManager.SetBattleCharacters(tmp);

            //player.PlayIdleAnimation();
            OnEventBattleStart.Invoke();
        }

        public void SetTarget(Character character)
        {
            if (enemyTarget != character && character != null)
            {
                enemyHealthBar.gameObject.SetActive(true);
                enemyHealthBar.DrawCharacter(character.CharacterData, character.CharacterStat);
                cameraController.SetLocked(character.transform);
            }
            else if (character == null)
            {
                enemyHealthBar.gameObject.SetActive(false);
                cameraController.SetLocked(null);
            }
            players[0].SetTarget(character);
            enemyTarget = character;

        }

        public void DrawHpPlayer()
        {
            playerHealthBar.DrawHealth(players[0].CharacterStat.GetHP(), players[0].CharacterStat.GetHPMax());
        }

        public void DrawHpEnemy()
        {
            if (enemyTarget == null)
                return;
            enemyHealthBar.DrawHealth(enemyTarget.CharacterStat.GetHP(), enemyTarget.CharacterStat.GetHPMax());
        }


        public void BattleStart()
        {

        }

        public void CreateEnemies()
        {

        }

        public void CharacterDead(Character character)
        {

            /*if (players.Count == 0)
                GameOver();*/
            if (enemiesController.Count == 1)
            {
                EndBattle();
            }
            else
            {
                ((EnemyController)character).EnemyAppear(false);
                enemiesController.Remove((EnemyController)character);

                // Faudra filer une reference a battleFeedbackManager pour par reupdate à chaque fois
                List<Character> tmp = new List<Character>();
                tmp.Add(players[0]);
                for (int i = 0; i < enemiesController.Count; i++)
                {
                    tmp.Add(enemiesController[i]);
                }
                battleFeedbackManager.SetBattleCharacters(tmp);
            }
        }

        public void GameOver()
        {

        }

        public void EndBattle()
        {
            players[0].SetActive(false);
            battleFeedbackManager.EndBattleMotionSpeed();
            StartCoroutine(EndBattleCoroutine());
        }

        private IEnumerator EndBattleCoroutine()
        {
            yield return new WaitForSeconds(4);
            for (int i = 0; i < enemiesController.Count; i++)
                enemiesController[i].EnemyAppear(false);
            OnEventBattleEnd.Invoke();
        }


        #endregion

    } 

} // #PROJECTNAME# namespace