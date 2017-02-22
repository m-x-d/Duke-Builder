#region ================== Namespaces

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace mxd.DukeBuilder.Controls
{
	//mxd. Based on (but heavily reworked since) TextureListPanel from Sledge (https://github.com/LogicAndTrick/sledge)
	internal class ImageBrowserPanel : Panel
	{
		#region ================== Variables

		private VScrollBar scrollbar;
		private List<ImageBrowserItem> items;
		private List<ImageBrowserItem> selection;
		private List<Rectangle> rectangles;
		private ImageBrowserItem lastselecteditem;
		private int imagesize;
		private bool showimagesizes;

		private static int fontheight = 4 + SystemFonts.MessageBoxFont.Height; // Cache to improve performance...

		//mxd. Tooltips
		private ToolTip tooltip;
		private Point lasttooltippos;
		private const int tooltipreshowdistance = 48;

		// Selection
		private bool allowselection;
		private bool allowmultipleselection;

		#endregion

		#region ================== Event handlers

		public delegate void ItemSelectedEventHandler(object sender, ImageBrowserItem item);
		public delegate void SelectionChangedEventHandler(object sender, List<ImageBrowserItem> selection);

		/*public event ItemSelectedEventHandler ItemSelected;
		private void OnItemSelected(ImageBrowserItem item)
		{
			if(ItemSelected != null) ItemSelected(this, item);
		}*/

		public event SelectionChangedEventHandler SelectionChanged;
		private void OnSelectionChanged(List<ImageBrowserItem> selection)
		{
			if(SelectionChanged != null) SelectionChanged(this, selection);
		}

		public event ItemSelectedEventHandler ItemDoubleClicked;
		private void OnItemDoubleClicked(ImageBrowserItem item)
		{
			if(ItemDoubleClicked != null) ItemDoubleClicked(this, item);
		}

		#endregion

		#region ================== Properties

		public bool HideSelection
		{
			get { return !allowselection; }
			set
			{
				allowselection = !value;
				if(!allowselection && selection.Count > 0)
				{
					selection.Clear();
					Refresh();
				}
			}
		}

		public bool MultiSelect
		{
			get { return allowmultipleselection; }
			set
			{
				allowmultipleselection = value;
				if(!allowmultipleselection && selection.Count > 0)
				{
					var first = selection[0];
					selection.Clear();
					selection.Add(first);
					Refresh();
				}
			}
		}

		public int ImageSize
		{
			get { return imagesize; }
			set
			{
				imagesize = value;
				UpdateRectangles();
				if(selection.Count > 0) ScrollToItem(selection[0]);
			}
		}

		public bool ShowImageSizes { get { return showimagesizes; } set { showimagesizes = value; } }

		public List<ImageBrowserItem> Items { get { return items; } }
		public List<ImageBrowserItem> SelectedItems { get { return selection; } }

		#endregion

		#region ================== Constructor / Disposer

		public ImageBrowserPanel()
		{
			VScroll = true;
			AutoScroll = true;
			DoubleBuffered = true;

			scrollbar = new VScrollBar { Dock = DockStyle.Right };
			scrollbar.ValueChanged += delegate { Refresh(); };
			tooltip = new ToolTip(); //mxd
			items = new List<ImageBrowserItem>();
			selection = new List<ImageBrowserItem>();
			imagesize = 128;
			rectangles = new List<Rectangle>();

			Controls.Add(scrollbar);
		}

		protected override void Dispose(bool disposing)
		{
			if(disposing) Clear();
			base.Dispose(disposing);
		}

		#endregion

		#region ================== Add/Remove/Get Textures

		//mxd. Clears the list without redrawing it
		public void Clear()
		{
			selection.Clear();
			items.Clear();
			lastselecteditem = null;
			rectangles.Clear();
		}

		//mxd
		public void ClearSelection()
		{
			selection.Clear();
			lastselecteditem = null;

			OnSelectionChanged(selection);
			Refresh();
		}

		public void SetItems(IEnumerable<ImageBrowserItem> items)
		{
			this.items.Clear();
			lastselecteditem = null;
			selection.Clear();
			this.items.AddRange(items);

			OnSelectionChanged(selection);
			UpdateRectangles();
		}

		public void SetSelectedItem(ImageBrowserItem item)
		{
			SetSelectedItems(new List<ImageBrowserItem> { item } );
		}

		public void SetSelectedItems(List<ImageBrowserItem> items)
		{
			selection.Clear();
			if(items.Count > 0)
			{
				selection.AddRange(items);
				ScrollToItem(items[0]); //mxd
				Refresh(); //mxd
			}
			OnSelectionChanged(selection);
		}

		public void ScrollToItem(ImageBrowserItem item)
		{
			int index = items.IndexOf(item);
			if(index < 0) return;

			Rectangle rec = rectangles[index];

			//mxd. Already visible?
			int ymin = scrollbar.Value;
			int ymax = ymin + this.ClientRectangle.Height;
			if(rec.Top - 3 >= ymin && rec.Bottom + 3 <= ymax) return;

			int yscroll = Math.Max(0, Math.Min(rec.Top - 3, scrollbar.Maximum - ClientRectangle.Height));
			scrollbar.Value = yscroll;
			Refresh();
		}

		public void SelectNextItem(SearchDirectionHint dir)
		{
			if(!allowselection) return;

			if(selection.Count == 0)
			{
				if(items.Count > 0) SetSelectedItem(items[0]);
				return;
			}

			int targetindex = items.IndexOf(selection[0]);
			Rectangle rect = rectangles[targetindex];
			int index, newindex, tx, cx, cy;

			switch(dir)
			{
				case SearchDirectionHint.Right:
					// Just select the next item
					if(targetindex < items.Count - 1) SetSelectedItem(items[targetindex + 1]);
					break;

				case SearchDirectionHint.Left:
					// Just select the previous item
					if(targetindex > 0) SetSelectedItem(items[targetindex - 1]);
					break;

				case SearchDirectionHint.Up:
					// Skip current row...
					index = targetindex - 1;
					if(index < 0) break;
					while(index > -1)
					{
						if(rectangles[index].Y != rect.Y) break;
						index--;
					}

					// Check upper row for best match
					tx = rect.X + rect.Width / 2;
					cx = int.MaxValue;
					cy = rectangles[index].Y;
					newindex = int.MaxValue;

					while(index > -1 && rectangles[index].Y == cy)
					{
						int ccx = Math.Abs(rectangles[index].X + rectangles[index].Width / 2 - tx);
						if(ccx < cx)
						{
							cx = ccx;
							newindex = index;
						}
						index--;
					}

					// Select item
					if(newindex != int.MaxValue) SetSelectedItem(items[newindex]);
					break;

				case SearchDirectionHint.Down:
					// Skip current row...
					index = targetindex + 1;
					if(index > rectangles.Count - 1) break;
					while(index < rectangles.Count)
					{
						if(rectangles[index].Y != rect.Y) break;
						index++;
					}

					// Check upper row for best match
					tx = rect.X + rect.Width / 2;
					cx = int.MaxValue;
					cy = rectangles[index].Y;
					newindex = int.MaxValue;

					while(index < rectangles.Count && rectangles[index].Y == cy)
					{
						int ccx = Math.Abs(rectangles[index].X + rectangles[index].Width / 2 - tx);
						if(ccx < cx)
						{
							cx = ccx;
							newindex = index;
						}
						index++;
					}

					// Select item
					if(newindex != int.MaxValue) SetSelectedItem(items[newindex]);
					break;
			}
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);
			if(General.Interface.CtrlState || General.Interface.ShiftState || selection.Count != 1)
				return;

			int index = GetIndexAt(e.X, scrollbar.Value + e.Y);
			if(index == -1) return;

			OnItemDoubleClicked(items[index]);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.Focus();

			if(!allowselection) return;
			if(!allowmultipleselection || !General.Interface.CtrlState)
				selection.Clear();

			int x = e.X;
			int y = scrollbar.Value + e.Y;

			int clickedIndex = GetIndexAt(x, y);
			var item = (clickedIndex >= 0 && clickedIndex < items.Count ? items[clickedIndex] : null);

			if(item == null)
			{
				selection.Clear();
			}
			else if(allowmultipleselection && General.Interface.CtrlState && selection.Contains(item))
			{
				selection.Remove(item);
				lastselecteditem = null;
			}
			else if(allowmultipleselection && General.Interface.ShiftState && lastselecteditem != null)
			{
				int bef = items.IndexOf(lastselecteditem);
				var start = Math.Min(bef, clickedIndex);
				var count = Math.Abs(clickedIndex - bef) + 1;
				selection.AddRange(items.GetRange(start, count).Where(i => !selection.Contains(i)));
			}
			else
			{
				selection.Add(item);
				lastselecteditem = item;
			}

			OnSelectionChanged(selection);
			Refresh();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			Focus();
			base.OnMouseEnter(e);
		}

		//mxd
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			int index = GetIndexAt(e.X, scrollbar.Value + e.Y);
			if(index == -1 || string.IsNullOrEmpty(items[index].ToolTip))
			{
				if(tooltip.Active) tooltip.Hide(this);
			}
			else if(!tooltip.Active || tooltip.GetToolTip(this) != items[index].ToolTip
				|| Math.Abs(lasttooltippos.X - e.Location.X) > tooltipreshowdistance
				|| Math.Abs(lasttooltippos.Y - e.Location.Y) > tooltipreshowdistance)
			{
				Point pos = new Point(e.Location.X, e.Location.Y + Cursor.Size.Height + 4);
				tooltip.Show(items[index].ToolTip, this, pos, 999999);
				lasttooltippos = e.Location;
			}
		}

		public int GetIndexAt(int x, int y)
		{
			const int pad = 3;
			for(var i = 0; i < rectangles.Count; i++)
			{
				var rec = rectangles[i];
				if(rec.Left - pad <= x
					&& rec.Right + pad >= x
					&& rec.Top - pad <= y
					&& rec.Bottom + pad + fontheight >= y)
				{
					return i;
				}
			}

			return -1;
		}

		#endregion

		#region ================== Scrolling

		private void ScrollByAmount(int value)
		{
			int newvalue = Math.Max(0, scrollbar.Value + value);
			scrollbar.Value = Math.Min(newvalue, Math.Max(0, scrollbar.Maximum - ClientRectangle.Height));
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			ScrollByAmount(scrollbar.SmallChange * (e.Delta / -120));
		}

		//mxd. Otherwise arrow keys won't be handled by OnKeyDown
		protected override bool IsInputKey(Keys keyData)
		{
			switch(keyData)
			{
				case Keys.Right: case Keys.Left: 
				case Keys.Up: case Keys.Down: 
				case Keys.Return: return true;
			}

			return base.IsInputKey(keyData);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				//mxd. Cursor keys
				case Keys.Left: SelectNextItem(SearchDirectionHint.Left); break;
				case Keys.Right: SelectNextItem(SearchDirectionHint.Right); break;
				case Keys.Up: SelectNextItem(SearchDirectionHint.Up); break;
				case Keys.Down: SelectNextItem(SearchDirectionHint.Down); break;

				case Keys.PageDown: ScrollByAmount(scrollbar.LargeChange); break;
				case Keys.PageUp: ScrollByAmount(-scrollbar.LargeChange); break;
				case Keys.End: ScrollByAmount(int.MaxValue); break;
				case Keys.Home: ScrollByAmount(-int.MaxValue); break;

				case Keys.Enter: if(selection.Count > 0) OnItemDoubleClicked(selection[0]); break;
			}
			
			base.OnKeyDown(e);
		}

		#endregion

		#region ================== Updating Rectangles & Dimensions

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			UpdateRectangles();

			//mxd
			if(selection.Count > 0) ScrollToItem(selection[0]);
		}

		private void UpdateRectangles()
		{
			int w = ClientRectangle.Width - scrollbar.Width;
			const int pad = 2;
			int cx = 0;
			int cy = 0;
			int my = 0;
			rectangles.Clear();

			foreach(var ti in items)
			{
				int rw = w - cx;
	
				int wid, hei;
				switch(imagesize)
				{
					case -1:
						wid = ti.Icon.Width * 2;
						hei = ti.Icon.Height * 2;
						break;

					case 0:
						wid = ti.Icon.Width;
						hei = ti.Icon.Height;
						break;

					default:
						if(imagesize < 1) throw new NotSupportedException("Unexpected imagesize!");
						wid = imagesize;
						hei = imagesize;
						break;
				}
				wid = Math.Max(wid, ti.ImageNameWidth) + pad + pad;
				hei += pad + pad + fontheight;
				
				if(rw < wid)
				{
					// New row
					cx = 0;
					cy += my;
					my = 0;
				}

				my = Math.Max(my, hei);
				var rect = new Rectangle(cx + pad, cy + pad, wid - pad - pad, hei - pad - pad - fontheight);
				rectangles.Add(rect);
				cx += wid;
			}

			if(rectangles.Count > 0)
			{
				scrollbar.Maximum = cy + my;
				scrollbar.SmallChange = (imagesize > 0 ? imagesize : 128) + pad + pad + fontheight;
				scrollbar.LargeChange = ClientRectangle.Height;
				scrollbar.Visible = (scrollbar.Maximum > ClientRectangle.Height);

				if(scrollbar.Value > scrollbar.Maximum - ClientRectangle.Height)
				{
					scrollbar.Value = Math.Max(0, scrollbar.Maximum - ClientRectangle.Height);
				}
			}
			else
			{
				scrollbar.Visible = false;
			}

			Refresh();
		}

		#endregion

		#region ================== Rendering

		protected override void OnPaint(PaintEventArgs e)
		{
			// Draw bg
			using(var bg = new LinearGradientBrush(new Point(), new Point(0, this.ClientRectangle.Height), Color.DarkGray, Color.Black))
			{
				e.Graphics.FillRectangle(bg, this.ClientRectangle);
			}

			DrawTextures(e.Graphics);
		}

		private void DrawTextures(Graphics g)
		{
			// Draw items
			if(items.Count > 0)
			{
				int y = scrollbar.Value;
				int height = ClientRectangle.Height;

				for(var i = 0; i < items.Count; i++)
				{
					Rectangle rec = rectangles[i];
					if(rec.Bottom < y) continue;
					if(rec.Top > y + height) break;

					items[i].Draw(g, items[i].Icon.GetPreview(), 
						rec.X, rec.Y - y, 
						rec.Width, rec.Height, 
						selection.Contains(items[i]), showimagesizes);
				}
			}
		}

		#endregion
	}
}
