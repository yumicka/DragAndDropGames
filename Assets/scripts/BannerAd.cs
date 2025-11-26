using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAd : MonoBehaviour
{
    [SerializeField] string _androidAdUnitID = "Banner_Android";
    string _adUnitId;

    public bool isBannerVisible = false;

    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    void Awake()
    {
        _adUnitId = _androidAdUnitID;
        Advertisement.Banner.SetPosition(_bannerPosition);
    }

    public void LoadAndShowBanner()
    {
        if (!Advertisement.isInitialized)
        {
            Debug.LogWarning("Tried to load banner before Unity Ads was initialized!");
            return;
        }

        Debug.Log("Loading banner ad...");
        BannerLoadOptions loadOptions = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Advertisement.Banner.Load(_adUnitId, loadOptions);
    }

    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded, showing...");
        BannerOptions showOptions = new BannerOptions
        {
            showCallback = OnBannerShown,
            hideCallback = OnBannerHidden,
            clickCallback = OnBannerClicked
        };

        Advertisement.Banner.Show(_adUnitId, showOptions);
    }

    void OnBannerError(string message)
    {
        Debug.LogWarning($"Banner failed to load: {message}");
        LoadAndShowBanner();
    }

    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    void OnBannerShown()
    {
        Debug.Log("Banner is now visible");
        isBannerVisible = true;
    }

    void OnBannerHidden()
    {
        Debug.Log("Banner is now hidden");
        isBannerVisible = false;
    }

    void OnBannerClicked()
    {
        Debug.Log("Banner was clicked");
    }
}
