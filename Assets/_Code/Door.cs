using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] UnityEvent onDoorOpen = new UnityEvent();
    [SerializeField] UnityEvent onDoorClose = new UnityEvent();

    [SerializeField, EventRef] private string doorOpenSound;
    [SerializeField, EventRef] private string doorCloseSound;
    

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            animator.SetBool("character_nearby", true);
            onDoorOpen.Invoke();
            RuntimeManager.PlayOneShot(doorOpenSound);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            animator.SetBool("character_nearby", false);
            onDoorClose.Invoke();
            RuntimeManager.PlayOneShot(doorCloseSound);
        }
    }
}
