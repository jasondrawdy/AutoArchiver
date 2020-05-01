/*
==============================================================================
Copyright © Jason Drawdy 

All rights reserved.

The MIT License (MIT)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

Except as contained in this notice, the name of the above copyright holder
shall not be used in advertising or otherwise to promote the sale, use or
other dealings in this Software without prior written authorization.
==============================================================================
*/

#region Imports

using System;
using System.Xml;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#endregion
namespace AutoArchiver
{
    public static class FormExtensions
    {
        #region Methods

        public static void RefreshControls(this Form form)
        {
            Size original = new Size();
            Size updated = new Size();
            foreach (Control c in form.Controls)
            {
                original = c.Size;
                updated = new Size(c.Width + 1, c.Height);
                c.Size = updated;
                c.Refresh();
                c.Size = original;
                c.Refresh();
            }
        }

        #endregion
    }

    public static class TreeViewExtensions
    {
        #region Methods

        public static List<string> GetExpansionState(this TreeNodeCollection nodes)
        {
            return nodes.Descendants()
                        .Where(n => n.IsExpanded)
                        .Select(n => n.FullPath)
                        .ToList();
        }

        public static void SetExpansionState(this TreeNodeCollection nodes, List<string> savedExpansionState)
        {
            foreach (var node in nodes.Descendants()
                                      .Where(n => savedExpansionState.Contains(n.FullPath)))
            {
                node.Expand();
            }
        }

        public static void SetSelectedNode(this TreeNodeCollection nodes, TreeView tree, string selectedNode)
        {
            TreeNode tn = new TreeNode();
            foreach (var node in nodes.Descendants()
                                 .Where(n => selectedNode.Contains(n.FullPath)))
            {
                if (node.Tag.ToString() == selectedNode) { tn = node; }
            }
            tree.SelectedNode = tn;
        }

        public static IEnumerable<TreeNode> Descendants(this TreeNodeCollection c)
        {
            foreach (var node in c.OfType<TreeNode>())
            {
                yield return node;

                foreach (var child in node.Nodes.Descendants())
                {
                    yield return child;
                }
            }
        }

        #endregion
    }

    public static class ColorExtensions
    {
        #region Methods

        public static Color GetColorFromString(this string input)
        {
            Color myColor = new Color();
            var splitString = input.Split(',');
            var splitInt = splitString.Select(item => int.Parse(item)).ToArray();
            myColor = Color.FromArgb(splitInt[0], splitInt[1], splitInt[2]);

            return myColor;
        }

        #endregion
    }

    public static class XElementExtensions
    {
        #region Methods

        public static string TryGetElementValue(this XElement parentElement, string elementName, string defaultValue = null)
        {
            var foundElement = parentElement.Element(elementName);
            if (foundElement != null) { return foundElement.Value; }
            return defaultValue;
        }

        #endregion
    }

    public static class StringExtensions
    {
        #region Methods

        public static List<string> CreateList(this string input)
        {
            List<string> list = new List<string>();
            list.Add(input);
            return list;
        }

        public static bool IsValidEmail(this string input)
        {
            string pattern = @"^\s*[\w\-\+_']+(\.[\w\-\+_']+)*\@[A-Za-z0-9]([\w\.-]*[A-Za-z0-9])?\.[A-Za-z][A-Za-z\.]*[A-Za-z]$";
            if (Regex.IsMatch(input, pattern)) return true;
            else return false;
            //if (!input.Contains('@')) return false;
        }

        public static string ReverseString(this string input)
        {
            char[] myArray = input.ToCharArray();
            Array.Reverse(myArray);

            string output = string.Empty;

            foreach (char character in myArray)
            {
                output += character;
            }

            return output;
        }

        public static string ToFileSize(this long l)
        {
            return String.Format(new FileSizeFormatProvider(), "{0:fs}", l);
        }

        public static IEnumerable<XElement> GetElementFromName(this string path, string elementName)
        {
            using (XmlReader reader = XmlReader.Create(path))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == elementName)
                        {
                            XElement el = XNode.ReadFrom(reader) as XElement;
                            if (el != null)
                                yield return el;
                        }
                    }
                }
            }
        }

        public static byte[] ToBytes(this string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }

        public static string FromBytes(this byte[] input)
        {
            return Encoding.UTF8.GetString(input);
        }
        
        public static string Encode(this string input)
        {
            return Convert.ToBase64String(input.ToBytes());
        }

        public static string Decode(this string input)
        {
            return Convert.FromBase64String(input).FromBytes();
        }

        public static byte[] FromBase64(this string input)
        {
            try
            {
                return Convert.FromBase64String(input);
            }
            catch { return null; }
        }

        public static int ParseSetting(this string input)
        {
            bool x;
            if (!bool.TryParse(input, out x))
                return 3;
            else
            {
                if (x) return 0;
                else return 1;
            }
        }

        public static int ParseInt(this string input)
        {
            int x;
            if (int.TryParse(input, out x))
                return x;
            else return 0;

        }

        public static bool ParseBool(this string input)
        {
            bool x;
            if (bool.TryParse(input, out x))
                return x;
            else return false;
        }
        #endregion
    }

    public static class ByteExtensions
    {
        #region Methods

        public static byte[] Merge(this byte[] input, byte[] partner)
        {
            byte[] merged = null;
            Buffer.BlockCopy(input, 0, merged, 0, input.Length);
            Buffer.BlockCopy(partner, 0, merged, merged.Length, partner.Length);
            return merged;
        }

        public static string ToBase64(this byte[] input)
        {
            try
            {
                return Convert.ToBase64String(input);
            }
            catch { return null; }
        }

        #endregion
    }

    public class FileSizeFormatProvider : IFormatProvider, ICustomFormatter
    {
        #region Variables

        private const string fileSizeFormat = "fs";
        private const Decimal OneKiloByte = 1024M;
        private const Decimal OneMegaByte = OneKiloByte * 1024M;
        private const Decimal OneGigaByte = OneMegaByte * 1024M;
        private const Decimal OneTeraByte = OneGigaByte * 1024M;
        private const Decimal OnePetaByte = OneTeraByte * 1024M;

        #endregion
        #region Methods

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter)) return this;
            return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (format == null || !format.StartsWith(fileSizeFormat))
            {
                return defaultFormat(format, arg, formatProvider);
            }

            if (arg is string)
            {
                return defaultFormat(format, arg, formatProvider);
            }

            Decimal size;

            try
            {
                size = Convert.ToDecimal(arg);
            }
            catch (InvalidCastException)
            {
                return defaultFormat(format, arg, formatProvider);
            }

            string suffix;
            if (size > OnePetaByte)
            {
                size /= OnePetaByte;
                suffix = " PB";
            }
            else if (size > OneTeraByte)
            {
                size /= OneTeraByte;
                suffix = " TB";
            }
            else if (size > OneGigaByte)
            {
                size /= OneGigaByte;
                suffix = " GB";
            }
            else if (size > OneMegaByte)
            {
                size /= OneMegaByte;
                suffix = " MB";
            }
            else if (size > OneKiloByte)
            {
                size /= OneKiloByte;
                suffix = " KB";
            }
            else
            {
                suffix = " B";
            }

            string precision = format.Substring(2);
            if (String.IsNullOrEmpty(precision)) precision = "2";
            return String.Format("{0:N" + precision + "}{1}", size, suffix);

        }

        private static string defaultFormat(string format, object arg, IFormatProvider formatProvider)
        {
            IFormattable formattableArg = arg as IFormattable;
            if (formattableArg != null)
            {
                return formattableArg.ToString(format, formatProvider);
            }
            return arg.ToString();
        }

        #endregion
    }
}
