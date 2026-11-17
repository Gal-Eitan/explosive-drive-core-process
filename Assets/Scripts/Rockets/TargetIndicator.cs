
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TargetIndicator : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color trackingColor = Color.red;
    [SerializeField] private Color lockedColor = Color.yellow;

    [Header("Pulse")]
    [SerializeField] private float pulseSpeed = 5f;
    [SerializeField] private float pulseAmount = 0.15f;

    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    private bool isTracking = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (!isTracking)
            return;

        float pulse = 1f +
            Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;

        transform.localScale = originalScale * pulse;
    }

    public void SetTracking()
    {
        isTracking = true;
        spriteRenderer.color = trackingColor;
    }

    public void SetLocked()
    {
        isTracking = false;
        spriteRenderer.color = lockedColor;
        transform.localScale = originalScale;
    }
}
