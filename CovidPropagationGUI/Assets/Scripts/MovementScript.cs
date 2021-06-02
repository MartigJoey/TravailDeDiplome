using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    private Transform target;
    private MeshRenderer meshRenderer;

    private Material activeMaterial;
    public Material healthyMaterial;
    public Material immuneMaterial;
    public Material infectedMaterial;
    public Material infectiousMaterial;

    private Vector3 targetPosition;
    private float speed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);

            if (targetPosition == transform.position)
            {
                speed = 0.001f;
                MoveInSite();
            }
        }
    }

    public void SetState(int state)
    {
        switch (state)
        {
            default:
            case 0:
                activeMaterial = healthyMaterial;
                break;
            case 1:
                activeMaterial = immuneMaterial;
                break;
            case 2:
                activeMaterial = infectedMaterial;
                break;
            case 3:
                activeMaterial = infectiousMaterial;
                break;
        }

        meshRenderer ??= GetComponent<MeshRenderer>();
        Debug.Log(activeMaterial.name);
        meshRenderer.material = activeMaterial;
    }

    /// <summary>
    /// Modifie le site cible de l'individu.
    /// </summary>
    /// <param name="target">Nouveau site cible.</param>
    public void SetTarget(Transform target)
    {
        this.target = target;
        targetPosition = target.position;
        speed = 0.01f;
    }

    /// <summary>
    /// Choisis un point aléatoire dans le lieu actuelle et le choisis comme nouvelle cible.
    /// </summary>
    public void MoveInSite()
    {
        Vector3 newTarget;

        float maxX = target.position.x + (target.localScale.x / 2) - (transform.localScale.x / 2);
        float maxZ = target.position.z + (target.localScale.z / 2) - (transform.localScale.z / 2);
        float minX = target.position.x - (target.localScale.x / 2) + (transform.localScale.x / 2);
        float minZ = target.position.z - (target.localScale.z / 2) + (transform.localScale.z / 2);

        float newX = Random.Range(minX*100, maxX*100)/100;
        float newZ = Random.Range(minZ*100, maxZ*100)/100;

        newTarget = new Vector3(newX, 0, newZ);

        targetPosition = newTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target != null && other.transform.name == target.name)
        {
            MoveInSite();
        }
    }
}
