using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    private const float SPEED = 5f;
    private readonly Color HEALTHY_COLOR = Color.blue;
    private readonly Color INFECTED_COLOR = Color.magenta;
    private readonly Color INFECTIOUS_COLOR = Color.red;
    private readonly Color IMMUNE_COLOR = Color.yellow;

    private Vector2 targetPosition;
    private float speed = SPEED;
    public SpriteRenderer sprite;
    public int targetIndex;
    public int index;
    public int state;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed);
        if (targetPosition.x == transform.position.x && targetPosition.y == transform.position.y)
        {
            this.enabled = false;
        }
    }

    public void SetState(int state)
    {
        switch (state)
        {
            default:
            case 0:
                sprite.color = HEALTHY_COLOR;
                break;
            case 1:
                sprite.color = IMMUNE_COLOR;
                break;
            case 2:
                sprite.color = INFECTED_COLOR;
                break;
            case 3:
                sprite.color = INFECTIOUS_COLOR;
                break;
        }
        this.state = state;
    }

    /// <summary>
    /// Modifie le site cible de l'individu.
    /// </summary>
    /// <param name="target">Nouveau site cible.</param>
    public void SetTarget(Vector2 positionMin, Vector2 positionMax, int targetIndex)
    {
        this.targetIndex = targetIndex;
        this.enabled = true;
        targetPosition = FindPlaceInSite(positionMin, positionMax);
        speed = SPEED;
    }

    /// <summary>
    /// Choisis un point aléatoire dans le lieu actuelle et le choisis comme nouvelle cible.
    /// </summary>
    public Vector2 FindPlaceInSite(Vector2 positionMin, Vector2 positionMax)
    {
        Vector2 newTarget = new Vector2(
                Random.Range(positionMin.x, positionMax.x), 
                Random.Range(-positionMin.y, -positionMax.y)
            );

        return newTarget;
    }
}
