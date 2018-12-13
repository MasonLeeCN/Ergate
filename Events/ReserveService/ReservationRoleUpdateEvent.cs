using Ergate;

namespace Ergate.Events.ReserveService
{
    public class ReservationRoleUpdateEvent:BaseEvent
    {

        /// <summary>
        /// 规则ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分院ID
        /// </summary>
        public int HospitalId { get; set; }

        /// <summary>
        /// 规则开始时间
        /// </summary>
        public string RoleStartTime { get; set; }

        /// <summary>
        /// 规则结束时间
        /// </summary>
        public string RoleEndTime { get; set; }

        /// <summary>
        /// 每小时放号量
        /// </summary>
        public int PerHourNum { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 1--挂号，2--美容
        /// </summary>
        public int SourceType { get; set; }
    }
}
