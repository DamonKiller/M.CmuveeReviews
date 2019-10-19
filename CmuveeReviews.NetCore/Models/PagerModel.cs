using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CmuveeReviews.NetCore.Models
{
    /// <summary>
    /// 分页基类 
    /// </summary>
    public class BasePager
    {
        /// <summary>
        /// 获取或设置是否显示分页数量
        /// </summary>
        public bool IsShowPageSize { get; set; }

        public bool IsShowGoto { get; set; }

        private string _PaginationID = "Pagination";
        public string PaginationID
        {
            set
            {
                _PaginationID = value;
            }
            get
            {
                return _PaginationID;
            }
        }
        private string _TolCountID = "TolCount";
        public string TolCountID
        {
            set { _TolCountID = value; }
            get { return _TolCountID; }
        }

        /// <summary>
        /// 自定义文本
        /// </summary>
        public string CustomText { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPageIndex { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// 每页显示的记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总页数(只读)
        /// </summary>
        public int TotalPageCount
        {
            get
            {
                double pageCount = (double)TotalItemCount / (double)PageSize;
                pageCount = Math.Ceiling(pageCount == 0 ? 1 : pageCount);
                return (int)pageCount;
            }
        }
    }

    /// <summary>
    /// 分页实体类
    /// </summary>
    /// <typeparam name="T">要进行分页的对象</typeparam>
    public class PagerModel<T> : BasePager
    {
        public PagerModel()
        {
        }
        public PagerModel(int PageSize, int CurrentPageIndex, int TotalItemCount, List<T> Data)
        {
            this.PageSize = PageSize;
            this.TotalItemCount = TotalItemCount;
            this.DataSource = Data;
            this.IsShowGoto = false;
            this.IsShowPageSize = false;
            if (CurrentPageIndex > TotalPageCount)
            {
                this.CurrentPageIndex = TotalPageCount;
            }
            else
            {
                this.CurrentPageIndex = CurrentPageIndex;
            }
        }
        public PagerModel(int PageSize, int CurrentPageIndex, int TotalItemCount, List<T> Data,
            bool IsShowPageSize = false, bool IsShowGoto = false, string CustomText = null)
        {
            this.PageSize = PageSize;
            this.TotalItemCount = TotalItemCount;
            this.DataSource = Data;
            this.IsShowPageSize = IsShowPageSize;
            this.CustomText = CustomText;
            this.IsShowGoto = IsShowGoto;
            if (CurrentPageIndex > TotalPageCount)
            {
                this.CurrentPageIndex = TotalPageCount;
            }
            else
            {
                this.CurrentPageIndex = CurrentPageIndex;
            }
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        public List<T> DataSource { get; set; }

    }
}
