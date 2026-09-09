using System.Collections.Generic;

namespace YuetanMingqu
{
    [System.Serializable]
    public class GameConfig
    {
        public float version;
        public DayLoop dayLoop;
        public AberrationPool aberrationPool;
        public Work work;
        public Training training;
        public Recruitment recruitment;
        public Economy economy;
        public Save save;
        public List<DrawRate> drawRates;
    }

    [System.Serializable]
    public class DayLoop
    {
        public int finalDay;
        public int trueEndingDeadlineDay;
        public List<int> memoryDays;
        public int quotaBase;
        public float quotaGrowthPer10Days;
        public float trialDayQuotaMultiplier;
    }

    [System.Serializable]
    public class AberrationPool
    {
        public int candidatesBase;
        public int candidatesPerDays;
        public int candidatesMax;
    }

    [System.Serializable]
    public class Work
    {
        public float failOutputRatio;
        public float diminishingReturnsStep;
        public float growthOnSuccess;
        public float growthOnFailure;
        public int collapseDays;
    }

    [System.Serializable]
    public class Training
    {
        public List<int> costByTargetLevel;
    }

    [System.Serializable]
    public class Recruitment
    {
        public int cost;
        public List<int> slotsByFacilityLevel;
    }

    [System.Serializable]
    public class Economy
    {
        public int starStoneConversionRate;
    }

    [System.Serializable]
    public class Save
    {
        public string directoryName;
    }

    [System.Serializable]
    public class DrawRate
    {
        public int fromDay;
        public int toDay;
        public float zayin;
        public float teth;
        public float he;
        public float waw;
        public float aleph;
    }
}

