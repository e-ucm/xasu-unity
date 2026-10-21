using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Xasu;
using Xasu.HighLevel;

public class ChoosePath : MonoBehaviour
{
    public CanvasGroup buttons;
    public Button interacted, pathA, pathB;

    public async void Start()
    {
        await Task.Yield();
        buttons.interactable = false;
        Debug.Log("[CHOOSEPATH] Sending Initialized and progressed trace");
        CompletableTracker.Instance.Initialized("MyGame", CompletableTracker.CompletableType.Game);
        CompletableTracker.Instance.Progressed("MyGame", CompletableTracker.CompletableType.Game, 0.1f);
        Debug.Log("[CHOOSEPATH] Done!");
        Debug.Log("[CHOOSEPATH] Sending Accessed trace");
        AccessibleTracker.Instance.Accessed("choosePath");
        Debug.Log("[CHOOSEPATH] Done!");
        Debug.Log("[CHOOSEPATH] Sending Initialized choose path level");
        CompletableTracker.Instance.Initialized("choosePath", CompletableTracker.CompletableType.Level);
        Debug.Log("[CHOOSEPATH] Done!");
        interacted.onClick.AddListener(Interacted);
        pathA.onClick.AddListener(PathA);
        pathB.onClick.AddListener(PathB);

        buttons.interactable = true;
    }

    public void Interacted()
    {
        interacted.interactable = false;
        Debug.Log("[CHOOSEPATH] Sending Interacted trace");
        GameObjectTracker.Instance.Interacted("choose-path-interacted-button");
        Debug.Log("[CHOOSEPATH] Done!");
        interacted.interactable = true;
    }

    public void PathA()
    {
        pathA.interactable = false;
        Debug.Log("[CHOOSEPATH] Sending Completed choose path level");
        CompletableTracker.Instance.Completed("choosePath", CompletableTracker.CompletableType.Level).WithSuccess(true);
        Debug.Log("[CHOOSEPATH] Done!");
        Debug.Log("[CHOOSEPATH] Sending Accessed trace");
        AccessibleTracker.Instance.Accessed("pathA");
        Debug.Log("[CHOOSEPATH] Done!");
        SceneManager.LoadScene("pathA");
    }

    public void PathB()
    {
        pathB.interactable = false;
        Debug.Log("[CHOOSEPATH] Sending Completed choose path level");
        CompletableTracker.Instance.Completed("choosePath", CompletableTracker.CompletableType.Level).WithSuccess(true);
        Debug.Log("[CHOOSEPATH] Done!");
        Debug.Log("[CHOOSEPATH] Sending Accessed trace");
        AccessibleTracker.Instance.Accessed("pathB");
        Debug.Log("[CHOOSEPATH] Done!");
        SceneManager.LoadScene("pathB");
    }
}
