using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalDetector.shared.combo
{
    public class ComboBoxItem
    {
        public int Value { get; set; }
        public string Text { get; set; }
        public override string ToString()
        {
            return Text.Replace("_", " ");
        }
    }
}
