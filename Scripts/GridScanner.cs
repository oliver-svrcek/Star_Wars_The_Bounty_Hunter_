using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScanner : MonoBehaviour
{
    private AstarPath AstarPath { get; set; } = null;
    public Coroutine ScanContinuouslyCoroutine { get; private set; } = null;

    private void Awake()
    {
        if ((AstarPath = this.gameObject.GetComponent<AstarPath>()) is null)
        {
            Debug.LogError(
                "ERROR: <GridScanner> - " + this.gameObject.name + " game object is missing AstarPath component."
            );
            Application.Quit(1);
        }
    }

    void Start()
    {
        ScanContinuously(true);
    }

    public void ScanOnce()
    {
        AstarPath.ScanAsync(AstarPath.graphs[0]);
    }
    
    public void ScanContinuously(bool scanOn)
    {
        if (scanOn && ScanContinuouslyCoroutine == null)
        {
            ScanContinuouslyCoroutine = StartCoroutine(ScanContinuously());
        }
        else if (!scanOn && ScanContinuouslyCoroutine != null)
        {
            StopCoroutine(ScanContinuouslyCoroutine);
            ScanContinuouslyCoroutine = null;
        }
    }
    
    private IEnumerator ScanContinuously()
    {
        while (true)
        {
            AstarPath.ScanAsync(AstarPath.graphs[0]);
            yield return new WaitForSeconds(1f);
        }
    }
}
