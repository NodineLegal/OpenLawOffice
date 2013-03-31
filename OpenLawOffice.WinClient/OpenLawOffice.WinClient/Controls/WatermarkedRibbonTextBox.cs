using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Windows.Controls.Ribbon;

namespace OpenLawOffice.WinClient.Controls
{
    public class WatermarkedRibbonTextBox : RibbonTextBox
    {
        private string _oldText = "";

        public bool IsDisplayingWatermark { get; private set; }
        public string Watermark { get; set; }
        public bool HasUserData
        {
            get
            {
                if (IsDisplayingWatermark) return false;
                return Text.Trim().Length > 0;
            }
        }
        public event TextChangedEventHandler UserDataChanged;

        public WatermarkedRibbonTextBox()
        {
            this.GotKeyboardFocus += (sender, e) =>
                {
                    if (IsDisplayingWatermark)
                        DropWatermark();
                };

            this.LostKeyboardFocus += (sender, e) =>
            {
                string text = ((WatermarkedRibbonTextBox)e.Source).Text;
                if (text.Length <= 0 && !IsDisplayingWatermark)
                    ApplyWatermark();
            };

            this.LostFocus += (sender, e) =>
                {
                    string text = ((WatermarkedRibbonTextBox)e.Source).Text;
                    if (text.Length <= 0 && !IsDisplayingWatermark)
                        ApplyWatermark();
                };

            this.Loaded += (sender, e) =>
                {
                    if (Text.Length <= 0)
                        ApplyWatermark();
                };

            this.TextChanged += (sender, e) =>
                {
                    if (!IsDisplayingWatermark)
                    {
                        WatermarkedRibbonTextBox textBox = (WatermarkedRibbonTextBox)e.Source;
                        if (textBox.Text != _oldText)
                        {
                            if (UserDataChanged != null) UserDataChanged(sender, e);
                            _oldText = textBox.Text.Trim();
                        }
                    }
                };
        }

        private void ApplyWatermark()
        {
            FontStyle = FontStyles.Italic;
            Foreground = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
            IsDisplayingWatermark = true;
            Text = Watermark;
        }

        private void DropWatermark()
        {
            FontStyle = FontStyles.Normal;
            Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            IsDisplayingWatermark = false;
            Text = "";
        }
    }
}
