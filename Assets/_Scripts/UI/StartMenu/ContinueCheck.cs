using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timeout(2));
    }

    IEnumerator Timeout(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (DataPersistanceManager.instance.IsNewGame())
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
