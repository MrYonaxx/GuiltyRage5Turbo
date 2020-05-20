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
    public class GameRunManager: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        GameRunData runData;

        [Title("Player")]
        [SerializeField]
        PlayerData playerData;
        [SerializeField]
        PlayerController playerController;

        [Title("Enemy")]
        [SerializeField]
        List<PlayerData> enemyDatas;

        [Title("Battle")]
        [SerializeField]
        BattleManager battleManager;


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
            CreatePlayer();
            battleManager.InitializeBattle();
        }

        public void CreatePlayer()
        {
            runData.CreateRunData(playerData);
            //playerController.SetCharacter(runData.PlayerCharacterData, runData.PlayerDeck, runData.PlayerStats);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace