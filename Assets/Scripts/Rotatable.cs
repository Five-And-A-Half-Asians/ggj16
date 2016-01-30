using UnityEngine;
using System.Collections;

public class Rotatable : MonoBehaviour {
    void Start()
    {
        float rand = Random.Range(0, 360);
        transform.Rotate(new Vector3(rand, rand, rand));
    }

    void LateUpdate()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}
