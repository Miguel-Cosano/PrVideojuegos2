using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlataformaBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Transform> targetPoints;
    public int currentTargetIndex = 0;
    private bool reachedTarget = false;
    public float speed;
    //public NavMeshAgent miAgente;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()

        {
            if (!reachedTarget)
            {
                Vector3 currentPosition = transform.position;
                Vector3 targetPosition = targetPoints[currentTargetIndex].position;

                // Calcular la dirección en la que se debe mover la plataforma
                Vector3 direction = (targetPosition - currentPosition).normalized;

                // Calcular la distancia que se debe mover
                float distance = Vector3.Distance(currentPosition, targetPosition);

                // Mover la plataforma en la dirección calculada
                transform.Translate(direction * speed * Time.deltaTime);
                // Si se ha alcanzado el punto objetivo, pasar al siguiente punto
                if (distance < 0.4f)
                {
                    currentTargetIndex++;

                    if (currentTargetIndex >= targetPoints.Count)
                    {
                        currentTargetIndex = 0;
                    }
                }
            }
        }
    }
