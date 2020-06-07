using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkScript : MonoBehaviour
{
    float flash = 0;
    Renderer spriteRenderer;
    private IEnumerator blinkCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<Renderer>();
    }

    public void Blink()
    {
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);
        blinkCoroutine = BlinkCoroutine();
        StartCoroutine(blinkCoroutine);
    }

    private IEnumerator BlinkCoroutine()
    {
        flash = 1;
        float t = 0f;
        while(t < 0.5f)
        {
            flash = Mathf.Lerp(1, 0, t * 2);
            t += Time.deltaTime;
            spriteRenderer.material.SetFloat("_FlashAmount", flash);
            yield return null;
        }
    }
}
