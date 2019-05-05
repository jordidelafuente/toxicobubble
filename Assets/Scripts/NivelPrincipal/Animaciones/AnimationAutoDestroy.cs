using UnityEngine;
using System.Collections;

public class AnimationAutoDestroy : MonoBehaviour
{
    public float delay = 0f;

    // Use this for initialization
    void Start()
    {
        if (   gameObject.transform.parent.tag == "Explosion"
            || gameObject.transform.parent.tag == "AnimCoin+1"
            || gameObject.transform.parent.tag == "AnimBola+1") //TODO: diferenciar delay por tipos de animaciones
        {
            Destroy(gameObject.transform.parent.gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
        }
    }
}
