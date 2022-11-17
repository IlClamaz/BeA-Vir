using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SlideScript : MonoBehaviour
{
    //All _slides in the project, minus the _fadeSlide
    [SerializeField] private List<Transform> _slides = default;

    //The panel that overlays all _slides and changes from clear to black
    [SerializeField] private Image _fadeSlide = default;

    [Header("Config Values")]
    [SerializeField, Tooltip("The duration (in seconds) over which the fade slide will fade in / out")]
    private float _fadeDuration = 0.75f;
    [Header("Config Values")]
    [SerializeField, Tooltip("The duration (in seconds) over switch silde")]
    private float _switchDuration = 1.6f;
    //The slide we're currently viewing
    private int _currentSlide = -1;

    private IEnumerator Start()
    {
        print("SCREEN");
        _fadeSlide.color = Color.black;
        print("ciaoSli");
        while (true)
        {
            print("ciao");
            _currentSlide++;
            _currentSlide = _currentSlide % _slides.Count;

            // Transition to the next slide
            StartCoroutine(SlideTransition());
            yield return new WaitForSeconds(_switchDuration);
        }
    }

    private IEnumerator SlideTransition()
    {
        // Fade to black
        yield return StartCoroutine(FadeToTargetColor(targetColor: Color.black));

        // Set only our current slide active - and all others inactive
        _slides.ForEach(slide => slide.gameObject.SetActive(_slides.IndexOf(slide) == _currentSlide));

        // Fade to clear
        yield return StartCoroutine(FadeToTargetColor(targetColor: Color.clear));
    }

    private IEnumerator FadeToTargetColor(Color targetColor)
    {
        // The total amount of seconds that has elapsed since the start of our lerp sequence
        float elapsedTime = 0.0f;

        // The color of our fade panel at the start of the lerp sequence
        Color startColor = _fadeSlide.color;

        // While we haven't reached the end of the lerp sequence..
        while (elapsedTime < _fadeDuration)
        {
            // Increase our elapsed time
            elapsedTime += Time.deltaTime;

            // Perform a lerp to our target color
            _fadeSlide.color = Color.Lerp(startColor, targetColor, elapsedTime / _fadeDuration);

            // Wait for the next frame
            yield return null;
        }
    }

    private void OnDisable()
    {
        print("DISABLE");
        foreach (var slide in _slides) slide.gameObject.SetActive(false);
        StopAllCoroutines();
    }
}