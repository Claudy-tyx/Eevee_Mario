using System.Collections;
using UnityEngine;

public class LeafLifetime : MonoBehaviour
{
    private LeafSpawner spawner;
    private bool reported;

    public void Setup(
        LeafSpawner leafSpawner,
        float lifetime
    )
    {
        spawner = leafSpawner;
        StartCoroutine(Lifetime(lifetime));
    }

    private IEnumerator Lifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (reported)
            return;

        reported = true;

        if (spawner != null)
            spawner.LeafDestroyed();
    }
}