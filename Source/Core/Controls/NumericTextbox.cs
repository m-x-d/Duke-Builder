
#region ================== Copyright (c) 2007 Pascal vd Heiden

/*
 * Copyright (c) 2007 Pascal vd Heiden, www.codeimp.com
 * This program is released under GNU General Public License
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 */

#endregion

#region ================== Namespaces

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

#endregion

namespace mxd.DukeBuilder.Controls
{
	public enum NumericTextboxApplyMode
	{
		NO_VALUE,
		SET,
		ADD,
		SUBTRACT,
	}
	
	public class NumericTextbox : AutoSelectTextbox
	{
		#region ================== Variables

		private bool allownegative;		// Allow negative numbers
		private bool allowrelative;		// Allow ++ and -- prefix for relative changes
		private bool allowdecimal;		// Allow decimal (float) numbers
		private bool controlpressed;

		//mxd. Result calculation caching...
		private int value;
		private float valuefloat;
		private bool valuechanged = true;
		private bool floatvaluechanged = true;
		private NumericTextboxApplyMode applymode;
		
		#endregion

		#region ================== Properties

		public bool AllowNegative { get { return allownegative; } set { allownegative = value; } }
		public bool AllowRelative { get { return allowrelative; } set { allowrelative = value; } }
		public bool AllowDecimal { get { return allowdecimal; } set { allowdecimal = value; } }
		public NumericTextboxApplyMode ApplyMode { get { return applymode; } } //mxd

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		public NumericTextbox()
		{
			this.ImeMode = ImeMode.Off;
		}

		#endregion

		#region ================== Methods

		// Key pressed
		protected override void OnKeyDown(KeyEventArgs e)
		{
			controlpressed = e.Control;
			base.OnKeyDown(e);
		}

		// Key released
		protected override void OnKeyUp(KeyEventArgs e)
		{
			controlpressed = e.Control;
			base.OnKeyUp(e);
		}
		
		// When a key is pressed
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			string allowedchars = "0123456789\b";

			// Determine allowed chars
			if(allownegative) allowedchars += CultureInfo.CurrentCulture.NumberFormat.NegativeSign;
			if(allowrelative) allowedchars += "+-";
			if(controlpressed) allowedchars += "\u0018\u0003\u0016";
			if(allowdecimal) allowedchars += CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
			
			// Check if key is not allowed
			if(allowedchars.IndexOf(e.KeyChar) == -1)
			{
				// Cancel this
				e.Handled = true;
			}
			else
			{
				// Check if + or - is pressed
				if((e.KeyChar == '+') || (e.KeyChar == '-'))
				{
					// Determine non-selected text
					string nonselectedtext;
					if(this.SelectionLength > 0)
					{
						nonselectedtext = this.Text.Substring(0, this.SelectionStart) +
							this.Text.Substring(this.SelectionStart + this.SelectionLength);
					}
					else if(this.SelectionLength < 0)
					{
						nonselectedtext = this.Text.Substring(0, this.SelectionStart + this.SelectionLength) +
							this.Text.Substring(this.SelectionStart);
					}
					else
					{
						nonselectedtext = this.Text;
					}
					
					// Not at the start?
					int selectionpos = this.SelectionStart - 1;
					if(this.SelectionLength < 0) selectionpos = (this.SelectionStart + this.SelectionLength) - 1;
					if(selectionpos > -1)
					{
						// Find any other characters before the insert position
						string textpart = this.Text.Substring(0, selectionpos + 1);
						textpart = textpart.Replace("+", "");
						textpart = textpart.Replace("-", "");
						if(textpart.Length > 0)
						{
							// Cancel this
							e.Handled = true;
						}
					}

					// Determine other prefix
					char otherprefix = (e.KeyChar == '+' ? '-' : '+');
					
					// Limit the number of + and - allowed
					int numprefixes = nonselectedtext.Split(e.KeyChar, otherprefix).Length;
					if(numprefixes > 2)
					{
						// Can't have more than 2 prefixes
						e.Handled = true;
					}
					else if(numprefixes > 1)
					{
						// Must have 2 the same prefixes
						if(this.Text.IndexOf(e.KeyChar) == -1) e.Handled = true;

						// Double prefix must be allowed
						if(!allowrelative) e.Handled = true;
					}
				}
			}
			
			// Call base
			base.OnKeyPress(e);
		}

		// Validate contents
		protected override void OnValidating(CancelEventArgs e)
		{
			string textpart = this.Text;

			// Strip prefixes
			textpart = textpart.Replace("+", "");
			if(!allownegative) textpart = textpart.Replace("-", "");
			
			// No numbers left?
			if(textpart.Length == 0)
			{
				// Make the textbox empty
				this.Text = "";
			}
			
			// Call base
			base.OnValidating(e);
		}

		//mxd. Value recalculation required!
		protected override void OnTextChanged(EventArgs e)
		{
			valuechanged = true;
			floatvaluechanged = true;

			base.OnTextChanged(e);
		}

		// This checks if the number is relative
		public bool CheckIsRelative()
		{
			// Prefixed with ++ or --?
			return (this.Text.StartsWith("++") || this.Text.StartsWith("--"));
		}
		
		// This determines the result value
		public int GetResult(int original)
		{
			//mxd. Update cached value? 
			if(valuechanged)
			{
				// Get text without prefixes
				string textpart = this.Text.Replace("+", "").Replace("-", "");

				// Any numbers left?
				if(textpart.Length > 0)
				{
					//mxd. Parse result and set Apply mode. Result will be set to 0 if parsing fails
					if(this.Text.StartsWith("++"))
					{
						int.TryParse(textpart, out value);
						applymode = NumericTextboxApplyMode.ADD;
					}
					else if(this.Text.StartsWith("--"))
					{
						int.TryParse(textpart, out value);
						applymode = NumericTextboxApplyMode.SUBTRACT;
					}
					else
					{
						int.TryParse(this.Text, out value);
						applymode = NumericTextboxApplyMode.SET;
					}
				}
				else
				{
					applymode = NumericTextboxApplyMode.NO_VALUE;
				}
				
				valuechanged = false;
			}
			
			//mxd. Calculate result
			int result;
			switch(applymode)
			{
				case NumericTextboxApplyMode.NO_VALUE: result = original; break;
				case NumericTextboxApplyMode.SET: result = value; break;
				case NumericTextboxApplyMode.ADD: result = value + original; break;
				case NumericTextboxApplyMode.SUBTRACT: result = original - value; break;
				default: throw new NotImplementedException("Unsupported ApplyMode");
			}

			//mxd. Return result
			return (!allownegative ? Math.Max(result, 0) : result);
		}

		// This determines the result value
		public float GetResultFloat(float original)
		{
			//mxd. Update cached value? 
			if(floatvaluechanged)
			{
				// Get text without prefixes
				string textpart = this.Text.Replace("+", "").Replace("-", "");

				// Any numbers left?
				if(textpart.Length > 0)
				{
					//mxd. Parse result and set Apply mode. Result will be set to 0 if parsing fails
					if(this.Text.StartsWith("++"))
					{
						float.TryParse(textpart, out valuefloat);
						applymode = NumericTextboxApplyMode.ADD;
					}
					else if(this.Text.StartsWith("--"))
					{
						float.TryParse(textpart, out valuefloat);
						applymode = NumericTextboxApplyMode.SUBTRACT;
					}
					else
					{
						float.TryParse(this.Text, out valuefloat);
						applymode = NumericTextboxApplyMode.SET;
					}
				}
				else
				{
					applymode = NumericTextboxApplyMode.NO_VALUE;
				}

				floatvaluechanged = false;
			}

			//mxd. Calculate result
			float result;
			switch(applymode)
			{
				case NumericTextboxApplyMode.NO_VALUE: result = original; break;
				case NumericTextboxApplyMode.SET: result = valuefloat; break;
				case NumericTextboxApplyMode.ADD: result = valuefloat + original; break;
				case NumericTextboxApplyMode.SUBTRACT: result = original - valuefloat; break;
				default: throw new NotImplementedException("Unsupported ApplyMode");
			}

			//mxd. Return result
			return (!allownegative ? Math.Max(result, 0f) : result);
		}
		
		#endregion
	}
}
