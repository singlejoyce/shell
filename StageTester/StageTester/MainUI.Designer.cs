namespace StageTester
{
	partial class MainUI
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.m_btn_ChooseDir = new System.Windows.Forms.Button();
            this.m_edt_StageDir = new System.Windows.Forms.TextBox();
            this.m_lab_StageDir = new System.Windows.Forms.Label();
            this.m_lab_StageMode = new System.Windows.Forms.Label();
            this.m_stc_StageCount = new System.Windows.Forms.Label();
            this.m_lab_StageCount = new System.Windows.Forms.Label();
            this.m_stc_SucCount = new System.Windows.Forms.Label();
            this.m_lab_SucCount = new System.Windows.Forms.Label();
            this.m_stc_FailCount = new System.Windows.Forms.Label();
            this.m_lab_FailCount = new System.Windows.Forms.Label();
            this.m_lst_Result = new System.Windows.Forms.ListBox();
            this.m_dgv_Detail = new System.Windows.Forms.DataGridView();
            this.m_btn_Test = new System.Windows.Forms.Button();
            this.m_dgv_colFeature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_dgv_colEasy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_dgv_colNormal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_dgv_colHard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_dgv_sup_colEasy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_dgv_sup_colNormal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_dgv_sup_colHard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgv_Detail)).BeginInit();
            this.SuspendLayout();
            // 
            // m_btn_ChooseDir
            // 
            this.m_btn_ChooseDir.Location = new System.Drawing.Point(12, 12);
            this.m_btn_ChooseDir.Name = "m_btn_ChooseDir";
            this.m_btn_ChooseDir.Size = new System.Drawing.Size(75, 23);
            this.m_btn_ChooseDir.TabIndex = 0;
            this.m_btn_ChooseDir.Text = "选择文件夹";
            this.m_btn_ChooseDir.UseVisualStyleBackColor = true;
            this.m_btn_ChooseDir.Click += new System.EventHandler(this.OnBtnChooseDirClick);
            // 
            // m_edt_StageDir
            // 
            this.m_edt_StageDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_edt_StageDir.Location = new System.Drawing.Point(93, 13);
            this.m_edt_StageDir.Name = "m_edt_StageDir";
            this.m_edt_StageDir.Size = new System.Drawing.Size(458, 21);
            this.m_edt_StageDir.TabIndex = 1;
            // 
            // m_lab_StageDir
            // 
            this.m_lab_StageDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_lab_StageDir.Location = new System.Drawing.Point(12, 39);
            this.m_lab_StageDir.Name = "m_lab_StageDir";
            this.m_lab_StageDir.Size = new System.Drawing.Size(473, 21);
            this.m_lab_StageDir.TabIndex = 2;
            this.m_lab_StageDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_lab_StageMode
            // 
            this.m_lab_StageMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_lab_StageMode.Location = new System.Drawing.Point(491, 39);
            this.m_lab_StageMode.Name = "m_lab_StageMode";
            this.m_lab_StageMode.Size = new System.Drawing.Size(60, 21);
            this.m_lab_StageMode.TabIndex = 3;
            this.m_lab_StageMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_stc_StageCount
            // 
            this.m_stc_StageCount.AutoSize = true;
            this.m_stc_StageCount.Location = new System.Drawing.Point(460, 120);
            this.m_stc_StageCount.Name = "m_stc_StageCount";
            this.m_stc_StageCount.Size = new System.Drawing.Size(29, 12);
            this.m_stc_StageCount.TabIndex = 4;
            this.m_stc_StageCount.Text = "关卡";
            this.m_stc_StageCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_lab_StageCount
            // 
            this.m_lab_StageCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_lab_StageCount.Location = new System.Drawing.Point(491, 116);
            this.m_lab_StageCount.Name = "m_lab_StageCount";
            this.m_lab_StageCount.Size = new System.Drawing.Size(60, 21);
            this.m_lab_StageCount.TabIndex = 5;
            this.m_lab_StageCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_stc_SucCount
            // 
            this.m_stc_SucCount.AutoSize = true;
            this.m_stc_SucCount.Location = new System.Drawing.Point(460, 147);
            this.m_stc_SucCount.Name = "m_stc_SucCount";
            this.m_stc_SucCount.Size = new System.Drawing.Size(29, 12);
            this.m_stc_SucCount.TabIndex = 6;
            this.m_stc_SucCount.Text = "通过";
            this.m_stc_SucCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_lab_SucCount
            // 
            this.m_lab_SucCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_lab_SucCount.Location = new System.Drawing.Point(491, 143);
            this.m_lab_SucCount.Name = "m_lab_SucCount";
            this.m_lab_SucCount.Size = new System.Drawing.Size(60, 21);
            this.m_lab_SucCount.TabIndex = 7;
            this.m_lab_SucCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_stc_FailCount
            // 
            this.m_stc_FailCount.AutoSize = true;
            this.m_stc_FailCount.Location = new System.Drawing.Point(460, 174);
            this.m_stc_FailCount.Name = "m_stc_FailCount";
            this.m_stc_FailCount.Size = new System.Drawing.Size(29, 12);
            this.m_stc_FailCount.TabIndex = 8;
            this.m_stc_FailCount.Text = "失败";
            this.m_stc_FailCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_lab_FailCount
            // 
            this.m_lab_FailCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_lab_FailCount.Location = new System.Drawing.Point(491, 170);
            this.m_lab_FailCount.Name = "m_lab_FailCount";
            this.m_lab_FailCount.Size = new System.Drawing.Size(60, 21);
            this.m_lab_FailCount.TabIndex = 9;
            this.m_lab_FailCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_lst_Result
            // 
            this.m_lst_Result.FormattingEnabled = true;
            this.m_lst_Result.ItemHeight = 12;
            this.m_lst_Result.Location = new System.Drawing.Point(12, 65);
            this.m_lst_Result.Name = "m_lst_Result";
            this.m_lst_Result.Size = new System.Drawing.Size(440, 232);
            this.m_lst_Result.TabIndex = 10;
            this.m_lst_Result.SelectedIndexChanged += new System.EventHandler(this.OnChangeResultSelected);
            // 
            // m_dgv_Detail
            // 
            this.m_dgv_Detail.AllowUserToAddRows = false;
            this.m_dgv_Detail.AllowUserToDeleteRows = false;
            this.m_dgv_Detail.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.m_dgv_Detail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgv_Detail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.m_dgv_Detail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_dgv_Detail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_dgv_colFeature,
            this.m_dgv_colEasy,
            this.m_dgv_colNormal,
            this.m_dgv_colHard,
            this.m_dgv_sup_colEasy,
            this.m_dgv_sup_colNormal,
            this.m_dgv_sup_colHard});
            this.m_dgv_Detail.Location = new System.Drawing.Point(12, 303);
            this.m_dgv_Detail.MultiSelect = false;
            this.m_dgv_Detail.Name = "m_dgv_Detail";
            this.m_dgv_Detail.ReadOnly = true;
            this.m_dgv_Detail.RowHeadersVisible = false;
            this.m_dgv_Detail.RowTemplate.Height = 23;
            this.m_dgv_Detail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgv_Detail.Size = new System.Drawing.Size(748, 349);
            this.m_dgv_Detail.TabIndex = 11;
            // 
            // m_btn_Test
            // 
            this.m_btn_Test.Location = new System.Drawing.Point(491, 78);
            this.m_btn_Test.Name = "m_btn_Test";
            this.m_btn_Test.Size = new System.Drawing.Size(60, 23);
            this.m_btn_Test.TabIndex = 12;
            this.m_btn_Test.Text = "测试";
            this.m_btn_Test.UseVisualStyleBackColor = true;
            this.m_btn_Test.Click += new System.EventHandler(this.OnBtnTestClick);
            // 
            // m_dgv_colFeature
            // 
            this.m_dgv_colFeature.HeaderText = "";
            this.m_dgv_colFeature.Name = "m_dgv_colFeature";
            this.m_dgv_colFeature.ReadOnly = true;
            this.m_dgv_colFeature.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.m_dgv_colFeature.Width = 144;
            // 
            // m_dgv_colEasy
            // 
            this.m_dgv_colEasy.HeaderText = "简单";
            this.m_dgv_colEasy.Name = "m_dgv_colEasy";
            this.m_dgv_colEasy.ReadOnly = true;
            this.m_dgv_colEasy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_dgv_colNormal
            // 
            this.m_dgv_colNormal.HeaderText = "普通";
            this.m_dgv_colNormal.Name = "m_dgv_colNormal";
            this.m_dgv_colNormal.ReadOnly = true;
            this.m_dgv_colNormal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_dgv_colHard
            // 
            this.m_dgv_colHard.HeaderText = "困难";
            this.m_dgv_colHard.Name = "m_dgv_colHard";
            this.m_dgv_colHard.ReadOnly = true;
            this.m_dgv_colHard.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_dgv_sup_colEasy
            // 
            this.m_dgv_sup_colEasy.HeaderText = "超级-简单";
            this.m_dgv_sup_colEasy.Name = "m_dgv_sup_colEasy";
            this.m_dgv_sup_colEasy.ReadOnly = true;
            this.m_dgv_sup_colEasy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_dgv_sup_colNormal
            // 
            this.m_dgv_sup_colNormal.HeaderText = "超级-普通";
            this.m_dgv_sup_colNormal.Name = "m_dgv_sup_colNormal";
            this.m_dgv_sup_colNormal.ReadOnly = true;
            this.m_dgv_sup_colNormal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_dgv_sup_colHard
            // 
            this.m_dgv_sup_colHard.HeaderText = "超级-困难";
            this.m_dgv_sup_colHard.Name = "m_dgv_sup_colHard";
            this.m_dgv_sup_colHard.ReadOnly = true;
            this.m_dgv_sup_colHard.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 664);
            this.Controls.Add(this.m_btn_Test);
            this.Controls.Add(this.m_dgv_Detail);
            this.Controls.Add(this.m_lst_Result);
            this.Controls.Add(this.m_lab_FailCount);
            this.Controls.Add(this.m_stc_FailCount);
            this.Controls.Add(this.m_lab_SucCount);
            this.Controls.Add(this.m_stc_SucCount);
            this.Controls.Add(this.m_lab_StageCount);
            this.Controls.Add(this.m_stc_StageCount);
            this.Controls.Add(this.m_lab_StageMode);
            this.Controls.Add(this.m_lab_StageDir);
            this.Controls.Add(this.m_edt_StageDir);
            this.Controls.Add(this.m_btn_ChooseDir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StageTester";
            ((System.ComponentModel.ISupportInitialize)(this.m_dgv_Detail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button m_btn_ChooseDir;
		private System.Windows.Forms.TextBox m_edt_StageDir;
		private System.Windows.Forms.Label m_lab_StageDir;
		private System.Windows.Forms.Label m_lab_StageMode;
		private System.Windows.Forms.Label m_stc_StageCount;
		private System.Windows.Forms.Label m_lab_StageCount;
		private System.Windows.Forms.Label m_stc_SucCount;
		private System.Windows.Forms.Label m_lab_SucCount;
		private System.Windows.Forms.Label m_stc_FailCount;
		private System.Windows.Forms.Label m_lab_FailCount;
		private System.Windows.Forms.ListBox m_lst_Result;
        private System.Windows.Forms.DataGridView m_dgv_Detail;
		private System.Windows.Forms.Button m_btn_Test;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_dgv_colFeature;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_dgv_colEasy;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_dgv_colNormal;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_dgv_colHard;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_dgv_sup_colEasy;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_dgv_sup_colNormal;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_dgv_sup_colHard;
	}
}

