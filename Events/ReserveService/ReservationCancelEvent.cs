using Ergate;

namespace Ergate.Events.ReserveService
{
    /// <summary>
    /// 预约单取消事件
    /// </summary>
    public class ReservationCancelEvent : BaseEvent
    {

        /// <summary>
        /// 预约单ID
        /// </summary>
        public int ReserveId { get; set; }

        /// <summary>
        /// 预约单类型  1--挂号，2--美容
        /// </summary>
        public int SourceType { get; set; }

        /// <summary>
        /// 取消类型
        /// </summary>
        public int CancelType { get; set; }

        /// <summary>
        /// 其它取消原因
        /// </summary>
        public string CancelReason { get; set; }
    }
}
