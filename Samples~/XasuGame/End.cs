using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Xasu;
using Xasu.HighLevel;

public class End : MonoBehaviour
{
    public CanvasGroup buttons;
    public Button interacted, finalized;

    public async void Start()
    {
        await Task.Yield();
        buttons.interactable = false;
        
        Debug.Log("[END] Sending Progressed trace");
        CompletableTracker.Instance.Progressed("MyGame", CompletableTracker.CompletableType.Game, 0.75f);
        Debug.Log("[END] Done!");
        
        Debug.Log("[END] Sending Initialized end level");
        CompletableTracker.Instance.Initialized("endScene", CompletableTracker.CompletableType.Level);
        Debug.Log("[END] Done!");

        interacted.onClick.AddListener(Interacted); 
        finalized.onClick.AddListener(Finalized);
        buttons.interactable = true;
    }

    public void Interacted()
    {
        interacted.interactable = false;
        Debug.Log("[END] Sending Interacted trace");
        GameObjectTracker.Instance.Interacted("endScene-interacted-button");
        Debug.Log("[END] Done!");
        interacted.interactable = true;
    }

    public async void Finalized()
    {
        finalized.interactable = false;
        Debug.Log("[END] Sending Completed end level");
        await CompletableTracker.Instance.Completed("endScene", CompletableTracker.CompletableType.Level).WithSuccess(true);
        Debug.Log("[END] Done!");
        Debug.Log("[END] Sending Finalize trace");
        await CompletableTracker.Instance.Progressed("MyGame", CompletableTracker.CompletableType.Game, 1f);
        await CompletableTracker.Instance.Completed("MyGame", CompletableTracker.CompletableType.Game).WithSuccess(true);
        Debug.Log("[END] Done!");
        buttons.interactable = false;
        var progress = new Progress<float>();
        progress.ProgressChanged += (_, p) =>
        {
            Debug.Log("Finalization progress: " + p);
        };
        await Xasu.XasuTracker.Instance.Finalize(progress);
        Debug.Log("Tracker finalized, game is now ready to close...");
        if (Application.isEditor) {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        } else {
            Application.Quit();
        }
    }
}
