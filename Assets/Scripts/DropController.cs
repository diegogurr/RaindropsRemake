using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameController;

public class DropController : MonoBehaviour
{
    [SerializeField] private float dropGenerationInterval = 2.0f;

    private List<Drop> activeDrops = new List<Drop>();

    [SerializeField] private float goldenDropProbability = 0.05f;
    [SerializeField] private int pointsForEveryDrop = 500;

    [SerializeField] private float slowdownFactor = 0.5f; 
    [SerializeField] private float slowdownDuration = 5f; 

    public void StartGeneratingDrops()
    {
        StartCoroutine(GenerateDropsPeriodically());
    }

    public void StopGeneratingDrops()
    {
        StopAllCoroutines();
    }

    IEnumerator GenerateDropsPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(dropGenerationInterval);
            GenerateDrop();
        }
    }

    public void GenerateDrop()
    {
        Drop newDrop = DropPool.Instance.GetDrop();
        newDrop.OnDestroyed += RemoveDrop;
        newDrop.OnThresholdReached += DropReachedThreshold;

        ConfigureDropAppearance(newDrop);

        activeDrops.Add(newDrop);
    }

    private void ConfigureDropAppearance(Drop drop)
    {
        drop.isGoldenDrop=Random.value< goldenDropProbability;

        if (drop.isGoldenDrop)
        {
            drop.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            drop.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void IncreaseDifficultyLevel()
    {
        foreach (var drop in activeDrops)
        {
            drop.difficultyLevel++;
        }
    }

    // Check if userInput passed by InputController is corresponding to one or more of the drop results
    public void CheckForMatches(int userInput)
    {
        bool matchFound = false;
        bool goldenDropSolved = false;

        for (int i = activeDrops.Count - 1; i >= 0; i--)
        {
            if (activeDrops[i].GetResult() == userInput)
            {
                matchFound = true;
                GameController.Instance.ResolveDrop(pointsForEveryDrop);

                if (activeDrops[i].isGoldenDrop)
                {
                    goldenDropSolved = true;
                }
                RemoveDrop(activeDrops[i]);
            }
        }

        // Golden drop solves all drops active
        if (matchFound && goldenDropSolved)
        {
            ResolveAllDrops();
        }
        else if (!matchFound)
        {
            Debug.Log("Error: no match found for the input.");
            AudioController.Instance.PlaySound("DropError");
        }
    }

    public void ResolveAllDrops()
    {
        while (activeDrops.Count > 0)
        {
            RemoveDrop(activeDrops[0]);
            GameController.Instance.ResolveDrop(pointsForEveryDrop);
        }
    }

    public void ExplodeAllDrops()
    {
        foreach (var drop in activeDrops.ToList())
        {
            RemoveDrop(drop);
        }
    }

    private void DropReachedThreshold(Drop drop)
    {
        GameController.Instance.LoseLife();
    }

    public void RemoveDrop(Drop drop)
    {
        drop.OnThresholdReached -= DropReachedThreshold;
        DropPool.Instance.ReturnDrop(drop);
        activeDrops.Remove(drop);
    }

    // ApplySlowdownToAllDrops needs to be optimized
    public void ApplySlowdownToAllDrops()
    {
        StartCoroutine(SlowdownRoutine());
    }

    private IEnumerator SlowdownRoutine()
    {
        // Apply slowdown to all active drops
        foreach (var drop in activeDrops)
        {
            drop.AdjustFallSpeed(slowdownFactor);
        }

        yield return new WaitForSeconds(slowdownDuration);

        // Reset the speed
        foreach (var drop in activeDrops)
        {
            drop.ResetFallSpeed();
        }
    }
}