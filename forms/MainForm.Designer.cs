namespace SwissPension.WasmPrototype.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            sfbHelloWorld = new Syncfusion.WinForms.Controls.SfButton();
            sfbFetchUsers = new Syncfusion.WinForms.Controls.SfButton();
            sfdgUsers = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)sfdgUsers).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(sfdgUsers, 0, 1);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(sfbHelloWorld, 0, 0);
            tableLayoutPanel2.Controls.Add(sfbFetchUsers, 1, 0);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel2.Size = new System.Drawing.Size(794, 44);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // sfbHelloWorld
            // 
            sfbHelloWorld.Dock = System.Windows.Forms.DockStyle.Fill;
            sfbHelloWorld.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            sfbHelloWorld.Location = new System.Drawing.Point(3, 3);
            sfbHelloWorld.Name = "sfbHelloWorld";
            sfbHelloWorld.Size = new System.Drawing.Size(391, 38);
            sfbHelloWorld.TabIndex = 0;
            sfbHelloWorld.Text = "Hello World";
            sfbHelloWorld.Click += sfbHelloWorld_Click;
            // 
            // sfbFetchUsers
            // 
            sfbFetchUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            sfbFetchUsers.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            sfbFetchUsers.Location = new System.Drawing.Point(400, 3);
            sfbFetchUsers.Name = "sfbFetchUsers";
            sfbFetchUsers.Size = new System.Drawing.Size(391, 38);
            sfbFetchUsers.TabIndex = 1;
            sfbFetchUsers.Text = "Fetch Users";
            sfbFetchUsers.Click += sfbFetchUsers_Click;
            // 
            // sfdgUsers
            // 
            sfdgUsers.AccessibleName = "Table";
            sfdgUsers.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.Fill;
            sfdgUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            sfdgUsers.EnableDataVirtualization = true;
            sfdgUsers.Location = new System.Drawing.Point(3, 53);
            sfdgUsers.Name = "sfdgUsers";
            sfdgUsers.ShowRowHeader = true;
            sfdgUsers.Size = new System.Drawing.Size(794, 394);
            sfdgUsers.Style.BorderColor = System.Drawing.Color.FromArgb(100, 100, 100);
            sfdgUsers.Style.CheckBoxStyle.CheckedBackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            sfdgUsers.Style.CheckBoxStyle.CheckedBorderColor = System.Drawing.Color.FromArgb(0, 120, 215);
            sfdgUsers.Style.CheckBoxStyle.IndeterminateBorderColor = System.Drawing.Color.FromArgb(0, 120, 215);
            sfdgUsers.Style.HyperlinkStyle.DefaultLinkColor = System.Drawing.Color.FromArgb(0, 120, 215);
            sfdgUsers.TabIndex = 1;
            sfdgUsers.Text = "sfdgUsers";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "WasmPrototype";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)sfdgUsers).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Syncfusion.WinForms.Controls.SfButton sfbHelloWorld;
        private Syncfusion.WinForms.Controls.SfButton sfbFetchUsers;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfdgUsers;
    }
}