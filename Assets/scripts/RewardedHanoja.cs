
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class RewardedHanoja : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string _androidAdUnitId = "Rewarded_Android";
    private string _adUnitId;

    [SerializeField] private Button _rewardedAdButton;

    public DiskUI diskToRestore;
    public PillarUI pillarToPlace;

    private void Awake()
    {
        _adUnitId = _androidAdUnitId;

        if (_rewardedAdButton != null)
        {
            SetButton(_rewardedAdButton);
        }
        LoadAd();
    }

   
    public void SetDiskToRestore(DiskUI disk, PillarUI pillar)
    {
        diskToRestore = disk;
        pillarToPlace = pillar;
    }

    public void LoadAd()
    {
        if (diskToRestore == null)
        {
            GameObject diskObj = GameObject.Find("block5");
            if (diskObj != null)
            {
                diskToRestore = diskObj.GetComponent<DiskUI>();
                Debug.Log("RewardedAds: diskToRestore найден -> block5");
            }
        }

        if (pillarToPlace == null)
        {
            GameObject pillarObj = GameObject.Find("pillar_right");
            if (pillarObj != null)
            {
                pillarToPlace = pillarObj.GetComponent<PillarUI>();
                Debug.Log("RewardedAds: pillarToPlace найден -> pillar_right");
            }
        }

        if (pillarToPlace != null && pillarToPlace.DiskCount > 0)
        {
            Debug.Log("RewardedAds: правый столб занят, рекламу для восстановления диска не загружаем.");
            if (_rewardedAdButton != null)
                _rewardedAdButton.interactable = false;
            StartCoroutine(WaitAndLoad(5f));
            return;
        }

        if (!Advertisement.isInitialized)
        {
            Debug.LogWarning("Tried to load rewarded ad before Unity Ads was initialized.");
            return;
        }

        Debug.Log("Loading rewarded ad...");
        Advertisement.Load(_adUnitId, this);

    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Rewarded ad loaded!");

        if (!placementId.Equals(_adUnitId) || _rewardedAdButton == null)
            return;

        if (pillarToPlace != null && pillarToPlace.DiskCount > 0)
        {
            Debug.Log("RewardedAds: правый столб занят, кнопку награды не активируем.");
            _rewardedAdButton.interactable = false;
        }
        else
        {
            _rewardedAdButton.interactable = true;
        }
    }


    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogWarning($"Failed to load rewarded ad: {error} - {message}");
        StartCoroutine(WaitAndLoad(5f));
    }

    private IEnumerator WaitAndLoad(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadAd();
    }

    public void SetButton(Button button)
    {
        if (button == null) return;

        _rewardedAdButton = button;
        _rewardedAdButton.onClick.RemoveAllListeners();
        _rewardedAdButton.onClick.AddListener(ShowAd);
        _rewardedAdButton.interactable = false;
    }

    public void ShowAd()
    {
        if (pillarToPlace != null && pillarToPlace.DiskCount > 0)
        {
            Debug.Log("RewardedAds: правый столб занят, ShowAd отменён.");
            if (_rewardedAdButton != null)
            {
                _rewardedAdButton.interactable = false;
                StartCoroutine(WaitAndLoad(5f));
            }
            return;
        }

        if (_rewardedAdButton != null)
            _rewardedAdButton.interactable = false;

        Advertisement.Show(_adUnitId, this);
    }


    public void OnUnityAdsShowStart(string placementId)
    {
        Time.timeScale = 0f;
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("User clicked on rewarded ad");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogWarning($"Failed to show rewarded ad: {error} - {message}");
        Time.timeScale = 1f;
        StartCoroutine(WaitAndLoad(5f));
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Time.timeScale = 1f;
        if (placementId.Equals(_adUnitId) &&
            showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Rewarded ad completed! Restoring disk...");

            if (pillarToPlace.DiskCount > 0)
            {
                Debug.Log("RewardedAds: правый столб оказался занят к моменту награды, диск не восстанавливаем.");
                return;
            }

            if (diskToRestore == null || pillarToPlace == null)
                return;

            // 🔹 НАХОДИМ старый столб, где диск ещё висит в стеке
            PillarUI oldPillar = PillarUI.FindPillarContainingDisk(diskToRestore);
            if (oldPillar != null && oldPillar != pillarToPlace)
            {
                oldPillar.RemoveDisk(diskToRestore);
                Debug.Log($"RewardedAds: удалили {diskToRestore.name} из стека столба {oldPillar.name}");
            }

            // 🔹 КЛАДЁМ НА ПРАВЫЙ СТОЛБ
            pillarToPlace.PushDisk(diskToRestore, snapImmediately: true, keepPosition: false);

            // 🔹 Обновляем ссылки в самом диске
            diskToRestore.currentPillar = pillarToPlace;
            diskToRestore.lastPillar = pillarToPlace;
            diskToRestore.lastValidPosition = diskToRestore
                .GetComponent<RectTransform>()
                .position;

            Debug.Log("RewardedAds: disk restored and pillar references updated.");
        }
        else
        {
            Debug.Log("Rewarded ad not completed, no reward given.");
            StartCoroutine(WaitAndLoad(5f));
        }
    }

}