using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    [SerializeField] private Image image;
    private float alpha;
    [HideInInspector] public bool CanFadeToBlack;
    [HideInInspector] public bool CanFadeToNormal;

    void Start()
    {
        alpha = image.color.a;
    }

    void Update()
    {
        FadeInAndOut();
    }

    public void FadeInAndOut()
    {
        if (alpha < 1)
        {
            alpha += 0.01f;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }
        else if (alpha == 0)
        {
            SceneManager.LoadScene(1);
        }

        if (alpha > 0)
        {
            alpha -= 0.01f;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }
    }
}
