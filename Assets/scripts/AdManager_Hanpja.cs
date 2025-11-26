using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdManager_Hanpja : MonoBehaviour
{
    public AdsInitializator adsInitializer;
    public InterstitialAd interstitialAd;
    [SerializeField] bool turnOffInterstitialAd = false;
    private bool firstAdShown = false;

    public RewardedHanoja rewardedAds;
    [SerializeField] bool turnOffRewardedAds = false;

    public BannerAd bannerAd;
    [SerializeField] bool turnOffBannerAd = false;

    public static AdManager_Hanpja Instance { get; private set; }

    private void Awake()
    {
        if (adsInitializer == null)
            adsInitializer = FindFirstObjectByType<AdsInitializator>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        //Instance = this;
        //DontDestroyOnLoad(gameObject);

        if (adsInitializer != null)
            adsInitializer.OnAdsIntitialized += HandleAdsInitialized;
    }

    private void HandleAdsInitialized()
    {
        if (!turnOffInterstitialAd && interstitialAd != null)
        {
            interstitialAd.OnInterstitialAdReady -= HandleInterstitialReady;
            interstitialAd.OnInterstitialAdReady += HandleInterstitialReady;
            interstitialAd.LoadAd();
        }

        if (!turnOffRewardedAds && rewardedAds != null)
        {
            rewardedAds.LoadAd();
        }
    }

    private void HandleInterstitialReady()
    {
        if (!firstAdShown && interstitialAd != null)
        {
            Debug.Log("Showing interstitial ad automatically!");
            interstitialAd.ShowAd();
            firstAdShown = true;
        }
        else
        {
            Debug.Log("Next interstitial ad is ready for manual show!");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private bool firstSceneLoad = false;
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (interstitialAd == null)
            interstitialAd = FindFirstObjectByType<InterstitialAd>();

        var interstitialGo = GameObject.FindGameObjectWithTag("IntestitialAdButton");
        Button interstitialButton = interstitialGo != null ? interstitialGo.GetComponent<Button>() : null;

        if (interstitialAd != null && interstitialButton != null)
        {
            interstitialAd.SetButton(interstitialButton);
        }

        if (rewardedAds == null)
            rewardedAds = FindFirstObjectByType<RewardedHanoja>();

        var rewardedGo = GameObject.FindGameObjectWithTag("RewardedButton");
        Button rewardedAdButton = rewardedGo != null ? rewardedGo.GetComponent<Button>() : null;

        if (rewardedAds != null && rewardedAdButton != null)
            rewardedAds.SetButton(rewardedAdButton);

        if (bannerAd == null)
            bannerAd = FindFirstObjectByType<BannerAd>();


        if (!firstSceneLoad)
        {
            firstSceneLoad = true;
            Debug.Log("First time scene loaded!");
            return;
        }

        Debug.Log("Scene loaded!");

        firstAdShown = false;
        HandleAdsInitialized();
    }
}