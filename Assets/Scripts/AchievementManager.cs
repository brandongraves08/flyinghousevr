using UnityEngine;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }
    
    [System.Serializable]
    public class Achievement
    {
        public string id;
        public string title;
        public string description;
        public int rewardCredits;
        public bool unlocked;
        public string iconName;
    }
    
    [Header("Achievements")]
    public List<Achievement> achievements = new List<Achievement>();
    
    void Awake()
    {
        Instance = this;
        InitializeAchievements();
    }
    
    void InitializeAchievements()
    {
        // Explorer achievements
        AddAchievement("first_flight", "First Flight", "Complete your first flight mission", 100);
        AddAchievement("navigator", "Navigator", "Unlock the Navigation Room", 200);
        AddAchievement("world_traveler", "World Traveler", "Visit 5 different cities", 500);
        AddAchievement("stargazer", "Stargazer", "Unlock the Observatory", 300);
        AddAchievement("engineer", "Engineer", "Unlock the Engine Bay", 300);
        
        // Flight achievements
        AddAchievement("high_flyer", "High Flyer", "Reach 2000m altitude", 400);
        AddAchievement("storm_chaser", "Storm Chaser", "Fly through a storm", 250);
        AddAchievement("landing", "Smooth Landing", "Complete NYC scenario without crashing", 150);
        
        // Room achievements
        AddAchievement("home_owner", "Home Owner", "Unlock 5 different rooms", 750);
        AddAchievement("captain", "Captain", "Unlock the Captain's Study", 500);
        AddAchievement("collector", "Collector", "Unlock all 8 rooms", 2000);
        AddAchievement("room_scanner", "Room Scanner", "Complete AR room scan", 200);
    }
    
    void AddAchievement(string id, string title, string desc, int reward)
    {
        if (!achievements.Exists(a => a.id == id))
        {
            achievements.Add(new Achievement { 
                id = id, 
                title = title, 
                description = desc, 
                rewardCredits = reward 
            });
        }
    }
    
    public void UnlockAchievement(string id)
    {
        var ach = achievements.Find(a => a.id == id);
        if (ach == null || ach.unlocked) return;
        
        ach.unlocked = true;
        
        var upgradeSystem = FindObjectOfType<HouseUpgradeSystem>();
        if (upgradeSystem != null)
        {
            upgradeSystem.AddCredits(ach.rewardCredits);
        }
        
        // Show notification
        HUDManager.Instance?.ShowMessage($"Achievement Unlocked: {ach.title}", $"+${ach.rewardCredits}");
        
        SaveAchievements();
    }
    
    void SaveAchievements()
    {
        foreach (var ach in achievements)
        {
            PlayerPrefs.SetInt($"Ach_{ach.id}", ach.unlocked ? 1 : 0);
        }
        PlayerPrefs.Save();
    }
    
    public bool IsUnlocked(string id)
    {
        var ach = achievements.Find(a => a.id == id);
        return ach?.unlocked ?? false;
    }
}
