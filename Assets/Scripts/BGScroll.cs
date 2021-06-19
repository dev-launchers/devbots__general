using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour {

    [SerializeField] private Transform BG1 = default(Transform);
    [SerializeField] private int BG1Len = default(int);
    [SerializeField] private float BG1Speed = default(float);

    [SerializeField] private Transform BG2 = default(Transform);
    [SerializeField] private int BG2Len = default(int);
    [SerializeField] private float BG2Speed = default(float);

    // Update is called once per frame
    void Update()
    {
        BG1.position = new Vector3(BG1Speed * Time.time % BG1Len, BG1.position.y, BG1.position.z);
        BG2.position = new Vector3(BG2Speed * Time.time % BG2Len, BG2.position.y, BG2.position.z);
    }
}
