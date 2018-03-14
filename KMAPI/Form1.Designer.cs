namespace API教育訓練
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.JsonResultTxt = new System.Windows.Forms.TextBox();
            this.SearchBtn = new System.Windows.Forms.Button();
            this.SearchText = new System.Windows.Forms.TextBox();
            this.SearchGetAll = new System.Windows.Forms.Button();
            this.DocIdOrUserId = new System.Windows.Forms.TextBox();
            this.GetTag = new System.Windows.Forms.Button();
            this.GetHotTag = new System.Windows.Forms.Button();
            this.GetAttachment = new System.Windows.Forms.Button();
            this.AttDocID = new System.Windows.Forms.TextBox();
            this.AttVerNo = new System.Windows.Forms.TextBox();
            this.AttFileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAdvKeyword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAdvFolderID = new System.Windows.Forms.TextBox();
            this.btnAdvSearch = new System.Windows.Forms.Button();
            this.btnDeleteDoc = new System.Windows.Forms.Button();
            this.txtDeleteDocID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSearchByID = new System.Windows.Forms.Button();
            this.txtSearchByID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnNewFolder = new System.Windows.Forms.Button();
            this.txtNewFolderParentId = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtNewFolderName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 237);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(362, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "建立KM文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // JsonResultTxt
            // 
            this.JsonResultTxt.Location = new System.Drawing.Point(7, 12);
            this.JsonResultTxt.Multiline = true;
            this.JsonResultTxt.Name = "JsonResultTxt";
            this.JsonResultTxt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.JsonResultTxt.Size = new System.Drawing.Size(362, 214);
            this.JsonResultTxt.TabIndex = 1;
            // 
            // SearchBtn
            // 
            this.SearchBtn.Location = new System.Drawing.Point(138, 328);
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(104, 22);
            this.SearchBtn.TabIndex = 2;
            this.SearchBtn.Text = "搜尋並取回一筆";
            this.SearchBtn.UseVisualStyleBackColor = true;
            this.SearchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
            // 
            // SearchText
            // 
            this.SearchText.Location = new System.Drawing.Point(10, 328);
            this.SearchText.Name = "SearchText";
            this.SearchText.Size = new System.Drawing.Size(122, 22);
            this.SearchText.TabIndex = 3;
            // 
            // SearchGetAll
            // 
            this.SearchGetAll.Location = new System.Drawing.Point(244, 328);
            this.SearchGetAll.Name = "SearchGetAll";
            this.SearchGetAll.Size = new System.Drawing.Size(125, 22);
            this.SearchGetAll.TabIndex = 4;
            this.SearchGetAll.Text = "搜尋並取回全部結果";
            this.SearchGetAll.UseVisualStyleBackColor = true;
            this.SearchGetAll.Click += new System.EventHandler(this.SearchGetAll_Click);
            // 
            // DocIdOrUserId
            // 
            this.DocIdOrUserId.Location = new System.Drawing.Point(10, 399);
            this.DocIdOrUserId.Name = "DocIdOrUserId";
            this.DocIdOrUserId.Size = new System.Drawing.Size(122, 22);
            this.DocIdOrUserId.TabIndex = 5;
            // 
            // GetTag
            // 
            this.GetTag.Location = new System.Drawing.Point(138, 399);
            this.GetTag.Name = "GetTag";
            this.GetTag.Size = new System.Drawing.Size(104, 22);
            this.GetTag.TabIndex = 6;
            this.GetTag.Text = "取某文件之標籤";
            this.GetTag.UseVisualStyleBackColor = true;
            this.GetTag.Click += new System.EventHandler(this.GetTag_Click);
            // 
            // GetHotTag
            // 
            this.GetHotTag.Location = new System.Drawing.Point(244, 399);
            this.GetHotTag.Name = "GetHotTag";
            this.GetHotTag.Size = new System.Drawing.Size(125, 22);
            this.GetHotTag.TabIndex = 7;
            this.GetHotTag.Text = "取熱門標籤";
            this.GetHotTag.UseVisualStyleBackColor = true;
            this.GetHotTag.Click += new System.EventHandler(this.GetHotTag_Click);
            // 
            // GetAttachment
            // 
            this.GetAttachment.Location = new System.Drawing.Point(244, 438);
            this.GetAttachment.Name = "GetAttachment";
            this.GetAttachment.Size = new System.Drawing.Size(125, 51);
            this.GetAttachment.TabIndex = 8;
            this.GetAttachment.Text = "取得某文件的某附檔";
            this.GetAttachment.UseVisualStyleBackColor = true;
            this.GetAttachment.Click += new System.EventHandler(this.GetAttachment_Click);
            // 
            // AttDocID
            // 
            this.AttDocID.Location = new System.Drawing.Point(57, 439);
            this.AttDocID.Name = "AttDocID";
            this.AttDocID.Size = new System.Drawing.Size(75, 22);
            this.AttDocID.TabIndex = 9;
            // 
            // AttVerNo
            // 
            this.AttVerNo.Location = new System.Drawing.Point(182, 438);
            this.AttVerNo.Name = "AttVerNo";
            this.AttVerNo.Size = new System.Drawing.Size(52, 22);
            this.AttVerNo.TabIndex = 9;
            // 
            // AttFileName
            // 
            this.AttFileName.Location = new System.Drawing.Point(86, 467);
            this.AttFileName.Name = "AttFileName";
            this.AttFileName.Size = new System.Drawing.Size(148, 22);
            this.AttFileName.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 444);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "DocID：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(133, 443);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "VerNo：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 472);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "AttFileName：";
            // 
            // txtAdvKeyword
            // 
            this.txtAdvKeyword.Location = new System.Drawing.Point(66, 287);
            this.txtAdvKeyword.Name = "txtAdvKeyword";
            this.txtAdvKeyword.Size = new System.Drawing.Size(77, 22);
            this.txtAdvKeyword.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 290);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "keyword：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(149, 290);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "FolderID：";
            // 
            // txtAdvFolderID
            // 
            this.txtAdvFolderID.Location = new System.Drawing.Point(206, 287);
            this.txtAdvFolderID.Name = "txtAdvFolderID";
            this.txtAdvFolderID.Size = new System.Drawing.Size(36, 22);
            this.txtAdvFolderID.TabIndex = 16;
            // 
            // btnAdvSearch
            // 
            this.btnAdvSearch.Location = new System.Drawing.Point(244, 287);
            this.btnAdvSearch.Name = "btnAdvSearch";
            this.btnAdvSearch.Size = new System.Drawing.Size(125, 23);
            this.btnAdvSearch.TabIndex = 17;
            this.btnAdvSearch.Text = "進階搜尋";
            this.btnAdvSearch.UseVisualStyleBackColor = true;
            this.btnAdvSearch.Click += new System.EventHandler(this.btnAdvSearch_Click);
            // 
            // btnDeleteDoc
            // 
            this.btnDeleteDoc.Location = new System.Drawing.Point(244, 502);
            this.btnDeleteDoc.Name = "btnDeleteDoc";
            this.btnDeleteDoc.Size = new System.Drawing.Size(123, 23);
            this.btnDeleteDoc.TabIndex = 18;
            this.btnDeleteDoc.Text = "刪除文件By DocID";
            this.btnDeleteDoc.UseVisualStyleBackColor = true;
            this.btnDeleteDoc.Click += new System.EventHandler(this.btnDeleteDoc_Click);
            // 
            // txtDeleteDocID
            // 
            this.txtDeleteDocID.Location = new System.Drawing.Point(86, 502);
            this.txtDeleteDocID.Name = "txtDeleteDocID";
            this.txtDeleteDocID.Size = new System.Drawing.Size(148, 22);
            this.txtDeleteDocID.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 507);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "DocID：";
            // 
            // btnSearchByID
            // 
            this.btnSearchByID.Location = new System.Drawing.Point(244, 364);
            this.btnSearchByID.Name = "btnSearchByID";
            this.btnSearchByID.Size = new System.Drawing.Size(125, 23);
            this.btnSearchByID.TabIndex = 21;
            this.btnSearchByID.Text = "搜尋文件By DocID";
            this.btnSearchByID.UseVisualStyleBackColor = true;
            this.btnSearchByID.Click += new System.EventHandler(this.btnSearchByID_Click);
            // 
            // txtSearchByID
            // 
            this.txtSearchByID.Location = new System.Drawing.Point(86, 364);
            this.txtSearchByID.Name = "txtSearchByID";
            this.txtSearchByID.Size = new System.Drawing.Size(148, 22);
            this.txtSearchByID.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(34, 369);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 12);
            this.label7.TabIndex = 23;
            this.label7.Text = "DocID：";
            // 
            // btnNewFolder
            // 
            this.btnNewFolder.Location = new System.Drawing.Point(244, 536);
            this.btnNewFolder.Name = "btnNewFolder";
            this.btnNewFolder.Size = new System.Drawing.Size(123, 50);
            this.btnNewFolder.TabIndex = 24;
            this.btnNewFolder.Text = "新增子文件夾";
            this.btnNewFolder.UseVisualStyleBackColor = true;
            this.btnNewFolder.Click += new System.EventHandler(this.btnNewFolder_Click_1);
            // 
            // txtNewFolderParentId
            // 
            this.txtNewFolderParentId.Location = new System.Drawing.Point(114, 536);
            this.txtNewFolderParentId.Name = "txtNewFolderParentId";
            this.txtNewFolderParentId.Size = new System.Drawing.Size(120, 22);
            this.txtNewFolderParentId.TabIndex = 26;
            this.txtNewFolderParentId.Text = "3";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 541);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 12);
            this.label8.TabIndex = 25;
            this.label8.Text = "Parent Folder ID：";
            // 
            // txtNewFolderName
            // 
            this.txtNewFolderName.Location = new System.Drawing.Point(114, 564);
            this.txtNewFolderName.Name = "txtNewFolderName";
            this.txtNewFolderName.Size = new System.Drawing.Size(120, 22);
            this.txtNewFolderName.TabIndex = 27;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 567);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 12);
            this.label9.TabIndex = 28;
            this.label9.Text = "New Folder Name：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 605);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtNewFolderName);
            this.Controls.Add(this.txtNewFolderParentId);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnNewFolder);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtSearchByID);
            this.Controls.Add(this.btnSearchByID);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtDeleteDocID);
            this.Controls.Add(this.btnDeleteDoc);
            this.Controls.Add(this.btnAdvSearch);
            this.Controls.Add(this.txtAdvFolderID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtAdvKeyword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AttFileName);
            this.Controls.Add(this.AttVerNo);
            this.Controls.Add(this.AttDocID);
            this.Controls.Add(this.GetAttachment);
            this.Controls.Add(this.GetHotTag);
            this.Controls.Add(this.GetTag);
            this.Controls.Add(this.DocIdOrUserId);
            this.Controls.Add(this.SearchGetAll);
            this.Controls.Add(this.SearchText);
            this.Controls.Add(this.SearchBtn);
            this.Controls.Add(this.JsonResultTxt);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox JsonResultTxt;
        private System.Windows.Forms.Button SearchBtn;
        private System.Windows.Forms.TextBox SearchText;
        private System.Windows.Forms.Button SearchGetAll;
        private System.Windows.Forms.TextBox DocIdOrUserId;
        private System.Windows.Forms.Button GetTag;
        private System.Windows.Forms.Button GetHotTag;
        private System.Windows.Forms.Button GetAttachment;
        private System.Windows.Forms.TextBox AttDocID;
        private System.Windows.Forms.TextBox AttVerNo;
        private System.Windows.Forms.TextBox AttFileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAdvKeyword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAdvFolderID;
        private System.Windows.Forms.Button btnAdvSearch;
        private System.Windows.Forms.Button btnDeleteDoc;
        private System.Windows.Forms.TextBox txtDeleteDocID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSearchByID;
        private System.Windows.Forms.TextBox txtSearchByID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnNewFolder;
        private System.Windows.Forms.TextBox txtNewFolderParentId;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtNewFolderName;
        private System.Windows.Forms.Label label9;
       
    }
}

