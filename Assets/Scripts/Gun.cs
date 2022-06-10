using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    Transform shootPoint;

    [SerializeField]
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        Debug.Log("Shooting...");
        GameObject bullet = GameObject.Instantiate(bulletPrefab);
        bullet.transform.position = shootPoint.transform.position;
        bullet.GetComponent<Rigidbody>().velocity = shootPoint.transform.forward * speed;
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());
    }
}
