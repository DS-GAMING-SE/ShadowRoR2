using HedgehogUtils.Miscellaneous;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static UnityEngine.UIElements.StylePropertyAnimationSystem;

namespace ShadowMod.Characters.Survivors.Shadow.Components
{
    public class HomingTracker : MonoBehaviour, ITargetTracker
    {
        private IndicatorPair[] indicators;
        private bool allIndicatorsInitialized;
        public int maxTargets
        {
            get { return _maxTargets; }
            set 
            {
                value = Math.Max(value, 1);
                UpdateIndicatorCount(value);
                _maxTargets = value;
            }
        }
        private int _maxTargets = 1;

        public bool visible;
        public bool locked
        {
            get { return _locked; }
            set
            {
                _locked = value;
                if (visible)
                {
                    foreach (var indicator in indicators)
                    {
                        IndicatorRetarget(indicator);
                    }
                }
            }
        }
        private bool _locked;

        private CharacterBody characterBody;
        private InputBankTest inputBank;
        private TeamComponent teamComponent;

        private BullseyeSearch search = new BullseyeSearch();

        private float trackerUpdateStopwatch;
        private float trackerUpdateFrequency = 8f;

        private static Vector3 secondaryIndicatorScale = new Vector3(0.6f, 0.6f, 0.6f);

        private Color primaryColor;
        private Color secondaryColor;

        public float maxRange { get; private set; }

        private void Awake()
        {
            indicators = new IndicatorPair[maxTargets];
            for (int i = 0; i < indicators.Length; i++)
            {
                indicators[i] = new IndicatorPair { indicator = new Indicator(base.gameObject, HedgehogUtils.Assets.lockOnIndicator), primary = i == 0 };
            }
            allIndicatorsInitialized = false;
            this.characterBody = base.GetComponent<CharacterBody>();
            characterBody.onRecalculateStats += RecalculateHomingStats;
            this.inputBank = base.GetComponent<InputBankTest>();
            this.teamComponent = base.GetComponent<TeamComponent>();
            ResetColors();
        }

        public HurtBox GetTrackingTarget()
        {
            return indicators[0].target;
        }
        public void GetTrackingTargets(List<HurtBox> results, int max = -1)
        {
            HG.ListUtils.EnsureCapacity(results, Math.Min(_maxTargets, max));
            foreach (var indicator in indicators)
            {
                if (indicator.target)
                {
                    results.Add(indicator.target);
                }
                if (results.Count >= Math.Min(_maxTargets, max)) break;
            }
        }
        private void OnEnable()
        {
            foreach (var indicator in indicators)
            {
                indicator.indicator.active = true;
                if (!indicator.initialized && indicator.indicator.hasVisualizer)
                {
                    InitializeIndicator(indicator);
                }
            }
            RecalculateHomingStats(characterBody);
        }
        private void InitializeIndicator(IndicatorPair indicator)
        {
            LockOnIndicator lockOnIndicator = indicator.indicator.visualizerInstance.GetComponent<LockOnIndicator>();
            if (!indicator.primary)
            {
                indicator.indicator.visualizerInstance.transform.localScale = secondaryIndicatorScale;
            }
            indicator.initialized = true;
            indicator.lockOnIndicator = lockOnIndicator;
            if (lockOnIndicator)
            {
                UpdateColors(indicator);
            }
        }
        public void ResetColors()
        {
            SetColors(ShadowSurvivor.shadowYellowColor, ShadowSurvivor.shadowRedColor);
        }
        public void SetColors(Color primary, Color secondary)
        {
            primaryColor = primary;
            secondaryColor = secondary;
            UpdateColors();
        }
        private void UpdateColors()
        {
            foreach (var indicator in indicators)
            {
                if (indicator.lockOnIndicator) indicator.lockOnIndicator.SetColors(primaryColor, secondaryColor);
            }
        }
        private void UpdateColors(IndicatorPair indicator)
        {
            if (indicator.lockOnIndicator) indicator.lockOnIndicator.SetColors(primaryColor, secondaryColor);
        }
        private void OnDisable()
        {
            foreach (var indicator in indicators)
            {
                indicator.indicator.active = false;
            }
        }

        private void FixedUpdate()
        {
            if (!Util.HasEffectiveAuthority(base.gameObject)) return;
            if (!allIndicatorsInitialized)
            {
                bool init = true;
                foreach (var indicator in indicators)
                {
                    if (!indicator.initialized)
                    {
                        if (indicator.indicator.hasVisualizer)
                        {
                            InitializeIndicator(indicator);
                        }
                        else
                        {
                            init = false;
                        }
                    }
                }
                allIndicatorsInitialized = init;
            }
            this.trackerUpdateStopwatch += Time.fixedDeltaTime;
            if (this.trackerUpdateStopwatch >= 1f / this.trackerUpdateFrequency && !locked)
            {
                this.trackerUpdateStopwatch = 0;
                this.SearchForTarget(inputBank.GetAimRay());
                if (visible)
                {
                    foreach (var indicator in indicators)
                    {
                        IndicatorRetarget(indicator);
                    }
                }
                else
                {
                    foreach (var indicator in indicators)
                    {
                        indicator.indicator.targetTransform = null;
                    }
                }
                for (int i = 0; i < maxTargets; i ++)
                {
                    indicators[i].previousTarget = indicators[i].indicator.targetTransform;
                }
            }
        }
        private void SearchForTarget(Ray aimRay)
        {
            this.search.viewer = characterBody;
            this.search.teamMaskFilter = TeamMask.GetUnprotectedTeams(this.teamComponent.teamIndex);
            this.search.filterByLoS = true;
            this.search.filterByDistinctEntity = true;
            this.search.searchOrigin = aimRay.origin;
            this.search.searchDirection = aimRay.direction;
            this.search.sortMode = BullseyeSearch.SortMode.Angle;
            this.search.maxDistanceFilter = maxRange;
            this.search.minDistanceFilter = 0;
            this.search.maxAngleFilter = 12;
            this.search.RefreshCandidates();
            this.search.FilterOutGameObject(base.gameObject);
            int targetedCount = 0;
            foreach (var indicator in indicators)
            {
                indicator.target = null;
            }
            foreach (var hurt in search.GetResults())
            {
                if (hurt && hurt.healthComponent && hurt.healthComponent.body && hurt.healthComponent.alive)
                {
                    indicators[targetedCount++].target = hurt;
                }
                if (targetedCount >= indicators.Length) break;
            }
        }
        private void IndicatorRetarget(IndicatorPair indicator)
        {
            if (!indicator.initialized) return;

            indicator.indicator.targetTransform = indicator.target ? indicator.target.transform : null;
            if (indicator.lockOnIndicator && indicator.indicator.targetTransform && 
                (!indicator.previousTarget || indicator.indicator.targetTransform != indicator.previousTarget))
            {
                indicator.lockOnIndicator.UpdateTarget();
                Util.PlaySound("Play_hedgehogutils_lockon", base.gameObject);
            }
        }
        private void UpdateIndicatorCount(int value)
        {
            if (value < _maxTargets)
            {
                for (int i = value; i < maxTargets; i++)
                {
                    indicators[i].indicator.active = false;
                }
            }
            Array.Resize(ref indicators, value);
            if (value > _maxTargets)
            {
                allIndicatorsInitialized = false;
                for (int i = _maxTargets; i < value - _maxTargets; i++)
                {
                    indicators[i] = new IndicatorPair { indicator = new Indicator(base.gameObject, HedgehogUtils.Assets.lockOnIndicator) };
                }
            }
        }

        private void RecalculateHomingStats(CharacterBody characterBody)
        {
            CalculateMaxRange();
        }
        private void CalculateMaxRange()
        {
            maxRange = ShadowStaticValues.homingRange;
        }

        private class IndicatorPair
        {
            public Indicator indicator;
            public LockOnIndicator lockOnIndicator;
            public HurtBox target;
            public Transform previousTarget;
            public bool initialized;
            public bool primary;
        }
    }
}
