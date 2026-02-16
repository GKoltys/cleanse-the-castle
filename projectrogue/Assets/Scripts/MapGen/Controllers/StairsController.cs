using UnityEngine;
using System;

public class StairsController : MonoBehaviour, IMapGenInit
{
    private MapGenerator dungeon;

    public void Init(MapGenerator controller)
    {
        dungeon = controller;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dungeon == null) return;

        if (other.CompareTag("Player"))
        {
            // https://docs.unity3d.com/6000.3/Documentation/ScriptReference/RigidbodyConstraints2D.html
            // disable movement
            var rb = other.attachedRigidbody;
            if (rb)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll; // freeze movement
                rb.Sleep();
            }
            // disable animation
            var anim = other.GetComponentInChildren<Animator>();
            if (anim)
            {
                anim.enabled = false;
            }

            // transition to next floor
            dungeon.GoToNextFloor();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // enable movement
        var rb = other.attachedRigidbody;
        if (rb)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // freeze rotation only allowing movement again
            rb.WakeUp();
        }

        // enable animation
        var anim = other.GetComponentInChildren<Animator>();
        if (anim)
        {
            anim.enabled = true;
        }
    }
}