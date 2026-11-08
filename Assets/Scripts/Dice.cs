
using System.Collections;

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    [Header("Dice Settings")]
    [Range(1, 6)] public int targetNumber = 6;
    public bool rollOnStart = false;
    public bool randomTarget = false;
    public bool showDebug = true;

    [Header("Roll Forces")]
    public float upwardForce = 5f;
    public float forwardForce = 2f;
    public float torqueForce = 15f;
    public float randomForce = 1f;

    [Header("Roll Time")]
    public float minimumRollTime = 1.5f;
    public float maximumRollTime = 5f;
    public float guidanceStartTime = 0.8f;

    [Header("Guidance")]
    public float guidanceStrength = 18f;
    public float guidanceDamping = 5f;
    public float aligmentTolerance = 5f;
    public float recoveryBonceSpeed = 2f;
    public int maxRecoveryBounces = 2;

    [Header("Stopping")]
    public float stopVelocity = 0.15f;
    public float stopAngularVelocity = 0.15f;
    public float stopCheckDuration = 0.4f;

    [Header("Face Directions")]
    public Vector3 face1 = Vector3.up;
    public Vector3 face2 = Vector3.right;
    public Vector3 face3 = Vector3.forward;
    public Vector3 face4 = Vector3.back;
    public Vector3 face5 = Vector3.left;
    public Vector3 face6 = Vector3.down;

    [Header("Current State")]
    public bool rolling = false;
    public int currentNumber = 0;

    private Rigidbody rb;
    private Coroutine rollCoroutine;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float lastGroundTime = -1000f;

    private Vector3 GetVelocity()
    {
        return rb.linearVelocity;
    }
    private void SetVelocity(Vector3 velocity)
    {
        rb.linearVelocity = velocity;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.isKinematic = false;

        startPosition = rb.position;
        startRotation = rb.rotation;
    }

    private void Start()
    {
        if (rollOnStart)
        {
            RollDice();
        }
    }

    private void OnDisable()
    {
        CancelRoll();
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckGround(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        CheckGround(collision);
    }

    private void CheckGround(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;

            if (Vector3.Dot(normal, Vector3.up) > 0.5f)
            {
                lastGroundTime = Time.fixedTime;
                return;
            }
        }
    }

    private bool IsGrounded()
    {
        float timeSinceGround = Time.fixedTime - lastGroundTime;

        if (timeSinceGround <= Time.fixedDeltaTime * 1.5f)
        {
            return true;
        }

        return false;
    }

    public void RollDice()
    {
        if (rolling)
        {
            return;
        }       

        if (!isActiveAndEnabled)
        {
            return;
        }

        if (randomTarget)
        {
            targetNumber = Random.Range(1, 7);
        }

        targetNumber = Mathf.Clamp(targetNumber, 1, 6);

        rollCoroutine = StartCoroutine(RollRoutine(targetNumber));
    }

    public void RollToNumber(int number)
    {
        if (rolling || !isActiveAndEnabled)
        {
            return;
        }

        SetTargetNumber(number);    
        RollDice();
    }

    public void SetTargetNumber(int number)
    {
        targetNumber = Mathf.Clamp(number, 1, 6);
        randomTarget = false;
    }
     
    private IEnumerator RollRoutine(int target)
    {
        rolling = true;
        currentNumber = 0;

        PrepareDice();
        ThrowDice();

        if (showDebug)
        {
            Debug.Log("Rolling Towards " + target);
        }

        float rollTime = 0f;
        float stoppedTime = 0f;
        int bounces = 0;

        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while (true)
        {
            yield return wait;

            float step = Time.fixedDeltaTime;

            rollTime += step;

            bool grounded = IsGrounded();
            bool sleeping = rb.IsSleeping();
            
            bool movingSlow = GetVelocity().magnitude <= stopVelocity;
            bool spinningSlow = rb.angularVelocity.magnitude <= stopAngularVelocity;

            bool stopped = sleeping;

            if (grounded && movingSlow && spinningSlow)
            {
                stopped = true;
            }

            if (stopped)
            {
                stoppedTime += step;
            }
            else
            {
                stoppedTime = 0f;
            }

            bool firstRoll = bounces == 0;
            float minTime = 0f;

            if (firstRoll)
            {
                minTime = minimumRollTime;
            }

            bool finished = rollTime >= minTime && stoppedTime >= stopCheckDuration;

            if (finished)
            {
                int topNumber = GetTopFaceNumber();

                Vector3 faceDir = GetWorldFaceDirection(target);
                float angle = Vector3.Angle(faceDir, Vector3.up);

                if (topNumber == target && angle <= aligmentTolerance)
                {
                    currentNumber = target;
                    break;
                }
            }
            
            if (finished || rollTime >= maximumRollTime)
            {
                if (!grounded && !sleeping)
                {
                    break;
                }

                if (bounces >= maxRecoveryBounces)
                {
                    break;
                }

                if (recoveryBonceSpeed <= 0f)
                {
                    break;
                }
                
                bounces ++;

                rollTime = 0f;
                stoppedTime = 0f;
                lastGroundTime = -1000f;
                
                rb.WakeUp();
                rb.AddForce(Vector3.up * recoveryBonceSpeed, ForceMode.VelocityChange);
                
                continue;
            }

            float delay = 0f;

            if (firstRoll)
            {
                delay = guidanceStartTime;
            }

            if (stopped || rollTime < delay)
            {
                continue;
            }

            float strength = 1f;

            if (firstRoll)
            {
                float amount = (rollTime - delay) / 0.5f;
                strength = Mathf.SmoothStep(0f, 1f, amount);
            }
            GuideDice(target, strength);
        }

        rolling = false;
        rollCoroutine = null;

        if (showDebug)
        {
            if (currentNumber == target)
            {
                Debug.Log("Dice landed on " + currentNumber);
            }
            else
            {
                Debug.LogWarning("Dice could not land on " + target);
            }
        }
    }

    private void GuideDice(int target, float strength)
    {
        Vector3 faceDir = GetWorldFaceDirection(target); 
        float angle = Vector3.Angle(faceDir, Vector3.up);

        Vector3 correction = Vector3.zero;

        if (angle > aligmentTolerance)
        {
            Vector3 axis = Vector3.Cross(faceDir, Vector3.up);

            if (axis.magnitude < 0.001f)
            {
                axis = Vector3.ProjectOnPlane(rb.rotation * Vector3.right, Vector3.up);

                if (axis.magnitude < 0.001f)
                {
                    axis = Vector3.ProjectOnPlane(rb.rotation * Vector3.forward, Vector3.up);
                }
            }
            axis.Normalize();
            
            correction = axis * angle * Mathf.Deg2Rad * guidanceStrength;
        }

        Vector3 damping = -rb.angularVelocity * guidanceDamping;
        Vector3 finalTorque = correction + damping;
        
        rb.AddTorque(finalTorque * strength, ForceMode.Acceleration);
    }

    public int GetTopFaceNumber()
    {
        int topNumber = 1;
        float bestDot = -1000f;

        for (int i = 1; i <= 6; i++)
        {
            Vector3 direction = GetWorldFaceDirection(i);

            float dot = Vector3.Dot(direction, Vector3.up);

            if (dot > bestDot)
            {
                bestDot = dot;
                topNumber = i;
            }
        }

        return topNumber;
    }

    private Vector3 GetWorldFaceDirection(int number)
    {
        Vector3 localDirection = GetFaceDirection(number);

        return rb.rotation * localDirection;
    }

    private Vector3 GetFaceDirection(int number)
    {
        if (number == 1) 
            return face1.normalized;
        if (number == 2) 
            return face2.normalized;
        if (number == 3) 
            return face3.normalized;
        if (number == 4) 
            return face4.normalized;
        if (number == 5) 
            return face5.normalized;
        if (number == 6) 
            return face6.normalized;

        return Vector3.up;
    }

    public void ResetDice()
    {
        CancelRoll();

        if (rb == null)
        {
            return;
        }

        rb.isKinematic = false;

        rb.position = startPosition;
        rb.rotation = startRotation;

        SetVelocity(Vector3.zero);
        rb.angularVelocity = Vector3.zero;

        rb.Sleep();
    }

    private void ThrowDice()
    {
        Vector2 randomDirection = Random.insideUnitCircle;

        if (randomDirection == Vector2.zero)
        {
            randomDirection = Vector2.right;
        }

        randomDirection.Normalize();

        Vector3 direction = new Vector3(randomDirection.x, 0f, randomDirection.y);

        Vector3 force = Vector3.up * upwardForce;
        force += direction * forwardForce;
        force += Random.insideUnitSphere * randomForce;

        rb.AddForce(force, ForceMode.Impulse);

        Vector3 spin = Random.onUnitSphere * torqueForce;

        rb.AddTorque(spin, ForceMode.Impulse);
    }

    private void PrepareDice()
    {
        lastGroundTime = -1000f;

        rb.isKinematic = false;
        rb.useGravity = true;

        SetVelocity(Vector3.zero);
        rb.angularVelocity = Vector3.zero;

        rb.WakeUp();
    }

    private void CancelRoll()
    {
        if (rollCoroutine != null)
        {
            StopCoroutine(rollCoroutine);
        }

        rollCoroutine = null;
        rolling = false;
    }

    public bool IsRolling()
    {
        return rolling;
    }

    public int GetCurrentNumber()
    {
        return currentNumber;
    }
}
