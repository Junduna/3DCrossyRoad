using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
   [SerializeField] Player player;
   [SerializeField] Vector3 offset;

    private void Start()
    {
        offset = this.transform.position - player.transform.position;
    }

    Vector3 lastAnimalPos;
    private void Update()
    {
        // camera stop follow klo player udh mati
        // IsDie, define player.enabled = false;
        if (player.IsDie || lastAnimalPos == player.transform.position)
            return;

        var targetAnimalPos = new Vector3(
            player.transform.position.x,
            0,
            player.transform.position.z
        );

        transform.position = targetAnimalPos + offset;

        lastAnimalPos = player.transform.position;
    } 
}
