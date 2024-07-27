using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject Whole;
    public GameObject Sliced;
    private Collider fruitCollider;
    private Rigidbody fruitRigibody;
    private ParticleSystem juiceParticleEffect;
    private bool hasSliced = false;


    private void Awake()
    {
        fruitCollider = GetComponent<Collider>();
        fruitRigibody = GetComponent<Rigidbody>();
        juiceParticleEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void Slice(Vector3 direction,Vector3 position,float force)
    {
        Whole.SetActive(false);
        Sliced.SetActive(true);

        fruitCollider.enabled = true;
        juiceParticleEffect.Play();

        // ¼ÆËãÇÐËéµÄ½Ç¶È
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody[] slices = Sliced.GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody slice in slices)
        {
            slice.velocity = fruitRigibody.velocity;
            slice.AddForceAtPosition(direction * force, position,ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSliced)
        {
            FindObjectOfType<GameManager>().IncreaseScore();
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.direction,blade.transform.position,blade.sliceForce);
            hasSliced = true;
        }
    }
}
