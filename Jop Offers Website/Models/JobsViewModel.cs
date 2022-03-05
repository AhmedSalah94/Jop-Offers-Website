using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jop_Offers_Website.Models
{
    public class JobsViewModel
    {
        public string JobTitle { get; set; } //الوظيفة
        public IEnumerable <ApplyForJob> Items { get; set; }//الاشخاص اللي قدموا على الوظيفة
    }
}
// عملنا ال 2 فيو مودل دول عشان احتجنا ال 2 يروبرتى دول مع بعض وهما لا 
//موجودين فى المودل جوب مع بعض ولا فى المودل ابلاى فور جوب