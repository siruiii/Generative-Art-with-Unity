using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldParticle : MonoBehaviour
{
    public float _moveSpeed;
    public int _audioBand; // react on audio
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!float.IsNaN(_moveSpeed) & !float.IsNaN(Time.deltaTime))
        {
            this.transform.position += transform.forward * _moveSpeed * Time.deltaTime;
        }
    }
    public void ApplyRotation(Vector3 rotation, float rotateSpeed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(rotation.normalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}
