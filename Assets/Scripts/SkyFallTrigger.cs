using UnityEngine;

public class SkyfallTileTrigger : MonoBehaviour
{
    public SkyfallTile parentTile;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            if (parentTile != null)
            {
                parentTile.TriggerTile();
            }
        }
    }
}