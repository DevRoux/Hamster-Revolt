using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfExplode : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(25);
        Object.Destroy(this.gameObject);
    }




}
