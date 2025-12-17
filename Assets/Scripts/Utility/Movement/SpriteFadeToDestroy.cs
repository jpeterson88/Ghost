using System.Collections;
using UnityEngine;

public class SpriteFadeToDestroy : MonoBehaviour
{
    public float fadeDuration = 2f, fadeInDuration = 1f;

    private SpriteRenderer spriteRenderer;
    private Coroutine fadeInCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)        
            Debug.LogError("SpriteFadeOut requires a SpriteRenderer component.");
    }

    private void Start()
    {
        // Start the fade-in effect when the object is spawned
        if (spriteRenderer != null)
        {
            fadeInCoroutine = StartCoroutine(FadeIn());
        }
    }

    /// <summary>
    /// Fades the sprite in from transparent to fully opaque.
    /// </summary>
    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;

        // Start fully transparent
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        while (elapsed < fadeInDuration)
        {
            // Calculate the new alpha value
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the sprite is fully opaque
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }

    public void Cleanup()
    {
        if (spriteRenderer != null)
        {
            StopCoroutine(fadeInCoroutine);
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            // Calculate the new alpha value
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the sprite is fully transparent
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Destroy the GameObject
        Destroy(gameObject);
    }
}