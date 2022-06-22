using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpGate : Interactable
{
    [SerializeField]private Animator animator;
    public bool activate;
    public List<Scrap> requiredScrap;
    [System.Serializable]
    public struct Scrap
    {
        public Droppable droppable;
        public int count;
    }

    public void Start()
    {
        SetupScrapRequirement();
    }

    private void SetupScrapRequirement()
    {
        Level currentLevel = BountyManager.instance.sceneDB.currentLevel;

        foreach(Level.Scrap scrap in currentLevel.requiredScrap)
        {
            Scrap newScrap = new Scrap
            {
                droppable = scrap.droppable,
                count = Random.Range(scrap.minCount, scrap.maxCount)
            };

            requiredScrap.Add(newScrap);
        }
    }

    public override void Interact(Player player) {
        if (!MeetsScrapRequirement(player))
            return;
        Debug.Log("Warp gate activated!");
        RemoveScrap(player);
        animator.SetInteger("GateState", 1);
    }

    private bool MeetsScrapRequirement(Player player)
    {
        if (activate)
            return true;

        foreach(Scrap scrap in requiredScrap)
        {
            int actualValue;
            if (player.stats.scrap.TryGetValue(scrap.droppable, out actualValue) &&
               actualValue >= scrap.count)
                continue;

            return false;
        }
        return true;
    }

    private void RemoveScrap(Player player)
    {
        if (activate)
            return;
        foreach (Scrap scrap in requiredScrap)
        {
            player.stats.scrap[scrap.droppable] -= scrap.count;
            if (player.stats.scrap[scrap.droppable] < 0)
                player.stats.scrap[scrap.droppable] = 0;
        }
    }

    private void PlayAnimation()
    {
        animator.SetInteger("gateState", 1);
    }

    public void changeToActive()
    {
        animator.SetInteger("GateState", 2);
    }

    public void LoadMapScene(float delayTime)
    {
        StartCoroutine(DelayAction(delayTime));
    }

    IEnumerator DelayAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        MapManager.instance.LoadMapScene();
    }
}
