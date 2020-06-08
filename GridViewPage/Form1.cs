using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GridViewPage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pageControl1.ClickPageButtonEvent += new PageControl.ClickPageButton(pageControl1_ClickPageButtonEvent);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetFilter();
            GetData(1);
        }

        void pageControl1_ClickPageButtonEvent(int current)
        {
            GetData(current);
        }

        /// <summary>
        /// 筛选条件
        /// </summary>
        public Filter currentFilter;
        public void GetFilter()
        {
            Filter filter = new Filter();
            filter.tableName = "tb_user";

            this.currentFilter = filter;
        }

        public void GetData(int pageIndex)
        {
            var result = DataHelp.GetPageData((pageIndex - 1) * pageControl1.pageSize, pageControl1.pageSize, currentFilter);
            dataGridView1.DataSource = result;

            var count = DataHelp.GetDataCount(currentFilter);
            pageControl1.TotalCount = count;
        }
    }

    #region MyRegion
    public class Filter
    {
        public string tableName { get; set; }
        public string filterString { get; set; }
    }

    public static class DataHelp
    {
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        public static DataTable GetPageData(int pageIndex, int pageSize, Filter filter)
        {
            if (string.IsNullOrEmpty(filter.filterString)) filter.filterString = "1=1";
            string sql = "select * from {0} where {3} limit {1},{2};";
            String sql_exe = string.Format(sql, filter.tableName, pageIndex, pageSize, filter.filterString);
            var data = DataOperate.getds(sql_exe);
            return data.Tables[0];
        }

        /// <summary>
        /// 获取数据总量
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        public static int GetDataCount(Filter filter)
        {
            if (string.IsNullOrEmpty(filter.filterString)) filter.filterString = "1=1";
            string sql = "select count(*) from {0} where {1};";
            String sql_exe = string.Format(sql, filter.tableName, filter.filterString);
            var data = DataOperate.getds(sql_exe);
            return int.Parse(data.Tables[0].Rows[0].ItemArray[0].ToString());
        }
    }
    #endregion
}
