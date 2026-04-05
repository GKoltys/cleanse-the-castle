using UnityEngine;
using System;

public class StairsController : MonoBehaviour, IMapGenInit
{
    private PlayerBase playerBase;
    private MapGenerator dungeon;
    private bool isUnlocked = true;

    public void Init(MapGenerator controller)
    {
        playerBase = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBase>();
        dungeon = controller;
    }

    // set stairs to active, used for boss floor after defeating boss
    public void Unlock()
    {
        isUnlocked = true;
    }

    // set stairs to active, used for boss floor after defeating boss
    public void Lock()
    {
        isUnlocked = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dungeon == null) return;

        if (!isUnlocked) return;

        if (other.CompareTag("Player"))
        {
            SoundEffectManager.Play(SoundGroupName.STAIRS);

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