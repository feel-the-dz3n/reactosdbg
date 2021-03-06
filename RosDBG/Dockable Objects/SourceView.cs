﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace RosDBG
{
  
    public partial class SourceView : ToolWindow
    {
        string mSourceFile;
        //public event CanCopyChangedEventHandler CanCopyChangedEvent;
        
        public string SourceFile
        {
            get { return mSourceFile; }
            set
            {
                mSourceFile = value;
                UpdateSourceCode();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SourceCode.BackColor = Color.FromKnownColor(KnownColor.Window);
           // ((MainWindow)this.ParentForm).CopyEvent += CopyEvent;
        }

        public void ScrollTo(int line)
        {
        }

        Dictionary<int, Color> mHighlightedLines = new Dictionary<int, Color>();
        public void ClearHighlight()
        {
            SourceCode.SelectAll();
            SourceCode.SelectionBackColor = Color.FromKnownColor(KnownColor.Window);
            SourceCode.SelectionColor = Color.FromKnownColor(KnownColor.WindowText);
        }

        public void AddHighlight(int line, Color backColor, Color foreColor)
        {
            SourceCode.SelectionStart = SourceCode.GetFirstCharIndexFromLine(line);
            SourceCode.SelectionLength = SourceCode.GetFirstCharIndexFromLine(line + 1) - SourceCode.SelectionStart;
            SourceCode.SelectionBackColor = backColor;
            SourceCode.SelectionColor = foreColor;
        }

        public void RemoveHighlight(int line)
        {
            AddHighlight(line, Color.FromKnownColor(KnownColor.Window), Color.FromKnownColor(KnownColor.WindowText));
        }

        public SourceView()
        {
            InitializeComponent();
        }

        public SourceView(string FileName)
        {
            InitializeComponent();
            this.Text = FileName;
        }

        public void UpdateSourceCode()
        {
            SourceCode.Clear();
            ClearHighlight();

            if (mSourceFile == null) return;
            try
            {
                SourceCode.Lines = File.ReadAllLines(mSourceFile);
            }
            catch (IOException ex)
            {
                SourceCode.Text = "Could not load " + mSourceFile + ": " + ex.Message + "\n";
            }
        }

      /*  void CopyEvent(object sender, CopyEventArgs args)
        {
            if (args.Obj == this)  
                  
        }
        */
        private void SourceCode_SelectionChanged(object sender, EventArgs e)
        {
            btnCopy.Enabled = (SourceCode.SelectionLength > 0);
            copyToolStripMenuItem.Enabled = btnCopy.Enabled;

          /*  if (CanCopyChangedEvent != null)
                CanCopyChangedEvent(this, new CanCopyChangedEventArgs(btnCopy.Enabled)); */
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (SourceCode.SelectionLength != 0)
                Clipboard.SetText(SourceCode.SelectedText);

            //CopyEvent(this, new CopyEventArgs(this));
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SourceCode.SelectionLength != 0)
                Clipboard.SetText(SourceCode.SelectedText);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SourceCode.SelectAll();  
        }

        /*
        public override void Save(string FileName)
        {
            SaveAs(FileName); 
        }

        public override void SaveAs(string FileName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Textfiles (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string File = saveFileDialog.FileName;
            }
        }
        */

        public override string GetDocumentName()
        {
            return SourceFile;
        }

        private void SourceCode_MouseClick(object sender, MouseEventArgs e)
        {
            UpdatePos();
        }

        private void UpdatePos()
        {
            toolStripStatusLabel1.Text = "Row " + (SourceCode.GetLineFromCharIndex(SourceCode.SelectionStart) + 1).ToString()
                + ", Col " + (SourceCode.SelectionStart - SourceCode.GetFirstCharIndexOfCurrentLine() + 1).ToString();
        }

        private void SourceCode_KeyUp(object sender, KeyEventArgs e)
        {
            UpdatePos();
        }

    }

}
