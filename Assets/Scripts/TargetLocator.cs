using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] private Transform weapon = null;
    [SerializeField] private Transform target = null;
    [SerializeField] private ParticleSystem projectileParticles = null;
    [SerializeField] private float range = 15.0f;

    private void Update()
    {
        FindClosestTarget();
        AimWeapon();
    }

    private void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float minDistance = float.MaxValue;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                closestTarget = enemy.transform;
                minDistance = distance;
            }
        }

        target = closestTarget;
    }

    private void AimWeapon()
    {
        if (target == null) { return; }

        weapon.LookAt(target);
        float targetDistance = Vector3.Distance(transform.position, target.position);

        if (targetDistance <= range)
        {
            Attack(true);
        }
        else
        {
            Attack(false);
        }
    }

    private void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
}
