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
    // Data de la run actuelle
    [CreateAssetMenu(fileName = "GameData", menuName = "GameData", order = 1)]
    public class GameRunData : ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        private List<PlayerData> players;
        public List<PlayerData> Players
        {
            get { return players; }
        }


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
        public void CreateRunData(PlayerData playerInitialData)
        {
            /*playerCharacterData = playerInitialData;
            playerStats.CreateStatController(playerInitialData);

            floor = 0;*/
        }


        #endregion

    } 

} // #PROJECTNAME# namespace