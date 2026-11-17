
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{
    [Header("Orientation")]
    [SerializeField] private Vector2 localForward = Vector2.up;

    [Header("Engine")]
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float reverseAcceleration = 12f;
    [SerializeField] private float maxSpeed = 15f;

    [Header("Steering")]
    [SerializeField] private float steeringTorque = 12f;
    [SerializeField] private float lateralGrip = 8f;
    [SerializeField] private float minSteeringSpeed = 0.5f;

    [SerializeField] private float maxAngularSpeed = 100f;

    private Rigidbody2D rb;
    private float throttle;
    private float steering;

    // Car's forward direction in world space.
    private Vector2 Forward =>
        transform.TransformDirection(
            (Vector3)localForward.normalized
        );

    // Perpendicular to the car's forward direction.
    private Vector2 Right =>
        new Vector2(Forward.y, -Forward.x);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
        {
            throttle = 0f;
            steering = 0f;
            return;
        }

        throttle = 0f;
        steering = 0f;

        if (keyboard.wKey.isPressed)
            throttle += 1f;

        if (keyboard.sKey.isPressed)
            throttle -= 1f;

        if (keyboard.aKey.isPressed)
            steering += 1f;

        if (keyboard.dKey.isPressed)
            steering -= 1f;
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();
        ApplySteering();
        ApplyLateralFriction();
    }

    private void ApplyEngineForce()
    {
        float forwardSpeed =
            Vector2.Dot(rb.linearVelocity, Forward);

        if (throttle > 0f && forwardSpeed >= maxSpeed)
            return;

        if (throttle < 0f && forwardSpeed <= -maxSpeed * 0.5f)
            return;

        float engineForce = throttle > 0f
            ? acceleration
            : reverseAcceleration;

        rb.AddForce(
            Forward * throttle * engineForce,
            ForceMode2D.Force
        );
    }

    private void ApplySteering()
{
    float forwardSpeed =
        Vector2.Dot(rb.linearVelocity, Forward);

    if (Mathf.Abs(forwardSpeed) < minSteeringSpeed)
        return;

    float reverseMultiplier = Mathf.Sign(forwardSpeed);

    rb.AddTorque(
        steering * reverseMultiplier * steeringTorque,
        ForceMode2D.Force
    );

    // Limit rotation speed.
    rb.angularVelocity = Mathf.Clamp(
        rb.angularVelocity,
        -maxAngularSpeed,
        maxAngularSpeed
    );
}

    private void ApplyLateralFriction()
    {
        // Calculate sideways velocity relative to the car.
        float lateralSpeed =
            Vector2.Dot(rb.linearVelocity, Right);

        Vector2 lateralVelocity =
            Right * lateralSpeed;

        // Counteract sideways sliding.
        rb.AddForce(
            -lateralVelocity * lateralGrip * rb.mass,
            ForceMode2D.Force
        );
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 forward = transform.TransformDirection(
            (Vector3)localForward.normalized
        );

        Gizmos.color = Color.green;

        Gizmos.DrawRay(
            transform.position,
            forward * 3f
        );
    }
}
