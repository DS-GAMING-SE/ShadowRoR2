using HedgehogUtils.Miscellaneous;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ShadowMod.Characters.Survivors.Shadow.Components
{
    public class HomingTracker : MonoBehaviour, ITargetTracker
    {
        private Indicator indicator;
        private LockOnIndicator lockOnIndicator;
        private bool lockOnIndicatorFound;

        public bool visible;
        public bool locked
        {
            get { return _locked; }
            set
            {
                _locked = value;
                if (visible)
                {
                    IndicatorRetarget(lockOnIndicator);
                }
            }
        }
        private bool _locked;

        private CharacterBody characterBody;
        private InputBankTest inputBank;
        private TeamComponent teamComponent;

        private BullseyeSearch search = new BullseyeSearch();

        private Transform previousTarget;
        private HurtBox trackingTarget;

        private float trackerUpdateStopwatch;
        private float trackerUpdateFrequency = 10f;

        public float maxRange { get; private set; }

        private void Awake()
        {
            this.indicator = new Indicator(base.gameObject, HedgehogUtils.Assets.lockOnIndicator); //LegacyResourcesAPI.Load<GameObject>("Prefabs/HuntressTrackingIndicator")
            this.characterBody = base.GetComponent<CharacterBody>();
            characterBody.onRecalculateStats += RecalculateHomingStats;
            this.inputBank = base.GetComponent<InputBankTest>();
            this.teamComponent = base.GetComponent<TeamComponent>();
        }

        public HurtBox GetTrackingTarget()
        {
            return this.trackingTarget;
        }
        private void OnEnable()
        {
            this.indicator.active = true;
            if (this.indicator.hasVisualizer)
            {
                InitializeIndicator();
            }
            RecalculateHomingStats(characterBody);
        }
        private void InitializeIndicator()
        {
            this.lockOnIndicator = this.indicator.visualizerInstance.GetComponent<LockOnIndicator>();
            if (lockOnIndicator)
            {
                lockOnIndicatorFound = true;
                ResetColors();
            }
        }
        public void ResetColors()
        {
            if (lockOnIndicator)
            {
                lockOnIndicator.SetColors(ShadowSurvivor.shadowYellowColor, ShadowSurvivor.shadowRedColor);
            }
        }
        public void SetColors(Color color, Color color2)
        {
            if (lockOnIndicator)
            {
                lockOnIndicator.SetColors(color, color2);
            }
        }
        private void OnDisable()
        {
            this.indicator.active = false;
        }

        private void FixedUpdate()
        {
            if (!Util.HasEffectiveAuthority(base.gameObject)) return;
            if (!lockOnIndicatorFound && this.indicator.hasVisualizer) { InitializeIndicator(); }
            this.trackerUpdateStopwatch += Time.fixedDeltaTime;
            if (this.trackerUpdateStopwatch >= 1f / this.trackerUpdateFrequency)
            {
                this.trackerUpdateStopwatch = 0;
                this.SearchForTarget(inputBank.GetAimRay());
                if (visible)
                {
                    if (!locked)
                    {
                        IndicatorRetarget(lockOnIndicator);
                    }
                }
                else
                {
                    this.indicator.targetTransform = null;
                }
                previousTarget = this.indicator.targetTransform;
            }
        }
        private void SearchForTarget(Ray aimRay)
        {
            this.search.teamMaskFilter = TeamMask.GetUnprotectedTeams(this.teamComponent.teamIndex);
            this.search.filterByLoS = true;
            this.search.searchOrigin = aimRay.origin;
            this.search.searchDirection = aimRay.direction;
            this.search.sortMode = BullseyeSearch.SortMode.DistanceAndAngle;
            this.search.maxDistanceFilter = maxRange;
            this.search.minDistanceFilter = 0;
            this.search.maxAngleFilter = 12;
            this.search.RefreshCandidates();
            this.search.FilterOutGameObject(base.gameObject);
            this.trackingTarget = this.search.GetResults().FirstOrDefault(hurt => hurt && hurt.healthComponent && hurt.healthComponent.body && hurt.healthComponent.alive);
        }
        private void IndicatorRetarget(LockOnIndicator indicator)
        {
            this.indicator.targetTransform = trackingTarget ? this.trackingTarget.transform : null;
            if (indicator && this.indicator.targetTransform && (!previousTarget || this.indicator.targetTransform != previousTarget))
            {
                indicator.UpdateTarget();
                Util.PlaySound("Play_hedgehogutils_lockon", base.gameObject);
            }
        }

        private void RecalculateHomingStats(CharacterBody characterBody)
        {
            CalculateMaxRange();
        }

        private void CalculateMaxRange()
        {
            maxRange = 60f;
        }
    }
}
