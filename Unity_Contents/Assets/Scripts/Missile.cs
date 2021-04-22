using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private GameObject body;
    [SerializeField]
    private ParticleSystem particle;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float angularSpeed;
    [SerializeField]
    private float minChackInterval;
    [SerializeField]
    private float maxChackInterval;

    private GameObject explosionObj;
    private Transform Target { get; set; }
    private Vector3 Velocity { get; set; }
    private Vector3 Axis { get; set; }

    public void SetTarget(Transform target)
    {
        Target = target;
    }

    private IEnumerator CheckTarget()
    {
        while (Target != null)
        {
            var forward = transform.forward;
            var toTarget = Target.position - transform.position;
            Axis = transform.InverseTransformDirection(Vector3.Cross(forward, toTarget).normalized);
            yield return new WaitForSeconds(Random.Range(minChackInterval, maxChackInterval));
        }
    }

    public void Launch(Vector3 position, Vector3 direction, Transform target)
    {
        SetTarget(target);
        transform.position = position;
        transform.LookAt(position + direction);
        Velocity = speed * Vector3.forward;

        StartCoroutine(CheckTarget());
    }

    private void FixedUpdate()
    {
        if(body.activeSelf)
        {
            transform.Rotate(Axis, angularSpeed * Time.deltaTime);
            transform.Translate(Velocity * Time.deltaTime, Space.Self);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.tag == "Player" || other.tag == "Missile")
        //{
            explosionObj = Instantiate(explosionPrefab, transform.position, transform.rotation);
            body.SetActive(false);
            StopAllCoroutines();
            Invoke("Dead", particle.main.startLifetime.constantMax);
        //}
    }

    private void Dead()
    {
        Destroy(explosionObj);
        Destroy(gameObject);
    }
}
