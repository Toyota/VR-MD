/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public RawImage rawimage;
    private ScreenType screenType;
    float fadeSpeed = 0.05f;
    float red, green, blue, alfa;

    void Start()
    {
        screenType = GetComponent<ScreenType>();

        red = rawimage.color.r;
        green = rawimage.color.g;
        blue = rawimage.color.b;
        alfa = rawimage.color.a;
    }

    void Update()
    {
        if (screenType.isFadeIn)
        {
            StartFadeIn();
        }
        if (screenType.isFadeOut)
        {
            StartFadeOut();
        }
    }

    void StartFadeOut()
    {
        alfa -= fadeSpeed;
        SetAlpha();
        if (alfa <= 0)
        {
            screenType.isFadeOut = false;
            screenType.screen.SetActive(false);
        }
    }

    void StartFadeIn()
    {
        screenType.screen.SetActive(true);
        alfa += fadeSpeed;
        SetAlpha();
        if (alfa >= 1)
        {
            screenType.isFadeIn = false;
        }
    }

    void SetAlpha()
    {
        rawimage.color = new Color(red, green, blue, alfa);
    }
}