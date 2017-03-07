#region ================== Namespaces

using System.Windows.Forms;
using mxd.DukeBuilder.Config;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.Controls
{
	internal partial class InfoPanelsControl : UserControl
	{
		#region ================== Constants

		private const int EXPANDED_HEIGHT = 100;

		#endregion

		#region ================== Variables

		private MapElement lastinfoobject;

		#endregion

		#region ================== Properties

		public bool IsExpanded { get { return this.Height == EXPANDED_HEIGHT; } }

		#endregion

		#region ================== Constructor

		public InfoPanelsControl()
		{
			InitializeComponent();
		}

		#endregion

		#region ================== Methods

		internal void Toggle()
		{
			if(IsExpanded)
			{
				this.Height = buttontoggleinfo.Height + buttontoggleinfo.Top;
				buttontoggleinfo.Text = "5";	// Arrow up
				HideInfo();
				labelcollapsedinfo.Visible = true;
#if DEBUG
				console.Visible = false;
#endif
			}
			else
			{
				this.Height = EXPANDED_HEIGHT;
				buttontoggleinfo.Text = "6";	// Arrow down
				labelcollapsedinfo.Visible = false;
				RefreshInfo();
			}
		}

		// This displays the current mode name
		public void DisplayModeName(string name)
		{
			if(lastinfoobject == null)
			{
				labelcollapsedinfo.Text = name;
				labelcollapsedinfo.Refresh();
			}
			modename.Text = name;
			modename.Refresh();
		}

		public void UpdateStatistics() { statistics.UpdateStatistics(); }
		public void RefreshInfo() { ShowInfo(lastinfoobject); }
		public void ShowInfo(MapElement obj)
		{
			if(obj is Vertex) ShowVertexInfo(obj as Vertex);
			else if(obj is Linedef) ShowLineInfo(obj as Linedef);
			else if(obj is Sidedef) ShowWallInfo(obj as Sidedef);
			else if(obj is Sector) ShowSectorInfo(obj as Sector);
			else if(obj is Thing) ShowSpriteInfo(obj as Thing);
			else HideInfo();
		}

		// This hides all info panels
		public void HideInfo()
		{
			// Hide them all
			lastinfoobject = null;
			if(lineinfo.Visible) lineinfo.Hide();
			if(wallinfo.Visible) wallinfo.Hide();
			if(vertexinfo.Visible) vertexinfo.Hide();
			if(sectorinfo.Visible) sectorinfo.Hide();
			if(spriteinfo.Visible) spriteinfo.Hide();
			labelcollapsedinfo.Text = modename.Text;
			labelcollapsedinfo.Refresh();

			bool showmodename = (General.Map != null && IsExpanded);
#if DEBUG
			console.Visible = true;
#else
			modename.Visible = showmodename;
			modename.Refresh();
#endif
			statistics.Visible = showmodename;
		}

		// Show line info
		private void ShowLineInfo(Linedef l)
		{
			if(l.IsDisposed)
			{
				HideInfo();
				return;
			}

			lastinfoobject = l;
			modename.Visible = false;
			if(vertexinfo.Visible) vertexinfo.Hide();
			if(wallinfo.Visible) wallinfo.Hide();
			if(sectorinfo.Visible) sectorinfo.Hide();
			if(spriteinfo.Visible) sectorinfo.Hide();
			if(IsExpanded)
			{
#if DEBUG
				console.Visible = console.AlwaysOnTop;
#endif
				lineinfo.ShowInfo(l);
			}

			// Show info on collapsed label
			labelcollapsedinfo.Text = "Line " + l.Index;
			labelcollapsedinfo.Refresh();
		}

		// Show wall info
		private void ShowWallInfo(Sidedef w)
		{
			if(w.IsDisposed)
			{
				HideInfo();
				return;
			}

			lastinfoobject = w;
			modename.Visible = false;
			if(vertexinfo.Visible) vertexinfo.Hide();
			if(lineinfo.Visible) lineinfo.Hide();
			if(sectorinfo.Visible) sectorinfo.Hide();
			if(spriteinfo.Visible) spriteinfo.Hide();
			if(IsExpanded)
			{
#if DEBUG
				console.Visible = console.AlwaysOnTop;
#endif
				wallinfo.ShowInfo(w);
			}

			// Show info on collapsed label
			labelcollapsedinfo.Text = "Wall " + w.Index + " (HiTag: " + w.HiTag + ", LoTag: " + w.LoTag + ")";
			labelcollapsedinfo.Refresh();
		}

		// Show vertex info
		private void ShowVertexInfo(Vertex v)
		{
			if(v.IsDisposed)
			{
				HideInfo();
				return;
			}

			lastinfoobject = v;
			modename.Visible = false;
			if(lineinfo.Visible) lineinfo.Hide();
			if(wallinfo.Visible) wallinfo.Hide();
			if(sectorinfo.Visible) sectorinfo.Hide();
			if(spriteinfo.Visible) spriteinfo.Hide();
			if(IsExpanded)
			{
#if DEBUG
				console.Visible = console.AlwaysOnTop;
#endif
				vertexinfo.ShowInfo(v);
			}

			// Show info on collapsed label
			labelcollapsedinfo.Text = v.Position.x.ToString("0.##") + ", " + v.Position.y.ToString("0.##");
			labelcollapsedinfo.Refresh();
		}

		// Show sector info
		private void ShowSectorInfo(Sector s)
		{
			if(s.IsDisposed)
			{
				HideInfo();
				return;
			}

			lastinfoobject = s;
			modename.Visible = false;
			if(lineinfo.Visible) lineinfo.Hide();
			if(wallinfo.Visible) wallinfo.Hide();
			if(vertexinfo.Visible) vertexinfo.Hide();
			if(spriteinfo.Visible) spriteinfo.Hide();
			if(IsExpanded)
			{
#if DEBUG
				console.Visible = console.AlwaysOnTop;
#endif
				sectorinfo.ShowInfo(s);
			}

			// Show info on collapsed label
			if(General.Map.Config.SectorEffects.ContainsKey(s.LoTag))
				labelcollapsedinfo.Text = General.Map.Config.SectorEffects[s.LoTag].ToString();
			else
				labelcollapsedinfo.Text = "Sector " + s.Index +  " (HiTag: " + s.HiTag + ", LoTag: " + s.LoTag + ")";

			labelcollapsedinfo.Refresh();
		}

		// Show thing info
		private void ShowSpriteInfo(Thing s)
		{
			if(s.IsDisposed)
			{
				HideInfo();
				return;
			}

			lastinfoobject = s;
			modename.Visible = false;
			if(lineinfo.Visible) lineinfo.Hide();
			if(wallinfo.Visible) wallinfo.Hide();
			if(vertexinfo.Visible) vertexinfo.Hide();
			if(sectorinfo.Visible) sectorinfo.Hide();
			if(IsExpanded)
			{
#if DEBUG
				console.Visible = console.AlwaysOnTop;
#endif
				spriteinfo.ShowInfo(s);
			}

			// Show info on collapsed label
			SpriteInfo ti = General.Map.Data.GetSpriteInfoEx(s.TileIndex);
			if(ti != null)
				labelcollapsedinfo.Text = s.TileIndex + " - " + ti.Title + " (HiTag: " + s.HiTag + ", LoTag: " + s.LoTag + ")";
			else
				labelcollapsedinfo.Text = "Sprite " + s.TileIndex + " (HiTag: " + s.HiTag + ", LoTag: " + s.LoTag + ")";

			labelcollapsedinfo.Refresh();
		}

		#endregion

		#region ================== Events

		private void buttontoggleinfo_MouseUp(object sender, MouseEventArgs e)
		{
			General.MainWindow.FocusDisplay();
			General.MainWindow.InvokeTaggedAction(sender, e);
		}

		#endregion
	}
}
