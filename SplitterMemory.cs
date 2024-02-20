﻿using LiveSplit.Memory;
using System;
using System.Diagnostics;
namespace LiveSplit.JumpKing {
    //.load C:\Windows\Microsoft.NET\Framework\v4.0.30319\SOS.dll
    public partial class SplitterMemory {
        private static ProgramPointer SaveManager = new ProgramPointer(AutoDeref.Single, new ProgramSignature(PointerVersion.Steam, "558BEC57565383EC24894DD0A1????????38008B7DD083C72C8B35", 13));
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
            byte[] data = new byte[16];
            byte[] temp = BitConverter.GetBytes(x);
            Array.Copy(temp, 0, data, 0, 4);
            temp = BitConverter.GetBytes(y);
            Array.Copy(temp, 0, data, 4, 4);
            temp = BitConverter.GetBytes(0.26f);
            Array.Copy(temp, 0, data, 12, 4);

            //SaveManager.instance.m_player.m_body.position.X/Y & .velocity.X/Y
            SaveManager.Write(Program, data, 0x0, 0x4, 0xc, 0x3c);
            Camera.Write<int>(Program, (int)screen, 0x0, 0x0);
        }

        // SaveManager.instance.
        // m_player         004
        // m_body           00c

        // Position         03c
        // Velocity         044

        public Screen PlayerScreen() {
            // Camera.CurrentScreenIndex1
            return (Screen)Camera.Read<int>(Program, 0x0, 0x0);
        }
        public float PlayerX() {
            //SaveManager.instance.m_player.m_body.position.X
            return SaveManager.Read<float>(Program, 0x0, 0x4, 0xc, 0x3c);
        }
        public float PlayerY() {
            //SaveManager.instance.m_player.m_body.position.Y
            return SaveManager.Read<float>(Program, 0x0, 0x4, 0xc, 0x40);
        }
        public float GameTime() {
            //AchievementManager.instance.m_all_time_stats._ticks (60 = 38 + 28)
            int allTime = IStatInfo.Read<int>(Program, 0x0, 0x60);
            //AchievementManager.instance.m_snapshot._ticks (30 = 8 + 28)
            int snapshot = IStatInfo.Read<int>(Program, 0x0, 0x30);
            return (allTime - snapshot) * 0.017f;
        }
        public int TimesWon() {
            //AchievementManager.instance.m_all_time_stats.times_won (5C = 38 + 24)
            return IStatInfo.Read<int>(Program, 0x0, 0x5C);
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