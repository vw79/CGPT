using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDropper : MonoBehaviour
{
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minRefreshTime;
    [SerializeField] private float maxRefreshTime;

    [SerializeField] private List<Buff> buffs;
    private bool isRefreshing = false;

    private int weightSum = 0;
    private List<int> weights = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(Buff buff in buffs)
        {
            weightSum += buff.weight;
            weights.Add(weightSum);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRefreshing)
        {
            StartCoroutine(DropBuff());
        }
    }

    private IEnumerator DropBuff()
    {
        isRefreshing = true;
        yield return new WaitForSeconds(Random.Range(minRefreshTime, maxRefreshTime));
        Instantiate(ChooseBuff(), new Vector3(Random.Range(minX, maxX), transform.position.y, transform.position.z), Quaternion.identity);
        isRefreshing = false;
    }

    private GameObject ChooseBuff()
    {
        int choice = Random.Range(0, weightSum);
        for (int i = 0; i < weights.Count; i++)
        {
            if(choice < weights[i])
            {
                return buffs[i].buff;
            }
        }
        return null;
    }
}

[System.Serializable] 
public struct Buff
{
    public int weight;
    public GameObject buff;
}
