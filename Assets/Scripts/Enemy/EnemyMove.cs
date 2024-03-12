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


    bool isPatrolling = true;
    public bool followSelectedTarget = false;

    bool reachDestiny = true;
    bool moveToDestiny = false;

    CharacterController cc;
    CharacterStatus characterStatus;
    EnemyAI enemyAI;
    EnemyCombat enemyCombat;
    Transform player;

    Vector3 destiny;
    Vector3 moveDirection;
    Bounds patrolAreaBounds;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        characterStatus = GetComponent<CharacterStatus>();
        enemyAI = GetComponent<EnemyAI>();
        enemyCombat = GetComponent<EnemyCombat>();
        player = GameObject.FindWithTag("Player").transform;

        patrolAreaBounds = transform.Find("PatrolArea").GetComponent<Renderer>().bounds;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(enemyCombat.isFighting)
        {
            Debug.Log("Esta lutando");
            if (followSelectedTarget)
            {
                Debug.Log("estáppesrguindo");
                Move(enemyCombat.target.position);
            }

            return;

        }

        if( isPatrolling )
        {
            Patrol();
        }

    }


    void Patrol()
    {
        if (reachDestiny == true)
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

        if (moveToDestiny == true)
        {
            Move(destiny);
        }

        if (Vector3.Distance(transform.position, destiny) < 0.9f)
        {
            moveToDestiny = false;
            reachDestiny = true;
        }

    }

    void Move(Vector3 moveDirection)
    {

        if(enemyCombat.target != null)
        {
            float offsetDistance = Vector3.Distance(transform.position, enemyCombat.target.position);
            if (offsetDistance < targetOffsetDistance)
            {
                Debug.Log("é menor porra");
                return;
            }
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
