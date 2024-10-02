using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoDragons_Lava : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(DisableTime());
    }

    private IEnumerator DisableTime()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}
