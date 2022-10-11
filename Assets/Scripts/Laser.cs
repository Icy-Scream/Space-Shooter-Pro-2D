using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _laserSpeed;
    [SerializeField] private float _destroyTimer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyLaserRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        LaserTranslate();
    }

    private void LaserTranslate()
    {
        transform.Translate(Vector3.up*_laserSpeed*Time.deltaTime);
    }
    IEnumerator DestroyLaserRoutine()
    {
        yield return new WaitForSeconds(_destroyTimer);
        Object.Destroy(this.gameObject);
    }
}
