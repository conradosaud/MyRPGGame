using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    public float gravity = Constants.gravity;
    public float targetOffsetDistance = 2f;

    public float minRestTime = 3f;
    public float maxRestTime = 6f;
    float restTime;

    Vector3 moveDirection;

    bool canRound = true;
    bool reachDestiny = true;
    bool moveToDestiny = false;

    float restTimeElapsed = 0;

    CharacterController cc;
    CharacterStatus characterStatus;
    EnemyAI enemyAI;

    Vector3 destiny;
    Bounds patrolAreaBounds;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        characterStatus = GetComponent<CharacterStatus>();
        enemyAI = GetComponent<EnemyAI>();

        patrolAreaBounds = transform.Find("PatrolArea").GetComponent<Renderer>().bounds;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( canRound )
        {
            if( reachDestiny == true )
            {
                reachDestiny = false;

                restTime = Random.Range(minRestTime, maxRestTime);

                Vector3 randomPointInPatrolArea = new Vector3(
                    Random.Range(patrolAreaBounds.min.x, patrolAreaBounds.max.x),
                    patrolAreaBounds.center.y, // keep original height
                    Random.Range(patrolAreaBounds.min.z, patrolAreaBounds.max.z)
                );
                destiny = randomPointInPatrolArea;
                Invoke("EnableMoveToDestiny", restTime);
            }

            if ( moveToDestiny == true)
            {
                Move(destiny);
            }

            if (Vector3.Distance(transform.position, destiny) < 0.9f)
            {
                moveToDestiny = false;
                reachDestiny = true;
            }

        }

    }

    void Move(Vector3 moveDirection)
    {

        if( enemyAI.target != null)
        {
            float offsetDistance = Vector3.Distance(transform.position, enemyAI.target.position);
            if (offsetDistance < targetOffsetDistance)
                return;
        }

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
            moveDirection = (moveDirection - transform.position).normalized;
        }

        // Apply gravity
        moveDirection.y -= gravity;
        // Apply velocity to the axis
        moveDirection.x *= characterStatus.moveSpeed;
        moveDirection.z *= characterStatus.moveSpeed;

        // Move towards the target
        cc.Move(moveDirection * Time.deltaTime);
    }

    void EnableMoveToDestiny()
    {
        moveToDestiny = true;
    }

}
