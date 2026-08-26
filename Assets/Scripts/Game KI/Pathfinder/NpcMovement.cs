using System.Collections.Generic;
using UnityEngine;


public class NpcMovement : MonoBehaviour
{
    private Pathfinding pathfinding;
    public GameObject[] patrolPoints;
    private int currentPatrolTarget = 0;

    private int startX ;
    private int startZ ;

    private int targetX ;
    private int targetZ ;

    public float speed = 3f;

    public List<PathNode> path;
    private int currentIndex = 0;

    private void Start()
    {
        pathfinding = GridManagerPathfinder.Instance.GetPathfinding();

        transform.position = patrolPoints[0].transform.position;

        CalculatePathToNextPoint();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))//tag nacht rhythmus
        {
            pathfinding.UpdateWalkableNodes();
            CalculatePath();
        }

        MoveAlongPath();
        
    }

    public void CalculatePath()
    {
        path = pathfinding.Findpath(startX, startZ, targetX, targetZ);
        currentIndex = 0;
    }

    public void MoveAlongPath()
    {
        if (path == null || path.Count == 0)
            return;

        if (currentIndex >= path.Count)
            return;

        Vector3 targetPos = GridToWorld(path[currentIndex].x, path[currentIndex].z);

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            currentIndex++;

            if (currentIndex >= path.Count)
            {
                currentPatrolTarget++;

                if (currentPatrolTarget >= patrolPoints.Length)
                    currentPatrolTarget = 0;

                CalculatePathToNextPoint();
            }
        }
       
    }

    private void OnDrawGizmosSelected()
    {

        if (patrolPoints == null || patrolPoints.Length < 2)
            return;

        Gizmos.color = Color.green;

        for (int i = 0; i < patrolPoints.Length - 1; i++)
        {
            if (patrolPoints[i] == null || patrolPoints[i + 1] == null)
                continue;

            Gizmos.DrawLine(
                patrolPoints[i].transform.position,
                patrolPoints[i + 1].transform.position
            );
            
        }
        foreach (GameObject point in patrolPoints)
        {
            Gizmos.DrawCube(point.transform.position,new Vector3(2,2,2));
        }
    }
    
    private void OnDrawGizmos()
    {
        if (path == null || path.Count < 2)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < path.Count - 1; i++)
        {
            if (path[i] == null || path[i + 1] == null)
                continue;

            Gizmos.DrawLine(
                GridToWorld(path[i].x, path[i].z),
                GridToWorld(path[i + 1].x, path[i + 1].z)
            );
        }
    }

    public Vector3 GridToWorld(int x, int z)
    {
        return new Vector3(x * 10f + 5f, transform.position.y, z * 10f + 5f);
    }
    public void CalculatePathToNextPoint()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = patrolPoints[currentPatrolTarget].transform.position;

        startX = Mathf.RoundToInt(startPos.x / 10f);
        startZ = Mathf.RoundToInt(startPos.z / 10f);

        targetX = Mathf.RoundToInt(endPos.x / 10f);
        targetZ = Mathf.RoundToInt(endPos.z / 10f);

        path = pathfinding.Findpath(startX, startZ, targetX, targetZ);

        currentIndex = 0;
    }
}