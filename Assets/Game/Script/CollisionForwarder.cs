using UnityEngine;

public class CollisionForwarder : MonoBehaviour
{
    public MonsterBase parentScript;

    private void OnTriggerEnter(Collider other)
    {
        parentScript.OnChildTriggerEnter(other, this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        parentScript.OnChildCollisionEnter(collision, this.gameObject);
    }
}