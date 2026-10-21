using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Xasu;
using Xasu.HighLevel;

public class PathB : MonoBehaviour
{
    public CanvasGroup buttons;
    public Button interacted, goToEnd;

    public async void Start()
    {
        await Task.Yield();
        buttons.interactable = false;
        
        Debug.Log("[PATHB] Sending Progressed trace");
        CompletableTracker.Instance.Progressed("MyGame", CompletableTracker.CompletableType.Game, 0.5f);
        Debug.Log("[PATHB] Done!");
        
        Debug.Log("[PATHB] Sending Initialized pathB level");
        CompletableTracker.Instance.Initialized("pathB", CompletableTracker.CompletableType.Level);
        Debug.Log("[PATHB] Done!");

        interacted.onClick.AddListener(Interacted);
        goToEnd.onClick.AddListener(GoToEnd);

        buttons.interactable = true;
    }

    public void Interacted()
    {
        interacted.interactable = false;
        Debug.Log("[PATHB] Sending Interacted trace");
        GameObjectTracker.Instance.Interacted("pathB-interacted-button");
        Debug.Log("[PATHB] Done!");
        interacted.interactable = true;
    }

    public void GoToEnd()
    {
        goToEnd.interactable = false;
        Debug.Log("[PATHB] Sending Completed pathB level");
        CompletableTracker.Instance.Completed("pathB", CompletableTracker.CompletableType.Level).WithSuccess(false);
        Debug.Log("[PATHB] Done!");
        Debug.Log("[PATHB] Sending Accessed trace");
        AccessibleTracker.Instance.Accessed("endScene");
        Debug.Log("[PATHB] Done!");
        goToEnd.interactable = true;
        SceneManager.LoadScene("End");
    }
}
