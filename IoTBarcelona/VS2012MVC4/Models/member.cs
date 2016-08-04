using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Barcelona.Models
{
    public class BethelMessage
    {
        public string result { get; set; }
        public string msgCode { get; set; }
        public debug debug { get; set; }
        public member member { get; set; }
    }

    public class debug
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class UserCard
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string CardId { get; set; }
        public DateTime cdt { get; set; }
    }

    public class member
    {
        [Key]
        public string ID_NO { get; set; }   //會員編號(UniqueID)，可變長度字串
        public string VIP_CODE { get; set; }　//會員編號(該Site 下的會員編號)，字串固定為八碼
        public string SITE { get; set; }    //加⼊入服務的企業識別碼
        public string Name { get; set; }    //會員姓名
        public string Email { get; set; }   //會員電⼦子郵件，可變⾧長度字串
        public string Store { get; set; }   //服務⾨門市
        public string Remark { get; set; }  //資料備註，自行附註之說明文字
    }
    /* Example:
     {result:"OK",msgCode:"100",msg:"查詢成功",debug:{xxxxxxxxxxx},
        member:
        [
            {ID_NO:"1001",VIP_CODE:"00000011",SITE:"BSMS002037",Name:"王大明",Email:"1001@mail.com",Store:"美的適中豐店",
            Remark:"sample remark data"}
        ],
        [
            {ID_NO:"1002",Vip_code:"00000012",Site:"BSMS002037",Name:"李小美",Email:"1002@mail.com",Store:"美的適榮民店",
            Remark:"sample remark data"}
        ]
     }
     
     */
}