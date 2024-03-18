using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenSong : MonoBehaviour
{
    [SerializeField] private float emitFrequency;
    private float broadcastRange;
    private string channel;

    public Material areaEffectMaterial; // Assign a material with transparency
    public float pulseSpeed = 2f; // Speed of the pulse effect
    public float minScale = 0.9f; // Minimum scale of the effect
    public float maxScale = 1.1f; // Maximum scale of the effect
    public Color safeColor = Color.green; // Color when player is outside the charm's effect
    public Color dangerColor = Color.red; // Color when player is within the charm's effect
    private GameObject visualCueObject; // The object that will show the visual cue
    private float currentScale = 1.0f;
    private bool isIncreasing = true;

    private IEnumerator emissionCoroutine;

    private void Awake()
    {
        CreateVisualCue();
    }

    private void Update()
    {
        PulseEffect();
        UpdateColorBasedOnPlayerDistance();
    }

    public void SetParameters(float freq, float range, string ch)
    {
        emitFrequency = freq;
        broadcastRange = range;
        channel = ch;
        emissionCoroutine = CoEmit();
        StartCoroutine(emissionCoroutine);
    }

    private IEnumerator CoEmit()
    {
        while (true)
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, broadcastRange);
            foreach (Collider c in targets)
            {
                ISubscriber[] subs = c.GetComponents<ISubscriber>();
                if (subs != null)
                {
                    foreach (ISubscriber sub in subs)
                    {
                        sub.ReceiveMessage(channel);
                        if (c.gameObject.GetComponent<Charmable>() != null)
                            c.gameObject.GetComponent<Charmable>().Siren = transform;
                    }
                }
            }

            yield return new WaitForSeconds(emitFrequency);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, broadcastRange);
    }

    void CreateVisualCue()
    {
        visualCueObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Destroy(visualCueObject.GetComponent<Collider>()); // We don't need the collider
        visualCueObject.transform.SetParent(transform, false);
        visualCueObject.transform.localScale = new Vector3(maxScale, 0.01f, maxScale); // Flat cylinder
        visualCueObject.transform.localPosition = Vector3.zero; // Centered on the Siren
        visualCueObject.GetComponent<Renderer>().material = areaEffectMaterial;
    }

    void PulseEffect()
    {
        if (isIncreasing)
        {
            currentScale += pulseSpeed * Time.deltaTime;
            if (currentScale >= maxScale)
            {
                currentScale = maxScale;
                isIncreasing = false;
            }
        }
        else
        {
            currentScale -= pulseSpeed * Time.deltaTime;
            if (currentScale <= minScale)
            {
                currentScale = minScale;
                isIncreasing = true;
            }
        }

        visualCueObject.transform.localScale = new Vector3(currentScale, 0.01f, currentScale);
    }

    void UpdateColorBasedOnPlayerDistance()
    {
        float distance = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        float effectRange = (maxScale - minScale) / 2.0f; // Assuming the effect range is related to the scale for simplicity

        if (distance <= effectRange)
        {
            visualCueObject.GetComponent<Renderer>().material.color = Color.Lerp(dangerColor, safeColor, distance / effectRange);
        }
        else
        {
            visualCueObject.GetComponent<Renderer>().material.color = safeColor;
        }
    }
}
