using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace Jop_Offers_Website.Models
{
    //الكلاس ده هو ناتج العلاقة مينى تو مينى بين المتقدم للوظيفة اي اليوزر والوظيفة نفسهااى الجوب
    //فهنعمل نافيجاشن بروبيتيز هنا مابين الكلاس ده وكلاس اليوزر وكلاس الجوب
    //كلاس الجوب احنا مكريتينه وكلاس اليوزر نازل مع الايدنتيتى اسمه ابليكاشن يوزر وفى الدتا بيز اترجم لجدول اسمه ايه اس بى نت يوزرز
    //يعنى حلاصة الكلاس ده انه هيتحول لجدول فى الداتابيز يقولك مين قدم على ايه امتى
    public class ApplyForJob
    {
        public int Id { get; set; } //اى دى عمليه التقدم عالوظيفه 
        public string Message { get; set; } //رسالة بيبعتها اللى مقدم عالوظيفه اي اليوزر للبابليشر اى معلومات او السي فى مثلا 
        public DateTime ApplyDate { get; set; } //تاريخ التقدم عالوظيفة
        public int JobId { get; set; } 
        public string UserId{ get; set; }

        public virtual Job jop { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}