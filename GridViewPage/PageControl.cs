using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GridViewPage
{
    public partial class PageControl : UserControl
    {
        public delegate void ClickPageButton(int current);
        public event ClickPageButton ClickPageButtonEvent;

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 页容量
        /// </summary>
        public int pageSize { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        private int _pageIndex;
        public int pageIndex
        {
            get { return this._pageIndex; }
            set
            {
                this._pageIndex = value;
                this.labCurrentPage.Text = value.ToString();
            }
        }
        //数据总量
        public int TotalCount
        {
            get
            {
                return int.Parse(labCount.Text);
            }
            set
            {
                labCount.Text = "共 "+ value.ToString() + " 条数据";
                TotalPages = value % pageSize > 0 ? value / pageSize + 1 : value / pageSize;
                this.labPageCount.Text = TotalPages.ToString();
            }
        }

        public PageControl()
        {
            InitializeComponent();
            this.pageIndex = 1;
            this.pageSize = 10;
            this.btnFirst.Tag = "F";
            this.btnPre.Tag = "P";
            this.btnNext.Tag = "N";
            this.btnLast.Tag = "L";
            this.btnFirst.Click += btn_Click;
            this.btnPre.Click += btn_Click;
            this.btnNext.Click += btn_Click;
            this.btnLast.Click += btn_Click;
        }
        void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (null != this.ClickPageButtonEvent)
            {
                if (null != btn)
                {
                    switch (btn.Tag.ToString())
                    {
                        case "F":
                            this.pageIndex = 1;
                            break;
                        case "P":
                            this.pageIndex = this.pageIndex <= 1 ? 1 : this.pageIndex - 1;
                            break;
                        case "N":
                            this.pageIndex = this.pageIndex >= this.TotalPages ? this.TotalPages : this.pageIndex + 1;
                            break;
                        case "L":
                            this.pageIndex = this.TotalPages;
                            break;
                        default:
                            this.pageIndex = 1;
                            break;
                    }
                    this.ClickPageButtonEvent(this.pageIndex);
                }
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (null != this.ClickPageButtonEvent)
            {
                int index=int.Parse(this.txtPageIndex.Text);
                if (index <= 1) this.txtPageIndex.Text = "1";
                if (index >= this.TotalPages) this.txtPageIndex.Text = this.TotalPages.ToString();
                pageIndex = int.Parse(this.txtPageIndex.Text);
                this.ClickPageButtonEvent(int.Parse(this.txtPageIndex.Text));
            }
        }

        private void comPageSize_SelectedValueChanged(object sender, EventArgs e)
        {
            this.pageSize = int.Parse(comPageSize.SelectedItem.ToString());
            this.pageIndex = 1;
            this.ClickPageButtonEvent(1);
        }
    }
}
