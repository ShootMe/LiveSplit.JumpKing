using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
namespace LiveSplit.JumpKing {
	public partial class SplitterSettings : UserControl {
		public List<SplitName> Splits { get; private set; }
		private bool isLoading;
		public SplitterSettings() {
			isLoading = true;
			InitializeComponent();

			Splits = new List<SplitName>();

			isLoading = false;
		}

		private void Settings_Load(object sender, EventArgs e) {
			FindForm().Text = "Jump King Autosplitter v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
			LoadSettings();
		}
		public void LoadSettings() {
			isLoading = true;
			this.flowMain.SuspendLayout();

			for (int i = flowMain.Controls.Count - 1; i > 0; i--) {
				flowMain.Controls.RemoveAt(i);
			}

			foreach (SplitName split in Splits) {
				SplitterSplitSettings setting = new SplitterSplitSettings();
				setting.cboSplit.DataSource = GetAvailableDescriptions<SplitName>();
				setting.cboSplit.Text = SplitterSplitSettings.GetEnumDescription<SplitName>(split);
				AddHandlers(setting);

				flowMain.Controls.Add(setting);
			}

			isLoading = false;
			this.flowMain.ResumeLayout(true);
		}
		private void AddHandlers(SplitterSplitSettings setting) {
			setting.cboSplit.SelectedIndexChanged += new EventHandler(ControlChanged);
			setting.btnRemove.Click += new EventHandler(btnRemove_Click);
		}
		private void RemoveHandlers(SplitterSplitSettings setting) {
			setting.cboSplit.SelectedIndexChanged -= ControlChanged;
			setting.btnRemove.Click -= btnRemove_Click;
		}
		public void btnRemove_Click(object sender, EventArgs e) {
			for (int i = flowMain.Controls.Count - 1; i > 0; i--) {
				if (flowMain.Controls[i].Contains((Control)sender)) {
					RemoveHandlers((SplitterSplitSettings)((Button)sender).Parent);

					flowMain.Controls.RemoveAt(i);
					break;
				}
			}
			UpdateSplits();
		}
		public void ControlChanged(object sender, EventArgs e) {
			UpdateSplits();
		}
		public void UpdateSplits() {
			if (isLoading) return;

			Splits.Clear();
			foreach (Control control in flowMain.Controls) {
				if (control is SplitterSplitSettings) {
					SplitterSplitSettings setting = (SplitterSplitSettings)control;
					if (!string.IsNullOrEmpty(setting.cboSplit.Text)) {
						Splits.Add(SplitterSplitSettings.GetEnumValue<SplitName>(setting.cboSplit.Text));
					}
				}
			}
		}
		public XmlNode UpdateSettings(XmlDocument document) {
			XmlElement xmlSettings = document.CreateElement("Settings");

			XmlElement xmlSplits = document.CreateElement("Splits");
			xmlSettings.AppendChild(xmlSplits);

			foreach (SplitName split in Splits) {
				XmlElement xmlSplit = document.CreateElement("Split");
				xmlSplit.InnerText = split.ToString();
				xmlSplits.AppendChild(xmlSplit);
			}

			return xmlSettings;
		}
		public void SetSettings(XmlNode settings) {
			Splits.Clear();
			XmlNodeList splitNodes = settings.SelectNodes(".//Splits/Split");
			foreach (XmlNode splitNode in splitNodes) {
				string splitDescription = splitNode.InnerText;
				SplitName split = SplitterSplitSettings.GetEnumValue<SplitName>(splitDescription);
				Splits.Add(split);
			}
		}
		private void btnAddSplit_Click(object sender, EventArgs e) {
			SplitterSplitSettings setting = new SplitterSplitSettings();
			List<string> names = GetAvailableDescriptions<SplitName>();
			setting.cboSplit.DataSource = names;
			setting.cboSplit.Text = names[0];
			AddHandlers(setting);

			flowMain.Controls.Add(setting);
			UpdateSplits();
		}
		private void btnNewGame_Click(object sender, EventArgs e) {
			if (Splits.Count > 0 && MessageBox.Show(this, "You already have some splits setup. This will clear anything you have and default in these splits.\r\n\r\nAre you sure you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) {
				return;
			}

			Splits.Clear();
			Splits.Add(SplitName.RedcrownWoods);
			Splits.Add(SplitName.ColossalDrain);
			Splits.Add(SplitName.FalseKingsKeep);
			Splits.Add(SplitName.Bargainburg);
			Splits.Add(SplitName.GreatFrontier);
			Splits.Add(SplitName.StormwallPass);
			Splits.Add(SplitName.ChapelPerilous);
			Splits.Add(SplitName.BlueRuin);
			Splits.Add(SplitName.EndGameMain);

			LoadSettings();
		}
		private void btnNewBabe_Click(object sender, EventArgs e) {
			if (Splits.Count > 0 && MessageBox.Show(this, "You already have some splits setup. This will clear anything you have and default in these splits.\r\n\r\nAre you sure you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) {
				return;
			}

			Splits.Clear();
			Splits.Add(SplitName.BrightcrownWoods);
			Splits.Add(SplitName.ColossalDungeon);
			Splits.Add(SplitName.FalseKingsCastle);
			Splits.Add(SplitName.Underburg);
			Splits.Add(SplitName.LostFrontier);
			Splits.Add(SplitName.HiddenKingdom);
			Splits.Add(SplitName.BlackSanctum);
			Splits.Add(SplitName.DeepRuin);
			Splits.Add(SplitName.EndGameNew);

			LoadSettings();
		}
		private void btnGhostBabe_Click(object sender, EventArgs e) {
			if (Splits.Count > 0 && MessageBox.Show(this, "You already have some splits setup. This will clear anything you have and default in these splits.\r\n\r\nAre you sure you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) {
				return;
			}

			Splits.Clear();
			Splits.Add(SplitName.RedcrownWoods);
			Splits.Add(SplitName.PhilosophersForest);
			Splits.Add(SplitName.Bog);
			Splits.Add(SplitName.MouldingManor);
			Splits.Add(SplitName.Bugstalk);
			Splits.Add(SplitName.HouseOfNineLives);
			Splits.Add(SplitName.PhantomTower);
			Splits.Add(SplitName.HaltedRuin);
			Splits.Add(SplitName.EndGameGhost);

			LoadSettings();
		}
		private List<string> GetAvailableDescriptions<T>() where T : struct {
			List<string> values = new List<string>();
			foreach (T value in Enum.GetValues(typeof(T))) {
				string name = value.ToString();
				MemberInfo[] infos = typeof(T).GetMember(name);
				DescriptionAttribute[] descriptions = null;
				if (infos != null && infos.Length > 0) {
					descriptions = (DescriptionAttribute[])infos[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
				}
				if (descriptions != null && descriptions.Length > 0) {
					values.Add(descriptions[0].Description);
				} else {
					values.Add(name);
				}
			}
			return values;
		}
		private void flowMain_DragDrop(object sender, DragEventArgs e) {
			UpdateSplits();
		}
		private void flowMain_DragEnter(object sender, DragEventArgs e) {
			e.Effect = DragDropEffects.Move;
		}
		private void flowMain_DragOver(object sender, DragEventArgs e) {
			SplitterSplitSettings data = (SplitterSplitSettings)e.Data.GetData(typeof(SplitterSplitSettings));
			FlowLayoutPanel destination = (FlowLayoutPanel)sender;
			Point p = destination.PointToClient(new Point(e.X, e.Y));
			var item = destination.GetChildAtPoint(p);
			int index = destination.Controls.GetChildIndex(item, false);
			if (index == 0) {
				e.Effect = DragDropEffects.None;
			} else {
				e.Effect = DragDropEffects.Move;
				int oldIndex = destination.Controls.GetChildIndex(data);
				if (oldIndex != index) {
					destination.Controls.SetChildIndex(data, index);
					destination.Invalidate();
				}
			}
		}
	}
}