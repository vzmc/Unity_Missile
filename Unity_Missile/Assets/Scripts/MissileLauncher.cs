using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField]
    private Missile missilePrefab;
    [SerializeField]
    private float launchInterval;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float randomPower;

    public void Launch()
    {
        var random = Random.insideUnitCircle.normalized * randomPower;
        var direction = (new Vector3(random.x, Mathf.Abs(random.y), -1.0f)).normalized;
        var missile = Instantiate(missilePrefab);
        missile.Launch(transform.position, transform.TransformDirection(direction), target);
    }

    private void FixedUpdate()
    {
        transform.LookAt(target);
    }

    private IEnumerator Start()
    {
        while(true)
        {
            Launch();
            yield return new WaitForSeconds(launchInterval);
        }
    }
}
