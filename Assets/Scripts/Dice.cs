
using System.Collections;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    [Header("Dice Settings")]
    public bool rollOnStart = false;
    public bool showDebug = true;

    [Header("Roll Forces")]
    public float upwardForce = 5f;
    public float forwardForce = 2f;
    public float torqueForce = 15f;
    public float randomForce = 1f;

    [Header("Roll Time")]
    public float minimumRollTime = 3f;

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

        rollCoroutine = StartCoroutine(RollRoutine());
    }
    

    private IEnumerator RollRoutine()
    {
        rolling = true;
        currentNumber = 0;

        PrepareDice();
        ThrowDice();

        if (showDebug)
        {
            Debug.Log("Rolling dice");
        }

        yield return new WaitForSeconds(minimumRollTime);
        currentNumber = GetTopFaceNumber();

        rolling = false;
        rollCoroutine = null;

        if (showDebug)
        {
            Debug.Log("Roll timer ended");
        }
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

        Vector3 direction = new Vector3(
            randomDirection.x,
            0f,
            randomDirection.y
        );

        Vector3 force = Vector3.up * upwardForce;
        force += direction * forwardForce;
        force += Random.insideUnitSphere * randomForce;

        rb.AddForce(force, ForceMode.Impulse);

        Vector3 spin = Random.onUnitSphere * torqueForce;

        rb.AddTorque(spin, ForceMode.Impulse);
    }

    private void PrepareDice()
    {
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
