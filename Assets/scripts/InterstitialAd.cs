using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

public class InterstitialAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{

    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    string _adUnitId;

    public event Action OnInterstitialAdReady;
    public bool isReady = false;
    [SerializeField] Button _intestitialAdButton;

    void Awake()
    {
        _adUnitId = _androidAdUnitId;
    }

    private void Update()
    {
        if (AdManager.Instance != null && AdManager.Instance.interstitialAd != null)
        {
            _intestitialAdButton.interactable = isReady;
        }
    }

    public void OnInterstitialAdButtonClicked()
    {
        Debug.Log("Interstitial ad button clicked");
        ShowIntestitial();
    }

    public void LoadAd()
    {
        if(!Advertisement.isInitialized)
        {
            Debug.LogWarning("Tried to load intestitial ad before Unity ads was initialized.");
            return;
        }

        Debug.Log("Loading intestitial ad");
        Advertisement.Load(_adUnitId, this);
    }

    public void ShowAd()
    {
        if (isReady)
        {
            Advertisement.Show(_adUnitId, this);
            isReady = false;
        } else
        {
            Debug.LogWarning("Ad ir not ready yet");
        }
    }

    public void ShowIntestitial()
    {
        if(AdManager.Instance.interstitialAd != null && isReady)
        {
            Debug.Log("Showing Ad manually");
            ShowAd();
        } else
        {
            Debug.Log("Ad is not ready yet, loading again");
            LoadAd();
        }
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Interstitial Ad loaded!");
        _intestitialAdButton.interactable = true;
        isReady = true;
        OnInterstitialAdReady?.Invoke();
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogWarning("Failed to load Ad");
        LoadAd();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("User clicked on Ad");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if(showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Ad watched is completed");
            StartCoroutine(SlowDownTimeTemperarlly(30f));
            LoadAd();
        } else
        {
            Debug.Log("Interstitial ad skipped is unknown!");
            Time.timeScale = 1.0f;
            LoadAd();
        }
    }

    private IEnumerator SlowDownTimeTemperarlly(float seconds)
    {
        Time.timeScale = 0.4f;
        Debug.Log("Time is slowed down 4x for: " + seconds + "sec");
        yield return new WaitForSeconds(seconds);

        Time.timeScale = 1.0f;
        Debug.Log("Time is returned to normal");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("Error showing interstitial ad!");
        LoadAd();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Showing intersitial Ad at this moment");
        Time.timeScale = 0f;
    }

    public void SetButton(Button button)
    {
        if (button == null)
            return;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnInterstitialAdButtonClicked);
        _intestitialAdButton = button;
        _intestitialAdButton.interactable = false;
    }
}
