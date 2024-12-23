using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Image fadeOutUIImage;
    [SerializeField] private float fadeTime;
    public enum FadeDirection
    {
        In, Out, etc
    }

    private void Start()
    {
        fadeOutUIImage = GetComponent<Image>();
    }

    public IEnumerator Fade(FadeDirection _fadeDirection, float t)
    {
        yield return new WaitForSeconds(t);

        float _alpha = _fadeDirection == FadeDirection.Out ? 1 : 0;
        float _fadeEndValue = _fadeDirection == FadeDirection.Out ? 0 : 1;

        if (_fadeDirection == FadeDirection.Out)
        {
            while (_alpha >= _fadeEndValue)
            {
                SetColorImage(ref _alpha, _fadeDirection);

                yield return null;
            }

            fadeOutUIImage.enabled = false;
        }
        else
        {
            fadeOutUIImage.enabled = true;

            while (_alpha <= _fadeEndValue)
            {
                SetColorImage(ref _alpha, _fadeDirection);

                yield return null;
            }
        }
    }

    public IEnumerator FadeAndLoadScene(FadeDirection _fadeDirection, string _sceneToLoad)
    {
        fadeOutUIImage.enabled = true;

        yield return Fade(_fadeDirection, 0f);
        SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive);
    }

    void SetColorImage(ref float _alpha, FadeDirection _fadeDirection)
    {
        fadeOutUIImage.color = new Color(fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, _alpha);

        _alpha += 2 * Time.deltaTime * (1 / fadeTime) * (_fadeDirection == FadeDirection.Out ? -1 : 1);
    }
}
