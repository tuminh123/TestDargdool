using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;
using UnityEngine.Networking;
using HadesSDK;
using HadesSDK.Ads.Core;
using HadesSDK.Ads.Runtime;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class EventTracking : MonoBehaviour
{
    private void Start()
    {
        AppStateEventNotifier.AppStateChanged += OnAppStateChange;
        
        _ingameNoAdStopwatch.Start();
        _ingameStopWatch.Start();
        _timeWatch.Start();
        
        StartCoroutine(CheckFTUETimeplay());
    }
    #region Gameplay
    public void OnSelectItemModeSandbox(string itemName)
    {
        FirebaseService.Instance.LogEvent("Select_item_mode_sandbox", 
            new EventParameter("name", itemName));
    }
    public void OnSelectCategoryModeSandbox(string categoryName)
    {
        FirebaseService.Instance.LogEvent("Select_category_mode_sandbox", 
            new EventParameter("name", categoryName));
        
    }
    public void UnlockItemAds(string itemName)
    {
        FirebaseService.Instance.LogEvent("Unlock_item_ads", 
            new EventParameter("name", itemName)
            /*GetSceneLocation()*/);
    }
    
    public void OnClickBtnReplay(bool isIngame) // or endgame
    {
        FirebaseService.Instance.LogEvent("Btn_replay",
            new EventParameter("location", isIngame ? "ingame" : "endgame"));
    }

    public void OnClickBtnRandomChar()
    {
        FirebaseService.Instance.LogEvent("Btn_random_char",new EventParameter() /*GetSceneLocation()*/);
    }

    public void OnSelectItemModeCampaigns(string itemName)
    {
        FirebaseService.Instance.LogEvent("Select_item_mode_campaigns",
            new EventParameter("name", itemName));
    }

    public void OnSelectMode(string modeName)
    {
        FirebaseService.Instance.LogEvent("Select_mode",
            new EventParameter("name", modeName));
    }

    public void OnGetCoinAds(int level)
    {
        FirebaseService.Instance.LogEvent("Get_coin_ads",
            new EventParameter("level", level.ToString()));
    }

    public void OnSelectMap(string mapName)
    {
        FirebaseService.Instance.LogEvent("Select_map",
            new EventParameter("name", mapName));
    }

    public void OnDailyLogin(int day)
    {
        FirebaseService.Instance.LogEvent("Daily_login",
            new EventParameter("name", $"D{day}"));
    }

    public void OnTicketIncome(string source)
    {
        FirebaseService.Instance.LogEvent("Ticket_income",
            new EventParameter("name", source));
    }
    public void OnTicketOutcome(string source)
    {
        FirebaseService.Instance.LogEvent("Ticket_outcome",
            new EventParameter("name", source));
    }

    public void OnSelectBoss(string bossName)
    {
        FirebaseService.Instance.LogEvent("Select_boss",
            new EventParameter("name", bossName));
    }

    public void OnBossLevelStart(int count, string bossID)
    {
        FirebaseService.Instance.LogEvent("Boss_level_start", new EventParameter("number", count.ToString()),
            new EventParameter("boss", bossID));
    }
    public void OnBossLevelReplay(int count, string bossID)
    {
        FirebaseService.Instance.LogEvent("Boss_level_replay", new EventParameter("number", count.ToString()),
            new EventParameter("boss", bossID));
        // new EventParameter("location", isBattleStarted ? "battleStarted" : "battleNotStarted"));
    }

    public void OnBossLevelEnd(int count, string bossID, int timePlay, float damage)
    {
        FirebaseService.Instance.LogEvent("Boss_level_end", 
            new EventParameter("number", count.ToString()),
            new EventParameter("boss", bossID),
            new EventParameter("timeplay","" /*timePlay.SecondsToMinute()*/),
            new EventParameter("hp", damage.ToString("F2")));
    }

    public void OnBellIncome(string source)
    {
        FirebaseService.Instance.LogEvent("Bell_income",
            new EventParameter("location", source));
    }
    public void OnBellOutcome(string source)
    {
        FirebaseService.Instance.LogEvent("Bell_outcome",
            new EventParameter("function", source));
    }
    
    #endregion
    
    #region Level

    public void OnLevelStart(int level)
    {
        FirebaseService.Instance.LogEvent("level_start", 
            new EventParameter("level", level.ToString())
        );
        // FTUE_PlayLevel(level);
    }
    public void OnLevelFail(int level)
    {
        FirebaseService.Instance.LogEvent("level_fail",
            new EventParameter("level", level.ToString())
        );
    }

    public void OnLevelReplay(int level, bool isIngame)
    {
        FirebaseService.Instance.LogEvent("level_replay",
            new EventParameter("level", level.ToString()),
            new EventParameter("location", isIngame ? "ingame" : "endgame")
        );
    }

    public void OnLevelWin(int level)
    {
        FirebaseService.Instance.LogEvent("level_win",
            new EventParameter("level", level.ToString())
        );
    }
    #endregion
    
    #region FTUE

/*    public void FTUE_SelectChar(string charName)
    {
        FirebaseService.Instance.LogEventFTUE("FTUE_select_char",
            new EventParameter("name", charName));
    }

    public void FTUE_DropCharGreen(string charName)
    {
        FirebaseService.Instance.LogEventFTUE("FTUE_drop_char_green",
            new EventParameter("name", charName));
    }
    public void FTUE_DropCharRed(string charName)
    {
        FirebaseService.Instance.LogEventFTUE("FTUE_drop_char_red",
            new EventParameter("name", charName));
    }

    public void FTUE_ClickCatWeapon1()
    {
        FirebaseService.Instance.LogEventFTUE("FTUE_click_cat_weapon1");
    }
    public void FTUE_SelectWeapon1(string weaponName)
    {
        FirebaseService.Instance.LogEventFTUE("FTUE_select_weapon1",
            new EventParameter("name", weaponName));
    }
    public void FTUE_DropWeapon1(string weaponName)
    {
        FirebaseService.Instance.LogEventFTUE("FTUE_drop_weapon1",
            new EventParameter("name", weaponName));
    }
    public void FTUE_ClickCatWeapon2()
    {
        FirebaseService.Instance.LogEventFTUE("FTUE_click_cat_weapon2");
    }
    public void FTUE_SelectWeapon2(string weaponName)
    {
        FirebaseService.Instance.LogEventFTUE("FTUE_select_weapon2",
            new EventParameter("name", weaponName));
    }
    public void FTUE_DropWeapon2(string weaponName)
    {
        FirebaseService.Instance.LogEventFTUE("FTUE_drop_weapon2",
            new EventParameter("name", weaponName));
    }

    public void FTUE_ClickStart()
    {
        FirebaseService.Instance.LogEventFTUE("FTUE_click_start");
    }

    public void FTUE_EndingReplay()
    {
        FirebaseService.Instance.LogEventFTUE("FTUE_ending_replay");
    }

    public void FTUE_SelectMap()
    {
        FirebaseService.Instance.LogEventFTUE("FTUE_select_map");
    }
    
    public void FTUE_PlayLevel(int level)
    {
        if (level > 10) return;
        FirebaseService.Instance.LogEventFTUE("FTUE_level_play_" + level, null);
    }
    public void FTUE_WinLevel(int level)
    {
        if (level > 10) return;
        FirebaseService.Instance.LogEventFTUE("FTUE_level_win_" + level, null);
    }
    public void FTUE_ReplayLevel(int level)
    {
        if (level > 10) return;
        FirebaseService.Instance.LogEventFTUE("FTUE_level_replay_" + level, null);
    }
    public void FTUE_RewardLevel(int level, string itemType)
    {
        if (level > 10) return;
        FirebaseService.Instance.LogEventFTUE("FTUE_level_reward_" + level, 
            new EventParameter("item_type", itemType));
    }*/
    // public void FTUE_UserPlaytime(int time)
    // {
    //     FirebaseService.Instance.LogEventFTUE("FTUE_user_playtime_" + time + "m", null);
    // }
    
    #endregion

    #region PlayTime
    private Stopwatch _ingameNoAdStopwatch = new Stopwatch();
    private Stopwatch _ingameStopWatch = new Stopwatch();
    private Stopwatch _timeWatch = new Stopwatch();
    public TimeSpan IngameNoAdTimeElapsed => _ingameNoAdStopwatch.Elapsed;
    public TimeSpan IngameTimeElapsed => _ingameStopWatch.Elapsed;
    public long IngameNoAdTimeElapsedMillisecond => _ingameNoAdStopwatch.ElapsedMilliseconds;
    public long IngameTimeElapsedMillisecond => _ingameStopWatch.ElapsedMilliseconds;
    
    void OnAppStateChange(AppState appState)
    {
        Debug.Log("App State Change: "+appState);

        if (appState == AppState.Background)
        {
            _ingameNoAdStopwatch.Stop();
            _timeWatch.Stop();

            double timePlayMinutes = ES3.Load<double>("TimePlay", 0);
            ES3.Save("TimePlay", timePlayMinutes + _timeWatch.Elapsed.TotalMinutes);
            _timeWatch.Reset();
        }
        else if (appState == AppState.Foreground)
        {
            _ingameNoAdStopwatch.Start();
            _timeWatch.Start();
        }
    }
    IEnumerator CheckFTUETimeplay()
    {
        WaitForSeconds waitAMinute = new WaitForSeconds(60);
        while (true)
        {
            yield return waitAMinute;
            int currentMilestone = PlayerPrefs.GetInt("TimePlayMileStone", 0);
            double mileStoneTime = 0;
            string mileStoneTimeKey = "0";
            switch (currentMilestone)
            {
                case 0:
                    mileStoneTime = 1; mileStoneTimeKey = "1";
                    break;
                case 1:
                    mileStoneTime = 3; mileStoneTimeKey = "3";
                    break;
                case 2:
                    mileStoneTime = 5; mileStoneTimeKey = "5";
                    break;
                case 3:
                    mileStoneTime = 10; mileStoneTimeKey = "10";
                    break;
                case 4:
                    mileStoneTime = 15; mileStoneTimeKey = "15";
                    break;
                case 5:
                    mileStoneTime = 20; mileStoneTimeKey = "20";
                    break;
                case 6:
                    mileStoneTime = 25; mileStoneTimeKey = "25";
                    break;
                case 7:
                    mileStoneTime = 30; mileStoneTimeKey = "30";
                    break;
                case 8:
                    mileStoneTime = 35; mileStoneTimeKey = "35";
                    break;
                default:
                    mileStoneTime = 35; mileStoneTimeKey = "35";
                    break;
            }
            double userTimePlay = ES3.Load<double>("TimePlay", 0);
            double timeElapsed = _timeWatch.Elapsed.TotalMinutes;

            if (userTimePlay + timeElapsed >= mileStoneTime)
            {
                _timeWatch.Stop();
                ES3.Save("TimePlay", userTimePlay + timeElapsed);
                _timeWatch.Reset();
                _timeWatch.Start();
                    
                //FirebaseService.Instance.LogEventFTUE($"FTUE_user_playtime_{mileStoneTimeKey}m");

                currentMilestone++;
                PlayerPrefs.SetInt("TimePlayMileStone", currentMilestone);
            }
        }
    }
    public static int GetDayDiff()
    {
        DateTime firstOpenDate = FirstOpenDate;
        DateTime now = DateTime.Now;

        int daysDifference = (now - firstOpenDate).Days;

        return daysDifference > 0 ? daysDifference : 0;
    }
    public static DateTime FirstOpenDate
    {
        get
        {
            if (!ES3.KeyExists("FirstOpenDate"))
            {
                ES3.Save("FirstOpenDate", DateTime.Now);
            }

            return ES3.Load<DateTime>("FirstOpenData", DateTime.Now);
        }
    }
    #endregion
    
    #region Feedback

    public string _feedbackFormURL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfQRJKsLf6HX5PELIJORs0K3MpFRDzqZ4KXdSrqwEuooxOe1Q/formResponse";
    public string _formEntry = "entry.1100241580";
    public void SendFeedback(string feedback, int star)
    {
        try
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string userFeedback = star + " - " + currentDate + " - " + feedback;

            StartCoroutine(Post(userFeedback));
        }
        catch (Exception e)
        {
            Debug.Log("Send feedback failed: " + e.Message);
        }
    }
    IEnumerator Post(string feedback)
    {
        WWWForm form = new WWWForm();
        form.AddField(_formEntry, feedback);
        UnityWebRequest www = UnityWebRequest.Post(_feedbackFormURL, form);
        yield return www.SendWebRequest();
        Debug.Log("Send feedback successfully");
    }
    #endregion

    #region Huy add

    public void UnlockItemAdsModeCampaigns(string itemName)
    {
        FirebaseService.Instance.LogEvent("Unlock_item_ads_mode_campaigns", 
            new EventParameter("name", itemName));
    }
    public void OnSelectCategoryModeCampaigns(string categoryName)
    {
        FirebaseService.Instance.LogEvent("Select_category_mode_campaigns", 
            new EventParameter("name", categoryName));
        
    }

    #endregion

    #region IAP

  /*  public void OnFirstInAppPurchase(string productId, string packName)
    {
        // string key = $"FirstIAP_{productId}";
        string key = $"FirstIAP_Shop";

        if (ES3.KeyExists(key)) return; // đã log => bỏ qua

        // Bắn event
        FirebaseService.Instance.LogEvent("First_in_app_purchase",
            AdManager.GetInternetParameter(),
            new EventParameter("time", $"D{GetDayDiff()}"),
            new EventParameter("type", packName),
            GetSceneLocation()
        );
        ES3.Save(key, true);
    }
    EventParameter GetSceneLocation()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Splash":
                return new EventParameter("location", "splash");
            case "Home":
                return new EventParameter("location", "home");
            case "Main":
                return new EventParameter("location", "sandbox");
            case "Campaigns":
                return new EventParameter("location", "campaigns");
            case "Noel":
                return new EventParameter("location", "noel");
            default:
                return new EventParameter("location", "sandbox");
        }
    }*/
   /* public void OnIAPNoAdsClick()
    {
        FirebaseService.Instance.LogEvent("IAP_noads_click", 
            AdManager.GetInternetParameter(),
            GetSceneLocation()
            );
    }
    public void OnIAPNoAdsFail()
    {
        FirebaseService.Instance.LogEvent("IAP_noads_fail",
            AdManager.GetInternetParameter(),
            GetSceneLocation()
            );
    }

    public void OnIAPNoAdsSuccess(bool isRetore)
    {
        FirebaseService.Instance.LogEvent("IAP_noads_success",
            AdManager.GetInternetParameter(),
            GetSceneLocation(),
            new EventParameter("type", isRetore ? "restore" : "firstTime")
            );
    }

    public void OnOpenNoAdsPopup()
    {
        FirebaseService.Instance.LogEvent("Open_noads_popup", GetSceneLocation());
    }
    public void OnIAPClick(string productName)
    {
        var productNameStandardize = productName.Replace(" ", "_");
        FirebaseService.Instance.LogEvent($"IAP_{productNameStandardize}_click", AdManager.GetInternetParameter(),
            PopupManager.Instance.GetLocationParameter());
    }
    public void OnIAPFail(string productName)
    {
        var productNameStandardize = productName.Replace(" ", "_");
        FirebaseService.Instance.LogEvent($"IAP_{productNameStandardize}_fail", AdManager.GetInternetParameter(),
            PopupManager.Instance.GetLocationParameter());
    }
    public void OnIAPSuccess(string productName, bool isRestore)
    {
        var productNameStandardize = productName.Replace(" ", "_");
        FirebaseService.Instance.LogEvent($"IAP_{productNameStandardize}_success", AdManager.GetInternetParameter(),
            PopupManager.Instance.GetLocationParameter(),
            new EventParameter("type", isRestore? "restore" : "firstTime"));
    }*/
    
    
    #endregion
    
    
}
