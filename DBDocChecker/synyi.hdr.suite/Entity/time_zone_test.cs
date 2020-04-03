using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
namespace synyi.hdr.suite.Entity
{
    [Table("time_zone_test")]
    public class time_zone_test
    {
        [ExplicitKey]
        [Key]
        public int id { get; set; }
        public DateTime time_with_tz { get; set; }
        public DateTime time_without_tz { get; set; }
    }
}
