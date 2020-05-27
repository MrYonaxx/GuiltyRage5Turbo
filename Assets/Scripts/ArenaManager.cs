using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [System.Serializable]
    public class ArenaData
    {
        [HorizontalGroup]
        public int EnemyToDefeat;

        [HorizontalGroup]
        public GameObject WaveParent;
    }

    public class ArenaManager : MonoBehaviour
    {
        [SerializeField]
        ArenaData[] arenaDatas;

        [SerializeField]
        UnityEvent eventArenaClear;

        [SerializeField]
        CameraBattleController cameraController;
        [SerializeField]
        Vector2 arenaCameraClamp;
        [SerializeField]
        Vector2 arenaEndCameraClamp;
        [SerializeField]
        GameObject arenaWall;

        int currentWave = 0;
        int killCount = 0;


        public void StartArena()
        {
            cameraController.SetNewClampX(arenaCameraClamp);
            arenaWall.SetActive(true);
        }

        public void AddKillCount()
        {
            killCount += 1;
            if(killCount >= arenaDatas[currentWave].EnemyToDefeat)
            {
                NextWave();
            }
        }

        public void NextWave()
        {
            if (currentWave >= arenaDatas.Length-1)
            {
                EndArena();
            }
            else
            {
                arenaDatas[currentWave].WaveParent.SetActive(true);
                currentWave += 1;
            }

        }

        public void EndArena()
        {
            eventArenaClear.Invoke();
            cameraController.SetNewClampX(arenaEndCameraClamp);
            arenaWall.SetActive(false);
        }
    }
}
