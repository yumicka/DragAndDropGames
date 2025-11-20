using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class BannerAd : MonoBehaviour
{
    [SerializeField] string _androidAdUnitID = "Banner_Android";
    string _adUnitId;
    [SerializeField] Button _bannerButton;
    public bool isBannerVisible = false;
    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    private void Awake()
    {
        _adUnitId = _androidAdUnitID;
        Advertisement.Banner.SetPosition(_bannerPosition);
    }

    public void LoadBanner()
    {
        if (!Advertisement.isInitialized)
        {
            Debug.LogWarning("Tried to load banner ad before ads was initialized!");
            return;
        }

        Debug.Log("Loading banner ad...");
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = onBannerError
        };

        Advertisement.Banner.Load(_adUnitId, options);
    }

    void OnBannerLoaded()
    {
        Debug.Log("Banner ad loaded successfully");
        _bannerButton.interactable = true;
    }

    void onBannerError(string message)
    {
        Debug.LogWarning($"Banner ad failed to load: {message}");
        LoadBanner();
    }

    public void ShowBannerAd()
    {
        if (isBannerVisible)
        {
            HideBannerAd();
        }
        else
        {
            BannerOptions options = new BannerOptions
            {
                showCallback = OnBannerShown,
                hideCallback = OnBannerHidden,
                clickCallback = OnBannerClicked
            };
            Advertisement.Banner.Show(_adUnitId, options);
        }
    }

    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    void OnBannerShown()
    {
        Debug.Log("Banner ad is now visible");
        isBannerVisible = false;
    }

    void OnBannerHidden()
    {
        Debug.Log("Banner ad is now hidden.");
        isBannerVisible = false;
    }

    void OnBannerClicked()
    {
        Debug.Log("Banner ad was clicked");
    }

    public void SetButton(Button button)
    {
        if (button == null)
            return;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ShowBannerAd);
        _bannerButton = button;
        _bannerButton.interactable = false;
    }
}
