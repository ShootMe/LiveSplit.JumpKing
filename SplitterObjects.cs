using System;
using System.ComponentModel;
namespace LiveSplit.JumpKing {
    public enum LogObject {
        CurrentSplit,
        Pointers,
        PointerVersion,
        PlayerEntity,
        PlayerScreen,
        PlayerX,
        PlayerY,
        GameTime,
        TimesWon
    }
    public enum Screen {
        RedcrownWoods1,
        RedcrownWoods2,
        RedcrownWoods3,
        RedcrownWoods4,
        RedcrownWoods5,
        ColossalDrain1,
        ColossalDrain2,
        ColossalDrain3,
        ColossalDrain4,
        ColossalDrain5,
        FalseKingsKeep1,
        FalseKingsKeep2,
        FalseKingsKeep3,
        FalseKingsKeep4,
        Bargainburg1,
        Bargainburg2,
        Bargainburg3,
        Bargainburg4,
        Bargainburg5,
        GreatFrontier1,
        GreatFrontier2,
        GreatFrontier3,
        GreatFrontier4,
        GreatFrontier5,
        GreatFrontier6,
        WindsweptBluff1,
        StormwallPass1,
        StormwallPass2,
        StormwallPass3,
        StormwallPass4,
        StormwallPass5,
        StormwallPass6,
        ChapelPerilous1,
        ChapelPerilous2,
        ChapelPerilous3,
        ChapelPerilous4,
        BlueRuin1,
        BlueRuin2,
        BlueRuin3,
        TheTower1,
        TheTower2,
        TheTower3,
        MainBabe,
        Unknown1,
        Bargainburg6,
        Bargainburg7,
        BrightcrownWoods1,
        BrightcrownWoods2,
        BrightcrownWoods3,
        BrightcrownWoods4,
        BrightcrownWoods5,
        BrightcrownWoods6,
        ColossalDungeon1,
        ColossalDungeon2,
        ColossalDungeon3,
        ColossalDungeon4,
        ColossalDungeon5,
        ColossalDungeon6,
        ColossalDungeon7,
        FalseKingsCastle1,
        FalseKingsCastle2,
        FalseKingsCastle3,
        FalseKingsCastle4,
        Underburg1,
        Underburg2,
        Underburg3,
        Underburg4,
        Underburg5,
        Underburg6,
        Underburg7,
        LostFrontier1,
        LostFrontier2,
        LostFrontier3,
        LostFrontier4,
        LostFrontier5,
        LostFrontier6,
        LostFrontier7,
        HiddenKingdom1,
        HiddenKingdom2,
        HiddenKingdom3,
        HiddenKingdom4,
        HiddenKingdom5,
        HiddenKingdom6,
        BlackSanctum1,
        BlackSanctum2,
        BlackSanctum3,
        BlackSanctum4,
        BlackSanctum5,
        BlackSanctum6,
        DeepRuin1,
        DeepRuin2,
        DeepRuin3,
        DeepRuin4,
        DeepRuin5,
        TheDarkTower1,
        TheDarkTower2,
        TheDarkTower3,
        TheDarkTower4,
        TheDarkTower5,
        NewBabe,
        Unknown2,
        Bog1,
        Bog2,
        Bog3,
        Bog4,
        Bog5,
        Bog6,
        Bog7,
        MouldingManor1,
        MouldingManor2,
        MouldingManor3,
        MouldingManor4,
        MouldingManor5,
        MouldingManor6,
        MouldingManor7,
        MouldingManor8,
        Bugstalk1,
        Bugstalk2,
        Bugstalk3,
        Bugstalk4,
        Bugstalk5,
        Bugstalk6,
        Bugstalk7,
        HouseOfNineLives1,
        HouseOfNineLives2,
        HouseOfNineLives3,
        HouseOfNineLives4,
        HouseOfNineLives5,
        HouseOfNineLives6,
        HouseOfNineLives7,
        PhantomTower1,
        PhantomTower2,
        PhantomTower3,
        PhantomTower4,
        PhantomTower5,
        PhantomTower6,
        PhantomTower7,
        PhantomTower8,
        PhantomTower9,
        HaltedRuin1,
        HaltedRuin2,
        HaltedRuin3,
        HaltedRuin4,
        HaltedRuin5,
        HaltedRuin6,
        HaltedRuin7,
        HaltedRuin8,
        TowerOfAntumbra1,
        TowerOfAntumbra2,
        TowerOfAntumbra3,
        TowerOfAntumbra4,
        TowerOfAntumbra5,
        TowerOfAntumbra6,
        GhostBabe,
        Unknown3,
        PhilosophersForest1,
        PhilosophersForest2,
        PhilosophersForest3,
        PhilosophersForest4,
        PhilosophersForest5,
        Hole4,
        Hole3,
        Hole2,
        Hole1
    }
    public enum SplitName {
        [Description("Manual Split (Not Automatic)"), ToolTip("Does not split automatically. Use this for custom splits not yet defined.")]
        ManualSplit,

        [Description("Redcrown Woods"), ToolTip("Splits when leaving Redcrown Woods")]
        RedcrownWoods,
        [Description("Colossal Drain"), ToolTip("Splits when leaving Colossal Drain")]
        ColossalDrain,
        [Description("False Kings Keep"), ToolTip("Splits when leaving False Kings Keep")]
        FalseKingsKeep,
        [Description("Bargainburg"), ToolTip("Splits when leaving Bargainburg")]
        Bargainburg,
        [Description("Great Frontier"), ToolTip("Splits when leaving Great Frontier")]
        GreatFrontier,
        [Description("Stormwall Pass"), ToolTip("Splits when leaving Stormwall Pass")]
        StormwallPass,
        [Description("Chapel Perilous"), ToolTip("Splits when leaving Chapel Perilous")]
        ChapelPerilous,
        [Description("Blue Ruin"), ToolTip("Splits when leaving Blue Ruin")]
        BlueRuin,

        [Description("Brightcrown Woods"), ToolTip("Splits when leaving Brightcrown Woods")]
        BrightcrownWoods,
        [Description("Colossal Dungeon"), ToolTip("Splits when leaving Colossal Dungeon")]
        ColossalDungeon,
        [Description("False Kings Castle"), ToolTip("Splits when leaving False Kings Castle")]
        FalseKingsCastle,
        [Description("Underburg"), ToolTip("Splits when leaving Underburg")]
        Underburg,
        [Description("Lost Frontier"), ToolTip("Splits when leaving Lost Frontier")]
        LostFrontier,
        [Description("Hidden Kingdom"), ToolTip("Splits when leaving Hidden Kingdom")]
        HiddenKingdom,
        [Description("Black Sanctum"), ToolTip("Splits when leaving Black Sanctum")]
        BlackSanctum,
        [Description("Deep Ruin"), ToolTip("Splits when leaving Deep Ruin")]
        DeepRuin,

        [Description("PhilosophersForest"), ToolTip("Splits when leaving Philosophers Forest")]
        PhilosophersForest,
        [Description("Bog"), ToolTip("Splits when leaving Bog")]
        Bog,
        [Description("Moulding Manor"), ToolTip("Splits when leaving Moulding Manor")]
        MouldingManor,
        [Description("Bugstalk"), ToolTip("Splits when leaving Bugstalk")]
        Bugstalk,
        [Description("House Of Nine Lives"), ToolTip("Splits when leaving House Of Nine Lives")]
        HouseOfNineLives,
        [Description("Phantom Tower"), ToolTip("Splits when leaving Phantom Tower")]
        PhantomTower,
        [Description("Halted Ruin"), ToolTip("Splits when leaving Halted Ruin")]
        HaltedRuin,

        [Description("End Game (Normal)"), ToolTip("Splits when ending the main game")]
        EndGameMain,
        [Description("End Game (New Babe)"), ToolTip("Splits when ending the new babe game")]
        EndGameNew,
        [Description("End Game (Ghost Babe)"), ToolTip("Splits when ending the ghost babe game")]
        EndGameGhost
    }
    public class ToolTipAttribute : Attribute {
        public string ToolTip { get; set; }
        public ToolTipAttribute(string text) {
            ToolTip = text;
        }
    }
}