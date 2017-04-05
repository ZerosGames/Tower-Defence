using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    Node CurrentNode;
    Node Next;

    float MaxSpeed = 3f;
    float MaxForce = 200f;
    Vector3 acceleration;
    Vector3 Velocity;

    void Start ()
    {
        CurrentNode = null;
    } 
	
	void Update ()
    {
        if (!GameManager.gameManager.GetPause())
        {
            CurrentNode = VectorFieldGrid.nodeGrid.NodeFromWorldPos(transform.position);

            if (CurrentNode.WorldPos.x < 1 && CurrentNode.WorldPos.x > -1 && CurrentNode.WorldPos.z < 1 && CurrentNode.WorldPos.z > -1)
            {
                GetComponent<EnemyController>().Destory(false);
                return;
            }

            Steer(CurrentNode);

            Velocity += acceleration;
            Vector3.ClampMagnitude(Velocity, MaxSpeed);

            transform.Translate(Velocity * Time.deltaTime);

            acceleration = Vector3.zero;
        }
    }

    void ApplyForce(Vector3 _force)
    {
        acceleration += _force;
    }

    void Steer(Node node)
    {
        Vector3 desired = node.GetDirectionVector();

        desired *= MaxSpeed;

        Vector3 steer = desired - Velocity;

        Vector3.ClampMagnitude(steer, MaxForce);

        ApplyForce(steer * Time.deltaTime);
    }
}
