using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    private float minCheckInterval;
    [SerializeField]
    private float maxCheckInterval;

    private GameObject explosionObj;
    private Vector3 Velocity { get; set; }
    private Vector3 Axis { get; set; }

    private Transform TargetTransform { get; set; }

    private Transform SelfTransform { get; set; }

    public void SetTarget(Transform target)
    {
        TargetTransform = target;
        SelfTransform = transform;
    }

    private IEnumerator CheckTarget()
    {
        while (TargetTransform)
        {
            var forward = SelfTransform.forward;
            var toTarget = TargetTransform.position - SelfTransform.position;
            Axis = SelfTransform.InverseTransformDirection(Vector3.Cross(forward, toTarget).normalized);
            yield return new WaitForSeconds(Random.Range(minCheckInterval, maxCheckInterval));
        }
    }

    public void Launch(Vector3 position, Vector3 direction, Transform target)
    {
        SetTarget(target);
        SelfTransform.position = position;
        SelfTransform.LookAt(position + direction);
        Velocity = speed * Vector3.forward;

        StartCoroutine(CheckTarget());
    }

    private void FixedUpdate()
    {
        if (!body.activeSelf) 
            return;
        
        SelfTransform.Rotate(Axis, angularSpeed * Time.deltaTime);
        SelfTransform.Translate(Velocity * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Missile")) 
            return;
        
        explosionObj = Instantiate(explosionPrefab, SelfTransform);
        body.SetActive(false);
        StopAllCoroutines();
        Invoke(nameof(Dead), particle.main.startLifetime.constantMax);
    }

    private void Dead()
    {
        Destroy(gameObject);
    }
}
