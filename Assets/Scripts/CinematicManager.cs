using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CinematicEvent
{
    [SerializeField]
    public UnityEvent cineEvent;
    [SerializeField]
    public float timeEvent;
}

public class CinematicManager : MonoBehaviour
{
    [SerializeField]
    Color debugColor;

    [SerializeField]
    List<CinematicEvent> cinematicEvents = new List<CinematicEvent>();

    public void StartCinematic()
    {
        StartCoroutine(CinematicCouroutine());
    }

    private IEnumerator CinematicCouroutine()
    {
        for(int i = 0; i < cinematicEvents.Count; i++)
        {
            cinematicEvents[i].cineEvent.Invoke();
            yield return new WaitForSeconds(cinematicEvents[i].timeEvent);
        }
    }


    public void SetAmbientLight()
    {
        RenderSettings.ambientLight = debugColor;
    }

}
