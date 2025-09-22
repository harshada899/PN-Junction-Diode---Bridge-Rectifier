using UnityEngine;

public class animation : MonoBehaviour
{
    public Animator anim;
    public GameObject tui;

    private bool isTalking = false;

    void Update()
    {
        if (tui.activeSelf && !isTalking)
        {
            anim.SetBool("isTalking", true);
            isTalking = true;
            Debug.Log("Started Talking");
        }
        else if (!tui.activeSelf && isTalking)
        {
            anim.SetBool("isTalking", false);
            isTalking = false;
            Debug.Log("Stopped Talking");
        }
    }
}
