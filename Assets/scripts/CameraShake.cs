using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeAmount = 0.02f;

    private Vector3 initialPos;
    private float shakeTimer = 0f;

    public static CameraShake Instance;

    void Awake()
    {
        Instance = this;
        initialPos = transform.localPosition;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            transform.localPosition = initialPos + Random.insideUnitSphere * shakeAmount;
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = initialPos;
        }
    }

    public void Shake(float duration)
    {
        shakeTimer = duration;
    }
}
