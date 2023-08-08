using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpell : BasicSpell
{
    //!!!! set default Layer !!!!!!
    public LayerMask collisionLayer;

    [SerializeField] private ParticleSystem trailParticle;
    [SerializeField] private ParticleSystem explodeParticle;
    [SerializeField] private ParticleSystem boltParticle;
    [SerializeField] private Vector3 movingAxis;
    private Vector3 moveWorldAxis; 
    private float spellSpeed = 4.5f;
    private float collisionRadius = 0.2f;
    private bool exploded;


    public override void Initialize(Transform wandTip)
    {
        base.Initialize(wandTip);
        //translate positions
        moveWorldAxis = wandTip.TransformDirection(movingAxis);
    }

    void Update()
    {
        //straight line
        transform.position += Time.deltaTime * moveWorldAxis * spellSpeed;
    }

    private void FixedUpdate()
    {
        if (!exploded)
        {
            //checking colliders
            Collider[] results = Physics.OverlapSphere(transform.position, collisionRadius);
            if (results.Length > 0)
            {
                Exploding();
            }
        }
       
    }

    private void Exploding()
    {
        exploded = true;
        explodeParticle.Play();
        trailParticle.Stop();
        boltParticle.Stop();

        Destroy(gameObject,1f);

    }

}
