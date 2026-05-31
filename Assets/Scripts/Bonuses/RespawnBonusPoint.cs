using System.Collections;
using UnityEngine;

public class RespawnBonusPoint : MonoBehaviour
{
    [SerializeField] private float respawnDuration;
    [SerializeField] private GameObject bonusGameObject;

    private bool isRespawning = false;


    void Update()
    {
        if (transform.childCount == 0 && !isRespawning)
            StartCoroutine(SpawnBonusRoutine());
    }

    private IEnumerator SpawnBonusRoutine()
    {
        isRespawning = true;

        yield return new WaitForSeconds(respawnDuration);

        var newBonus = Instantiate(bonusGameObject, transform.position, Quaternion.identity);
        newBonus.transform.SetParent(transform);

        isRespawning = false;
    }
}
