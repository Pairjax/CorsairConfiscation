using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercerHarpoon : ComponentBehavior
{
    void Start() { name = "Piercer Harpoon"; }
}

public class MultiHarpoonLauncher : ComponentBehavior
{
    void Start() 
    {
        name = "Multi-Harpoon Launcher";

        foreach (Harpoon h in playerController.harpoons)
            h.gameObject.SetActive(true);
    }

    // Sets only the first harpoon active
    void OnDestroy()
    {
        foreach (Harpoon h in playerController.harpoons)
            h.gameObject.SetActive(false);

        playerController.harpoons[0].gameObject.SetActive(true);
    }
}

public class TheMaw : ComponentBehavior
{
    void Start() { name = "The Maw"; }

}

