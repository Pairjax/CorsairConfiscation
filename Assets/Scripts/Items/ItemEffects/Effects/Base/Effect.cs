using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float effectCooldown;
    public Collider2D col2D;
    public SpriteRenderer rend;
    internal bool disabled;
    public virtual void Disable()
    {
        col2D.enabled = false;
        rend.enabled = false;
    }
    public virtual void Enable()
    {
        col2D.enabled = true;
        rend.enabled = true;
        disabled = false;
    }
    internal IEnumerator DisableEffect(float dur)
    {
        Disable();
        yield return new WaitForSeconds(dur);
        Enable();

    }


}
