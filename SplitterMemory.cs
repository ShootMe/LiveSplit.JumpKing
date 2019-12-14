using LiveSplit.Memory;
using System;
using System.Diagnostics;
namespace LiveSplit.JumpKing {
    //.load C:\Windows\Microsoft.NET\Framework\v4.0.30319\SOS.dll
    public partial class SplitterMemory {
        private static ProgramPointer SaveManager = new ProgramPointer(AutoDeref.Single, new ProgramSignature(PointerVersion.Steam, "558BEC57565383EC24894DD0A1????????38008B7DD083C724", 13));
        private static ProgramPointer IStatInfo = new ProgramPointer(AutoDeref.Single, new ProgramSignature(PointerVersion.Steam, "558BEC57568B412C85C0741E83F801752B8B35", 19));
        private static ProgramPointer Camera = new ProgramPointer(AutoDeref.None, new ProgramSignature(PointerVersion.Steam, "558BEC833D????????007D0833D28915????????8B15", 22));
        public Process Program { get; set; }
        public bool IsHooked { get; set; } = false;
        public DateTime LastHooked;

        public SplitterMemory() {
            LastHooked = DateTime.MinValue;
        }
        public string RAMPointers() {
            return SaveManager.GetPointer(Program).ToString("X");
        }
        public string RAMPointerVersion() {
            return SaveManager.Version.ToString();
        }
        public int PlayerEntity() {
            //SaveManager.instance.m_player
            return SaveManager.Read<int>(Program, 0x0, 0x4);
        }
        public void TeleportPlayer(Screen screen, float x, float y) {
            SaveManager.Write<float>(Program, x, 0x0, 0x4, 0x10, 0x2c);
            SaveManager.Write<float>(Program, y, 0x0, 0x4, 0x10, 0x30);
            Camera.Write<int>(Program, (int)screen, 0x0, 0x0);
        }
        public Screen PlayerScreen() {
            //SaveManager.instance.m_player.m_body.m_last_screen
            return (Screen)SaveManager.Read<int>(Program, 0x0, 0x4, 0x10, 0x14);
        }
        public float PlayerX() {
            //SaveManager.instance.m_player.m_body.position.X
            return SaveManager.Read<float>(Program, 0x0, 0x4, 0x10, 0x2c);
        }
        public float PlayerY() {
            //SaveManager.instance.m_player.m_body.position.Y
            return SaveManager.Read<float>(Program, 0x0, 0x4, 0x10, 0x30);
        }
        public float GameTime() {
            //AchievementManager.instance.m_all_time_stats._ticks
            int allTime = IStatInfo.Read<int>(Program, 0x0, 0x3c);
            //AchievementManager.instance.m_snapshot._ticks
            int snapshot = IStatInfo.Read<int>(Program, 0x0, 0x20);
            return (allTime - snapshot) * 0.017f;
        }
        public int TimesWon() {
            //AchievementManager.instance.m_all_time_stats.times_won
            return IStatInfo.Read<int>(Program, 0x0, 0x38);
        }
        public bool HookProcess() {
            IsHooked = Program != null && !Program.HasExited;
            if (!IsHooked && DateTime.Now > LastHooked.AddSeconds(1)) {
                LastHooked = DateTime.Now;
                Process[] processes = Process.GetProcessesByName("Jumpking");
                Program = processes != null && processes.Length > 0 ? processes[0] : null;

                if (Program != null && !Program.HasExited) {
                    MemoryReader.Update64Bit(Program);
                    IsHooked = true;
                }
            }

            return IsHooked;
        }
        public void Dispose() {
            if (Program != null) {
                Program.Dispose();
            }
        }
    }
}