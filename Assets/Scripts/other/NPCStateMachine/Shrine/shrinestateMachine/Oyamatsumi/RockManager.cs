using UnityEngine;
using System.Collections;
public class RockManager : MonoBehaviour
{
    public Objectpool RockPool;
   // public Transform player;
    public float RockSpeed;
    public float AtkTimer;
   // public float RockNumber;
   
    public float fireRate;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
            AtkTimer += Time.deltaTime;

        if (AtkTimer > fireRate)
        {
            RookShoot();
            AtkTimer = 0f;
        }

    }
    void RookShoot()
    {
        GameObject rock = RockPool.GetObject();
        rock.transform.position = transform.position;
        rock.transform.rotation = transform.rotation;
        
        
        Rigidbody rb = rock.GetComponent<Rigidbody>();
        Collider col = rb.GetComponent<Collider>();
        
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * RockSpeed;
        }
      
        StartCoroutine(DeactivateBullet(rock));
    }
    IEnumerator DeactivateBullet(GameObject rock)
    {
        yield return new WaitForSeconds(AtkTimer);
        RockPool.ReturnObject(rock);
    }
}