using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.ORMData;

namespace CmuveeReviews.NetCore.Models
{
    [Table(TableName ="Movies")]
    public class Movies:ORMBase
    {
        [Column(IsPrimary =true,IsIdentity =false)]
        public string SourceId { set; get; }
        public string Name { set; get; }
        public string AliasName { set; get; }
        public string Remark { set; get; }
        public string Starring { set; get; }
        public string Director { set; get; }
        public string Category { set; get; }
        public string Type { set; get; }
        public string Language { set; get; }
        public string Region { set; get; }
        public string Status { set; get; }
        public string Release { set; get; }
        public string DoubanScore { set; get; }
        public string Description { set; get; }
        public string PicsAddress { set; get; }
        public string PlayAddress { set; get; }
        public string CreateTime { set; get; }
    }
}
