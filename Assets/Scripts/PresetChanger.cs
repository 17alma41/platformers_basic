using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class PresetChanger : MonoBehaviour
{
    public List<PlayerStats> presets;

    int currentProfileIndex = 0;
    PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentProfileIndex = (currentProfileIndex + 1) % presets.Count;
        }

        ChangePresetStats();
    }

    void ChangePresetStats()
    {
        player.stats = presets[currentProfileIndex];
        print(presets[currentProfileIndex].name);
    }
}
