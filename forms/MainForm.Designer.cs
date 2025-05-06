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
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            sfbHelloWorld = new Syncfusion.WinForms.Controls.SfButton();
            sfbFetchUsers = new Syncfusion.WinForms.Controls.SfButton();
            sfDataGrid1 = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)sfDataGrid1).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(sfDataGrid1, 0, 1);
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
            // 
            // sfDataGrid1
            // 
            sfDataGrid1.AccessibleName = "Table";
            sfDataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            sfDataGrid1.Location = new System.Drawing.Point(3, 53);
            sfDataGrid1.Name = "sfDataGrid1";
            sfDataGrid1.Size = new System.Drawing.Size(794, 394);
            sfDataGrid1.Style.BorderColor = System.Drawing.Color.FromArgb(100, 100, 100);
            sfDataGrid1.Style.CheckBoxStyle.CheckedBackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            sfDataGrid1.Style.CheckBoxStyle.CheckedBorderColor = System.Drawing.Color.FromArgb(0, 120, 215);
            sfDataGrid1.Style.CheckBoxStyle.IndeterminateBorderColor = System.Drawing.Color.FromArgb(0, 120, 215);
            sfDataGrid1.Style.HyperlinkStyle.DefaultLinkColor = System.Drawing.Color.FromArgb(0, 120, 215);
            sfDataGrid1.TabIndex = 1;
            sfDataGrid1.Text = "sfDataGrid1";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "MainForm";
            ShowIcon = false;
            Text = "WasmPrototype";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)sfDataGrid1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Syncfusion.WinForms.Controls.SfButton sfbHelloWorld;
        private Syncfusion.WinForms.Controls.SfButton sfbFetchUsers;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGrid1;
    }
}