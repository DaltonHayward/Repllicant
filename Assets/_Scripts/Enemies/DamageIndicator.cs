using UnityEngine;
using System.Collections;
using System.Linq;

public class DamageIndicator : MonoBehaviour
{
    private Color[] _originalMaterialColor;

    // Start is called before the first frame update
    void Start()
    {
        // Store the original color of the model
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        _originalMaterialColor = new Color[renderers.Sum(r => r.materials.Length)]; // Initialize the array
        int count = 0;
        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.materials)
            {
                _originalMaterialColor[count] = material.color;
                count++;
            }
        }
    }

    // changes model color to indicate damaged
    public void Hurt()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.materials)
            {
                material.color = Color.red;
            }
        }
        StartCoroutine(HurtTimeout(0.1f));
    }

    IEnumerator HurtTimeout(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        int count = 0;
        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.materials)
            {
                material.color = _originalMaterialColor[count];
                count ++;
            }
        }
    }
}

