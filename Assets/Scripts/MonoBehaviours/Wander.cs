using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class Wander : MonoBehaviour
{

    public float pursuitSpeed;
    public float wanderSpeed;
    float currentSpeed;

    public float directionChangeInterval;
    public bool followPlayer;
    Coroutine moveCoroutine;

    new Rigidbody2D rigidbody;
    Animator animator;
    Transform targetTransfrom;

    Vector3 endPosition;

    float currentAngle = 0f;

    CircleCollider2D circleCollider;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentSpeed = wanderSpeed;
        rigidbody = GetComponent<Rigidbody2D>();
        StartCoroutine(wanderRoutine());
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rigidbody.position, endPosition, Color.red);
    }

    public IEnumerator wanderRoutine()
    {
        while (true)
        {
            ChooseNewEndpoint();

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move(rigidbody, currentSpeed));

            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    private IEnumerator Move(Rigidbody2D rigidbodyToMove, float currentSpeed)
    {
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;

        while (remainingDistance > float.Epsilon)
        {
            if (targetTransfrom != null)
            {
                endPosition = targetTransfrom.position;
            }

            if (rigidbodyToMove != null)
            {
                animator.SetBool("isWalking", true);

                Vector3 newPosition = Vector3.MoveTowards(rigidbodyToMove.position, endPosition, currentSpeed * Time.deltaTime);

                rigidbody.MovePosition(newPosition);

                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }

            yield return new WaitForFixedUpdate();
        }

        animator.SetBool("isWalking", false);
    }

    private void ChooseNewEndpoint()
    {
        currentAngle += Random.Range(0, 360);
        currentAngle = Mathf.Repeat(currentAngle, 360);
        endPosition += Vector3FromAngle(currentAngle);
    }

    Vector3 Vector3FromAngle(float currentAngle)
    {
        float inputAngleRadians = currentAngle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(inputAngleRadians), 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && followPlayer)
        {
            currentSpeed = pursuitSpeed;

            targetTransfrom = collision.gameObject.transform;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move(rigidbody, currentSpeed));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isWalking", false);

            currentSpeed = wanderSpeed;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            targetTransfrom = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }
}
