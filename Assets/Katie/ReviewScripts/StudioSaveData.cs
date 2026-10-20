using System;
using System.Collections.Generic;

// Stores a snapshot of the studio.
[Serializable]
public class StudioSaveData
{
    public int version = 1;

    // Player resources.
    public int money;
    public int fandom;
    public int developmentSkill;
    public int technicalDebt;

    // AI credits, purchase prices and subscription.
    public int aiCredits;
    public int aiCreditPurchaseCount;
    public bool aiSubscriptionActive;
    public bool aiSubscriptionPaused;
    public int nextAISubscriptionDay;

    // In-game clock.
    public int currentDay;
    public int currentHour;
    public int currentMinute;

    // Current project and completed-project history.
    public ProjectData currentProject;
    public List<ProjectData> completedProjects =
        new List<ProjectData>();

    public bool currentProjectArchived;
    public int fullAIProjects;
    public DevelopmentStage developmentStage;

    // Current project conditions.
    public float quality;
    public float workload;
    public int bugs;
    public float totalTime;

    // Previously applied results prevent duplicate rewards.
    public StageResult codingResult;
    public StageResult designResult;
    public StageResult soundResult;
    public StageResult debuggingResult;

    // Purchased room upgrades.
    public bool pcUpgrade;
    public bool lightUpgrade;
    public bool cleanUpgrade;
    public bool windowUpgrade;
    public bool decorUpgrade;

    // Player title and its progression counters.
    public int gamesDeveloped;
    public int manualDevelopment;
    public int totalMoneyEarned;
    public int currentTitleLevel;

    // Sound settings.
    public float bgmVolume;
    public float sfxVolume;
    public float keyboardVolume;

    // Each activity will supply its own progress snapshot.
    public string codingProgressJson;
    public string designProgressJson;
    public string soundProgressJson;
    public string debuggingProgressJson;
    public string buildProgressJson;

    public bool hasCodingResult;
    public bool hasDesignResult;
    public bool hasSoundResult;
    public bool hasDebuggingResult;
}