using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Represents a rarity type with a name and a weight (chance)

public class RngWeighted : MonoBehaviour
{
    [Tooltip("List of rarities with their weights. Weights represent relative chances.")]
    public List<Rarity> rarities = new List<Rarity>()
    {
        new Rarity("Common", 70f),
        new Rarity("Uncommon", 20f),
        new Rarity("Rare", 7f),
        new Rarity("Epic", 2f),
        new Rarity("Legendary", 1f),
    };
    
    private float totalWeight;

    void Awake()
    {
        // Calculate the total weight once for efficiency
        totalWeight = 0f;
        foreach (var rarity in rarities)
        {
            if (rarity.Weight < 0)
            {
                Debug.LogWarning($"Rarity {rarity.Name} has negative weight. Setting to 0.");
                rarity.Weight = 0;
            }
            totalWeight += rarity.Weight;
            Debug.Log(rarity.Weight);
        }

        if (totalWeight <= 0)
        {
            Debug.LogError("Total weight of rarities must be greater than zero.");
        }
    }

    // Roll a rarity based on weighted chances
    public Rarity Roll()
    {
        if (rarities.Count == 0 || totalWeight <= 0f)
        {
            Debug.LogError("Rarities list is empty or total weight is invalid.");
            return null;
        }

        float roll = UnityEngine.Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var rarity in rarities)
        {
            cumulative += rarity.Weight;
            if (roll <= cumulative)
            {
                return rarity;
            }
        }

        // Should never reach here unless due to floating point rounding
        return rarities[rarities.Count - 1];
    }


    // Example usage: print the roll result
    public TextMeshProUGUI resultText; // Optional, assign in inspector to show result on UI
    public void RollOnClick()
    {
        Rarity rolled = Roll();
        if (rolled != null)
        {
            Debug.Log($"You rolled: {rolled.Name} rarity!");
            if (resultText != null)
                resultText.text = $"You rolled: {rolled.Name} rarity!";
        }
    }
    // Remove rolling from Update or leave it empty.
    void Update()
    {
        // No roll here anymore
    }
}
[Serializable]
public class Rarity
{
    public string Name;
    public float Weight; // The higher the weight, the more common the rarity is

    public Rarity(string name, float weight)
    {
        Name = name;
        Weight = weight;
    }
}



