using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0Controller : MonoBehaviour
{
    public float lifeTime = 0f;
    private int behaviour = 0;
    public Vector3 speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime;


        if (lifeTime > 0f)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0f)
                Destroy(gameObject);
        }
    }
}
