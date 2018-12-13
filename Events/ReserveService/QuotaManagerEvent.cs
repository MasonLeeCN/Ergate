using Ergate;

namespace Ergate.Events.ReserveService
{
    /// <summary>
    /// 号源管理(开启,关闭)
    /// </summary>
    public class QuotaManagerEvent : BaseEvent
    {
        /// <summary>
        /// 医院ID
        /// </summary>
        public int HospitalId { get; set; }

        /// <summary>
        /// 号源类型  1--预约挂号，2--预约美容
        /// </summary>
        public int SourceType { get; set; }

        /// <summary>
        /// 年月日
        /// </summary>
        public string YearMonthDay { get; set; }

        /// <summary>
        /// 时分
        /// </summary>
        public string HourMin { get; set; }

        /// <summary>
        /// 操作类型(0--关闭，1--开启)
        /// </summary>
        public int ActionType { get; set; }

        /// <summary>
        /// 操作数量
        /// </summary>
        public int ActionNum { get; set; }
    }
}
