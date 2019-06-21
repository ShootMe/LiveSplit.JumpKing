#if !DebugInfo
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
namespace LiveSplit.JumpKing {
	public class SplitterComponent : IComponent {
		public string ComponentName { get { return "Jump King Autosplitter"; } }
		public TimerModel Model { get; set; }
		public IDictionary<string, Action> ContextMenuControls { get { return null; } }
		private static string LOGFILE = "_JumpKing.txt";
		private Dictionary<LogObject, string> currentValues = new Dictionary<LogObject, string>();
		private SplitterMemory mem;
		private SplitterSettings settings;
		private int currentSplit = -1, lastLogCheck, lastPlayerEntity, lastTimesWon;
		private bool hasLog;
		private Screen lastScreen;
		private float lastGameTime;

		public SplitterComponent(LiveSplitState state) {
			mem = new SplitterMemory();
			settings = new SplitterSettings();
			foreach (LogObject key in Enum.GetValues(typeof(LogObject))) {
				currentValues[key] = "";
			}

			if (state != null) {
				Model = new TimerModel() { CurrentState = state };
				Model.InitializeGameTime();
				Model.CurrentState.IsGameTimePaused = true;
				state.OnReset += OnReset;
				state.OnPause += OnPause;
				state.OnResume += OnResume;
				state.OnStart += OnStart;
				state.OnSplit += OnSplit;
				state.OnUndoSplit += OnUndoSplit;
				state.OnSkipSplit += OnSkipSplit;
			}
		}

		public void GetValues() {
			if (!mem.HookProcess()) { return; }

			if (Model != null) {
				HandleSplits();
			}

			LogValues();
		}
		private void HandleSplits() {
			bool shouldSplit = false;
			float gameTime = mem.GameTime();

			if (currentSplit == -1) {
				int playerEntity = mem.PlayerEntity();
				shouldSplit = lastPlayerEntity != playerEntity && gameTime < 0.1f && DateTime.Now > mem.LastHooked.AddSeconds(5);
				lastPlayerEntity = playerEntity;
			} else if (Model.CurrentState.CurrentPhase == TimerPhase.Running) {
				Screen screen = mem.PlayerScreen();
				int timesWon = mem.TimesWon();

				if (currentSplit < Model.CurrentState.Run.Count && currentSplit < settings.Splits.Count) {
					SplitName split = settings.Splits[currentSplit];
					
					switch (split) {
						case SplitName.RedcrownWoods: shouldSplit = lastScreen != Screen.ColossalDrain1 && screen == Screen.ColossalDrain1; break;
						case SplitName.ColossalDrain: shouldSplit = lastScreen != Screen.FalseKingsKeep1 && screen == Screen.FalseKingsKeep1; break;
						case SplitName.FalseKingsKeep: shouldSplit = lastScreen != Screen.Bargainburg1 && screen == Screen.Bargainburg1; break;
						case SplitName.Bargainburg: shouldSplit = lastScreen != Screen.GreatFrontier1 && screen == Screen.GreatFrontier1; break;
						case SplitName.GreatFrontier: shouldSplit = lastScreen != Screen.WindsweptBluff1 && screen == Screen.WindsweptBluff1; break;
						case SplitName.StormwallPass: shouldSplit = lastScreen != Screen.ChapelPerilous1 && screen == Screen.ChapelPerilous1; break;
						case SplitName.ChapelPerilous: shouldSplit = lastScreen != Screen.BlueRuin1 && screen == Screen.BlueRuin1; break;
						case SplitName.BlueRuin: shouldSplit = lastScreen != Screen.TheTower1 && screen == Screen.TheTower1; break;

						case SplitName.BrightcrownWoods: shouldSplit = lastScreen != Screen.ColossalDungeon1 && screen == Screen.ColossalDungeon1; break;
						case SplitName.ColossalDungeon: shouldSplit = lastScreen != Screen.FalseKingsCastle1 && screen == Screen.FalseKingsCastle1; break;
						case SplitName.FalseKingsCastle: shouldSplit = lastScreen != Screen.Underburg1 && screen == Screen.Underburg1; break;
						case SplitName.Underburg: shouldSplit = lastScreen != Screen.LostFrontier1 && screen == Screen.LostFrontier1; break;
						case SplitName.LostFrontier: shouldSplit = lastScreen != Screen.HiddenKingdom1 && screen == Screen.HiddenKingdom1; break;
						case SplitName.HiddenKingdom: shouldSplit = lastScreen != Screen.BlackSanctum1 && screen == Screen.BlackSanctum1; break;
						case SplitName.BlackSanctum: shouldSplit = lastScreen != Screen.DeepRuin1 && screen == Screen.DeepRuin1; break;
						case SplitName.DeepRuin: shouldSplit = lastScreen != Screen.TheDarkTower1 && screen == Screen.TheDarkTower1; break;

						case SplitName.EndGameMain: shouldSplit = screen == Screen.MainBabe && lastTimesWon < timesWon; break;
						case SplitName.EndGameNew: shouldSplit = screen == Screen.NewBabe && lastTimesWon < timesWon; break;
					}
				} else {
					shouldSplit = (screen == Screen.MainBabe || screen == Screen.NewBabe) && lastTimesWon < timesWon;
				}

				lastScreen = screen;
				lastTimesWon = timesWon;
			}

			if (gameTime > 0 || lastGameTime == gameTime) {
				Model.CurrentState.SetGameTime(TimeSpan.FromSeconds(gameTime));
			}

			Model.CurrentState.IsGameTimePaused = Model.CurrentState.CurrentPhase != TimerPhase.Running || gameTime == lastGameTime;

			lastGameTime = gameTime;

			HandleSplit(shouldSplit, false);
		}
		private void HandleSplit(bool shouldSplit, bool shouldReset = false) {
			if (shouldReset) {
				if (currentSplit >= 0) {
					Model.Reset();
				}
			} else if (shouldSplit) {
				if (currentSplit == -1) {
					Model.Start();
				} else {
					Model.Split();
				}
			}
		}
		private void LogValues() {
			if (lastLogCheck == 0) {
				hasLog = File.Exists(LOGFILE);
				lastLogCheck = 300;
			}
			lastLogCheck--;

			if (hasLog || !Console.IsOutputRedirected) {
				string prev = string.Empty, curr = string.Empty;
				foreach (LogObject key in Enum.GetValues(typeof(LogObject))) {
					prev = currentValues[key];

					switch (key) {
						case LogObject.CurrentSplit: curr = currentSplit.ToString(); break;
						case LogObject.Pointers: curr = mem.RAMPointers(); break;
						case LogObject.PointerVersion: curr = mem.RAMPointerVersion(); break;
						case LogObject.PlayerEntity: curr = mem.PlayerEntity().ToString("X"); break;
						case LogObject.PlayerScreen: curr = mem.PlayerScreen().ToString(); break;
						case LogObject.TimesWon: curr = mem.TimesWon().ToString(); break;
						//case LogObject.GameTime: curr = mem.GameTime().ToString("0"); break;
						default: curr = string.Empty; break;
					}

					if (string.IsNullOrEmpty(prev)) { prev = string.Empty; }
					if (string.IsNullOrEmpty(curr)) { curr = string.Empty; }
					if (!prev.Equals(curr)) {
						WriteLog(DateTime.Now.ToString(@"HH\:mm\:ss.fff") + (Model != null ? " | " + Model.CurrentState.CurrentTime.RealTime.Value.ToString("G").Substring(3, 11) : "") + ": " + key.ToString() + ": ".PadRight(16 - key.ToString().Length, ' ') + prev.PadLeft(25, ' ') + " -> " + curr);

						currentValues[key] = curr;
					}
				}
			}
		}

		public void Update(IInvalidator invalidator, LiveSplitState lvstate, float width, float height, LayoutMode mode) {
			GetValues();
		}
		public void OnReset(object sender, TimerPhase e) {
			currentSplit = -1;
			lastPlayerEntity = mem.PlayerEntity();
			Model.CurrentState.IsGameTimePaused = true;
			WriteLog("---------Reset----------------------------------");
		}
		public void OnResume(object sender, EventArgs e) {
			WriteLog("---------Resumed--------------------------------");
		}
		public void OnPause(object sender, EventArgs e) {
			WriteLog("---------Paused---------------------------------");
		}
		public void OnStart(object sender, EventArgs e) {
			currentSplit = 0;
			Model.CurrentState.IsGameTimePaused = true;
			WriteLog("---------New Game " + Assembly.GetExecutingAssembly().GetName().Version.ToString(3) + "-------------------------");
		}
		public void OnUndoSplit(object sender, EventArgs e) {
			currentSplit--;
			WriteLog("---------Undo-----------------------------------");
		}
		public void OnSkipSplit(object sender, EventArgs e) {
			currentSplit++;
			WriteLog("---------Skip-----------------------------------");
		}
		public void OnSplit(object sender, EventArgs e) {
			currentSplit++;
			WriteLog("---------Split-----------------------------------");
			if (currentSplit == Model.CurrentState.Run.Count) {
				ISegment segment = Model.CurrentState.Run[currentSplit - 1];
				segment.SplitTime = new Time(segment.SplitTime.RealTime, TimeSpan.FromSeconds(lastGameTime));
			}
		}
		private void WriteLog(string data) {
			if (hasLog || !Console.IsOutputRedirected) {
				if (Console.IsOutputRedirected) {
					using (StreamWriter wr = new StreamWriter(LOGFILE, true)) {
						wr.WriteLine(data);
					}
				} else {
					Console.WriteLine(data);
				}
			}
		}

		public Control GetSettingsControl(LayoutMode mode) { return settings; }
		public void SetSettings(XmlNode document) { settings.SetSettings(document); }
		public XmlNode GetSettings(XmlDocument document) { return settings.UpdateSettings(document); }
		public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion) { }
		public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion) { }
		public float HorizontalWidth { get { return 0; } }
		public float MinimumHeight { get { return 0; } }
		public float MinimumWidth { get { return 0; } }
		public float PaddingBottom { get { return 0; } }
		public float PaddingLeft { get { return 0; } }
		public float PaddingRight { get { return 0; } }
		public float PaddingTop { get { return 0; } }
		public float VerticalHeight { get { return 0; } }
		public void Dispose() {
			if (Model != null) {
				Model.CurrentState.OnReset -= OnReset;
				Model.CurrentState.OnPause -= OnPause;
				Model.CurrentState.OnResume -= OnResume;
				Model.CurrentState.OnStart -= OnStart;
				Model.CurrentState.OnSplit -= OnSplit;
				Model.CurrentState.OnUndoSplit -= OnUndoSplit;
				Model.CurrentState.OnSkipSplit -= OnSkipSplit;
			}
		}
	}
}
#endif