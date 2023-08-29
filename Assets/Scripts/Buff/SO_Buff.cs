using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_Buff : ScriptableObject
{
    /*
    public string buffName;
    public string buffDescription;
    */
    public Sprite buffIcon;
    public bool isOneTimeUse = true;

    public virtual void UseBuff(GameObject player)
    {

    }

    public virtual IEnumerator ResetBuff()
    {
        yield return null;
    }

    public Sprite GetIcon()
    {
        return buffIcon;
    }
}
