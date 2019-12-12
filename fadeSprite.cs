using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Este script irá en cada objeto tras el que el hamster pueda entrar y hará que el objeto se torne medio transparente para así ver si está el enemigo dentro.
 * 
 */
public class fadeSprite : MonoBehaviour
{
    SpriteRenderer sr;
    bool transparent;
    public float fadeSpeed;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            fade();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            fade();
        }

    }
    void fade()
    {
        if (!transparent)
        {
            transparent = true;
            StartCoroutine(fadeOut());
        }
        else
        {
            transparent = false;
            StartCoroutine(fadeIn());
        }
    }
    IEnumerator fadeIn() //volver a normal
    {
        while (sr.color.a < 0.99f)
        {
            Color newColor = sr.color;
            newColor.a += fadeSpeed * Time.deltaTime;
            sr.color = newColor;
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator fadeOut() //volver transparente
    {
        while (sr.color.a > 0.5f)
        {
            Color newColor = sr.color;
            newColor.a -= fadeSpeed * Time.deltaTime;
            sr.color = newColor;
            yield return new WaitForEndOfFrame();
        }
    }
}
