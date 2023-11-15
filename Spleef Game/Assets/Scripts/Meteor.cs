using Unity.VisualScripting;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public delegate void GameObjectDelegate(GameObject gameObject);

    public GameObjectDelegate OnCollision;

    public float speed = 10f;

    private GameObject target;

    private void Update()
    {
        if (target != null)
        {
            //This can probably be refined to feel less linear
            transform.position += Vector3.Normalize(target.transform.position - transform.position) * Time.deltaTime * speed;
        }
    }

    private void PerformImpact()
    {
        if (OnCollision != null)
        {
            OnCollision(gameObject);
        }
        target.transform.parent.GetComponent<PlatformBase>().KillTile(target);
        target = null;
        gameObject.SetActive(false);
    }

    public void Spawn(GameObject target, Vector3 spawnPoint)
    {
        gameObject.SetActive(true);
        transform.position = spawnPoint;
        this.target = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            PerformImpact();
        }
    }
}