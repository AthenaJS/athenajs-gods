namespace GodsViewer {
  partial class Main {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose (bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose ();
      }
      base.Dispose (disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent () {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (Main));
      this._panel1UI = new System.Windows.Forms.Panel ();
      this._panel2UI = new GodsViewer.DoubleBufferedPanel ();
      this._statusUI = new System.Windows.Forms.StatusStrip ();
      this._statusLabelUI = new System.Windows.Forms.ToolStripStatusLabel ();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip ();
      this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel ();
      this._worldUI = new System.Windows.Forms.ToolStripComboBox ();
      this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel ();
      this._systemUI = new System.Windows.Forms.ToolStripComboBox ();
      this._menuUI = new System.Windows.Forms.MenuStrip ();
      this._menuFile = new System.Windows.Forms.ToolStripMenuItem ();
      this._menuFileExitUI = new System.Windows.Forms.ToolStripMenuItem ();
      this._menuViewUI = new System.Windows.Forms.ToolStripMenuItem ();
      this._menuViewShowRasterUI = new System.Windows.Forms.ToolStripMenuItem ();
      this._menuViewHighlightUI = new System.Windows.Forms.ToolStripMenuItem ();
      this._menuViewShowVisualHintsUI = new System.Windows.Forms.ToolStripMenuItem ();
      this._menuViewShowEventsUI = new System.Windows.Forms.ToolStripMenuItem ();
      this._menuViewShowHiddenObjectsUI = new System.Windows.Forms.ToolStripMenuItem ();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator ();
      this._menuViewRecursiveEventsUI = new System.Windows.Forms.ToolStripMenuItem ();
      this._menuViewShowForwardLinksUI = new System.Windows.Forms.ToolStripMenuItem ();
      this._menuViewShowBackwardLinksUI = new System.Windows.Forms.ToolStripMenuItem ();
      this._menuFileSavePngUI = new System.Windows.Forms.ToolStripMenuItem ();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator ();
      this._panel1UI.SuspendLayout ();
      this._statusUI.SuspendLayout ();
      this.toolStrip1.SuspendLayout ();
      this._menuUI.SuspendLayout ();
      this.SuspendLayout ();
      // 
      // _panel1UI
      // 
      this._panel1UI.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._panel1UI.AutoScroll = true;
      this._panel1UI.BackColor = System.Drawing.Color.Black;
      this._panel1UI.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this._panel1UI.Controls.Add (this._panel2UI);
      this._panel1UI.Location = new System.Drawing.Point (0, 49);
      this._panel1UI.Margin = new System.Windows.Forms.Padding (0);
      this._panel1UI.Name = "_panel1UI";
      this._panel1UI.Size = new System.Drawing.Size (632, 383);
      this._panel1UI.TabIndex = 1;
      // 
      // _panel2UI
      // 
      this._panel2UI.Dock = System.Windows.Forms.DockStyle.Fill;
      this._panel2UI.Location = new System.Drawing.Point (0, 0);
      this._panel2UI.Margin = new System.Windows.Forms.Padding (0);
      this._panel2UI.Name = "_panel2UI";
      this._panel2UI.Size = new System.Drawing.Size (628, 379);
      this._panel2UI.TabIndex = 0;
      this._panel2UI.MouseLeave += new System.EventHandler (this._panel2UI_MouseLeave);
      this._panel2UI.Paint += new System.Windows.Forms.PaintEventHandler (this._panel2UI_Paint);
      this._panel2UI.MouseMove += new System.Windows.Forms.MouseEventHandler (this._panel2UI_MouseMove);
      this._panel2UI.MouseDown += new System.Windows.Forms.MouseEventHandler (this._panel2UI_MouseDown);
      // 
      // _statusUI
      // 
      this._statusUI.Items.AddRange (new System.Windows.Forms.ToolStripItem [] {
            this._statusLabelUI});
      this._statusUI.Location = new System.Drawing.Point (0, 431);
      this._statusUI.Name = "_statusUI";
      this._statusUI.Size = new System.Drawing.Size (632, 22);
      this._statusUI.TabIndex = 2;
      this._statusUI.Text = "statusStrip1";
      // 
      // _statusLabelUI
      // 
      this._statusLabelUI.Name = "_statusLabelUI";
      this._statusLabelUI.Size = new System.Drawing.Size (0, 17);
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange (new System.Windows.Forms.ToolStripItem [] {
            this.toolStripLabel1,
            this._worldUI,
            this.toolStripLabel2,
            this._systemUI});
      this.toolStrip1.Location = new System.Drawing.Point (0, 24);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size (632, 25);
      this.toolStrip1.TabIndex = 3;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // toolStripLabel1
      // 
      this.toolStripLabel1.Name = "toolStripLabel1";
      this.toolStripLabel1.Size = new System.Drawing.Size (31, 22);
      this.toolStripLabel1.Text = "Map:";
      // 
      // _worldUI
      // 
      this._worldUI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._worldUI.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
      this._worldUI.Items.AddRange (new object [] {
            "Level 1 - Part A - The City",
            "Level 1 - Part B - The City",
            "Level 2 - Part A - The Temple",
            "Level 2 - Part B - The Temple",
            "Level 3 - Part A - The Labyrinth",
            "Level 3 - Part B - The Labyrinth",
            "Level 4 - Part A - The Underworld",
            "Level 4 - Part B - The Underworld"});
      this._worldUI.Name = "_worldUI";
      this._worldUI.Size = new System.Drawing.Size (192, 25);
      this._worldUI.SelectedIndexChanged += new System.EventHandler (this._worldUI_SelectedIndexChanged);
      // 
      // toolStripLabel2
      // 
      this.toolStripLabel2.Name = "toolStripLabel2";
      this.toolStripLabel2.Size = new System.Drawing.Size (46, 22);
      this.toolStripLabel2.Text = "System:";
      // 
      // _systemUI
      // 
      this._systemUI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._systemUI.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
      this._systemUI.Items.AddRange (new object [] {
            "Atari",
            "Amiga"});
      this._systemUI.Name = "_systemUI";
      this._systemUI.Size = new System.Drawing.Size (75, 25);
      this._systemUI.SelectedIndexChanged += new System.EventHandler (this._systemUI_SelectedIndexChanged);
      // 
      // _menuUI
      // 
      this._menuUI.Items.AddRange (new System.Windows.Forms.ToolStripItem [] {
            this._menuFile,
            this._menuViewUI});
      this._menuUI.Location = new System.Drawing.Point (0, 0);
      this._menuUI.Name = "_menuUI";
      this._menuUI.Size = new System.Drawing.Size (632, 24);
      this._menuUI.TabIndex = 4;
      this._menuUI.Text = "menuStrip1";
      // 
      // _menuFile
      // 
      this._menuFile.DropDownItems.AddRange (new System.Windows.Forms.ToolStripItem [] {
            this._menuFileSavePngUI,
            this.toolStripSeparator2,
            this._menuFileExitUI});
      this._menuFile.Name = "_menuFile";
      this._menuFile.Size = new System.Drawing.Size (35, 20);
      this._menuFile.Text = "File";
      // 
      // _menuFileExitUI
      // 
      this._menuFileExitUI.Name = "_menuFileExitUI";
      this._menuFileExitUI.Size = new System.Drawing.Size (180, 22);
      this._menuFileExitUI.Text = "Exit";
      this._menuFileExitUI.Click += new System.EventHandler (this._menuFileExitUI_Click);
      // 
      // _menuViewUI
      // 
      this._menuViewUI.DropDownItems.AddRange (new System.Windows.Forms.ToolStripItem [] {
            this._menuViewShowRasterUI,
            this._menuViewHighlightUI,
            this._menuViewShowVisualHintsUI,
            this._menuViewShowEventsUI,
            this._menuViewShowHiddenObjectsUI,
            this.toolStripSeparator1,
            this._menuViewRecursiveEventsUI,
            this._menuViewShowForwardLinksUI,
            this._menuViewShowBackwardLinksUI});
      this._menuViewUI.Name = "_menuViewUI";
      this._menuViewUI.Size = new System.Drawing.Size (41, 20);
      this._menuViewUI.Text = "View";
      // 
      // _menuViewShowRasterUI
      // 
      this._menuViewShowRasterUI.CheckOnClick = true;
      this._menuViewShowRasterUI.Name = "_menuViewShowRasterUI";
      this._menuViewShowRasterUI.Size = new System.Drawing.Size (191, 22);
      this._menuViewShowRasterUI.Text = "Show raster effect";
      this._menuViewShowRasterUI.Click += new System.EventHandler (this._menuViewShowRasterUI_Click);
      // 
      // _menuViewHighlightUI
      // 
      this._menuViewHighlightUI.CheckOnClick = true;
      this._menuViewHighlightUI.Name = "_menuViewHighlightUI";
      this._menuViewHighlightUI.Size = new System.Drawing.Size (191, 22);
      this._menuViewHighlightUI.Text = "Highlight walls && stairs";
      this._menuViewHighlightUI.Click += new System.EventHandler (this._menuViewHighlightUI_Click);
      // 
      // _menuViewShowVisualHintsUI
      // 
      this._menuViewShowVisualHintsUI.CheckOnClick = true;
      this._menuViewShowVisualHintsUI.Name = "_menuViewShowVisualHintsUI";
      this._menuViewShowVisualHintsUI.Size = new System.Drawing.Size (191, 22);
      this._menuViewShowVisualHintsUI.Text = "Show visual hints";
      this._menuViewShowVisualHintsUI.Click += new System.EventHandler (this._menuViewShowVisualHintsUI_Click);
      // 
      // _menuViewShowEventsUI
      // 
      this._menuViewShowEventsUI.CheckOnClick = true;
      this._menuViewShowEventsUI.Name = "_menuViewShowEventsUI";
      this._menuViewShowEventsUI.Size = new System.Drawing.Size (191, 22);
      this._menuViewShowEventsUI.Text = "Show events";
      this._menuViewShowEventsUI.Click += new System.EventHandler (this._menuViewShowEventsUI_Click);
      // 
      // _menuViewShowHiddenObjectsUI
      // 
      this._menuViewShowHiddenObjectsUI.CheckOnClick = true;
      this._menuViewShowHiddenObjectsUI.Name = "_menuViewShowHiddenObjectsUI";
      this._menuViewShowHiddenObjectsUI.Size = new System.Drawing.Size (191, 22);
      this._menuViewShowHiddenObjectsUI.Text = "Show hidden objects";
      this._menuViewShowHiddenObjectsUI.Click += new System.EventHandler (this._menuViewShowHiddenObjectsUI_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size (188, 6);
      // 
      // _menuViewRecursiveEventsUI
      // 
      this._menuViewRecursiveEventsUI.CheckOnClick = true;
      this._menuViewRecursiveEventsUI.Name = "_menuViewRecursiveEventsUI";
      this._menuViewRecursiveEventsUI.Size = new System.Drawing.Size (191, 22);
      this._menuViewRecursiveEventsUI.Text = "Recursive events";
      this._menuViewRecursiveEventsUI.Click += new System.EventHandler (this._menuViewRecursiveEventsUI_Click);
      // 
      // _menuViewShowForwardLinksUI
      // 
      this._menuViewShowForwardLinksUI.CheckOnClick = true;
      this._menuViewShowForwardLinksUI.Name = "_menuViewShowForwardLinksUI";
      this._menuViewShowForwardLinksUI.Size = new System.Drawing.Size (191, 22);
      this._menuViewShowForwardLinksUI.Text = "Show forward links";
      this._menuViewShowForwardLinksUI.Click += new System.EventHandler (this._menuViewShowForwardLinksUI_Click);
      // 
      // _menuViewShowBackwardLinksUI
      // 
      this._menuViewShowBackwardLinksUI.CheckOnClick = true;
      this._menuViewShowBackwardLinksUI.Name = "_menuViewShowBackwardLinksUI";
      this._menuViewShowBackwardLinksUI.Size = new System.Drawing.Size (191, 22);
      this._menuViewShowBackwardLinksUI.Text = "Show backward links";
      this._menuViewShowBackwardLinksUI.Click += new System.EventHandler (this._menuViewShowBackwardLinksUI_Click);
      // 
      // _menuFileSavePngUI
      // 
      this._menuFileSavePngUI.Name = "_menuFileSavePngUI";
      this._menuFileSavePngUI.Size = new System.Drawing.Size (180, 22);
      this._menuFileSavePngUI.Text = "Save map to PNG...";
      this._menuFileSavePngUI.Click += new System.EventHandler (this._menuFileSavePngUI_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size (177, 6);
      // 
      // Main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size (632, 453);
      this.Controls.Add (this.toolStrip1);
      this.Controls.Add (this._statusUI);
      this.Controls.Add (this._menuUI);
      this.Controls.Add (this._panel1UI);
      this.Icon = ((System.Drawing.Icon) (resources.GetObject ("$this.Icon")));
      this.MainMenuStrip = this._menuUI;
      this.MinimumSize = new System.Drawing.Size (320, 200);
      this.Name = "Main";
      this.Text = "Gods Viewer - v1.02";
      this.Load += new System.EventHandler (this.Main_Load);
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler (this.Main_FormClosed);
      this._panel1UI.ResumeLayout (false);
      this._statusUI.ResumeLayout (false);
      this._statusUI.PerformLayout ();
      this.toolStrip1.ResumeLayout (false);
      this.toolStrip1.PerformLayout ();
      this._menuUI.ResumeLayout (false);
      this._menuUI.PerformLayout ();
      this.ResumeLayout (false);
      this.PerformLayout ();

    }

    #endregion

    private System.Windows.Forms.Panel _panel1UI;
    private DoubleBufferedPanel _panel2UI;
    private System.Windows.Forms.StatusStrip _statusUI;
    private System.Windows.Forms.ToolStripStatusLabel _statusLabelUI;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    private System.Windows.Forms.ToolStripComboBox _worldUI;
    private System.Windows.Forms.MenuStrip _menuUI;
    private System.Windows.Forms.ToolStripMenuItem _menuFile;
    private System.Windows.Forms.ToolStripMenuItem _menuFileExitUI;
    private System.Windows.Forms.ToolStripMenuItem _menuViewUI;
    private System.Windows.Forms.ToolStripMenuItem _menuViewShowRasterUI;
    private System.Windows.Forms.ToolStripMenuItem _menuViewHighlightUI;
    private System.Windows.Forms.ToolStripMenuItem _menuViewShowVisualHintsUI;
    private System.Windows.Forms.ToolStripMenuItem _menuViewShowEventsUI;
    private System.Windows.Forms.ToolStripMenuItem _menuViewShowHiddenObjectsUI;
    private System.Windows.Forms.ToolStripMenuItem _menuViewShowBackwardLinksUI;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem _menuViewRecursiveEventsUI;
    private System.Windows.Forms.ToolStripMenuItem _menuViewShowForwardLinksUI;
    private System.Windows.Forms.ToolStripLabel toolStripLabel2;
    private System.Windows.Forms.ToolStripComboBox _systemUI;
    private System.Windows.Forms.ToolStripMenuItem _menuFileSavePngUI;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
  }
}

