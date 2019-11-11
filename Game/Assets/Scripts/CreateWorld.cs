using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWorld : MonoBehaviour
{
    public GameObject[] prefab = null;
    private float currentOffset = 0f;
    private float xPosition = 0f;
    private const float estimatedSize = 4.5f;
    public GameObject player;

    private GameObject old = null;
    private GameObject current = null;
    private int x = 0;
    // Start is called before the first frame update
    void Start()
    {
        /*Vector3 position = new Vector3(transform.position.x - 1f, transform.position.y - 0.5f , transform.position.z);
        current = Instantiate(prefab[x], position, Quaternion.identity);
        currentOffset += estimatedSize;
        x = (x+1)%prefab.Length;*/
    }

   
    void FixedUpdate ()
    {
       /* if (currentOffset - 2f< player.transform.position.x) {
            xPosition = transform.position.x + currentOffset;
            Vector3 position = new Vector3(xPosition, transform.position.y - 0.5f , transform.position.z);
            if (old)
                Destroy(old);
            old = current;
            current = Instantiate(prefab[x], position, Quaternion.identity);   
            x = (x+1)%prefab.Length;
            currentOffset += 3f;
        }
        if (player.transform.position.x > xPosition) {
            currentOffset = xPosition + estimatedSize;
            xPosition += estimatedSize;
        }   */
    }



}
