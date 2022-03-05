using Jop_Offers_Website.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(db.Categories.ToList()) ;
        }

        public ActionResult Details (int JobId)
        {
            var job = db.Jobs.Find(JobId);

            if (job == null)
            {
                return HttpNotFound();
            }

            Session["JobId"] = JobId;//هنحرن فى سيشن ديكشنري الاي دى بتاع الجوب الظاهر تفاصيله حاليا

            return View(job);
        }


        [Authorize]
        public ActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Apply(string Message)
        //لما اليوزر يدوس ابلاى هيرسل الداتا(البروبرتيز) اللى فى كلاس اى جدول ابلاي فور جوب ..
        //اى هيرسل المسج اللى هنستقبلها فى بارميتر والباقى هنستقبله فى بودي الاكشن دى زى ماهتشوف كده
        {
            var UserId = User.Identity.GetUserId();//هترجع الاي دي بتاع اليوزر المتصل حاليا
            var JobId = (int)Session["JobId"]; //الاي دى اللى خزناه في السيشن ده فى الديتيلز اكشن هنحولها ل انت 

            var check = db.ApplyForJobs.Where(a => a.UserId == UserId && a.JobId == JobId).ToList();
            //مش عايزين اليوزر يثدم على وظيفة واحدة مرتين فهنستخدم تشيك بالشكل ده
            //ومعناها دورلى فى الجدول ابلى فور جوبز على اي عنصر الجوب اي دى واليوزر اى دى بتوعه = اللى معرفينهم هنا دول
            //وهاتهملى في ليست وخزنهم فى تشيك لو عددهم اقل من 1 يعنى 0 ده معناه ان اليوزر ده مقدمش عالوظيفه 
            //دى قبل كده فاقبل تقديمه ..لو عددهم 1 فحلاص متقبلش لانه قدم قبل كده
            if (check.Count < 1)
            {
                var job = new ApplyForJob();

                job.UserId = UserId;
                job.JobId = JobId;
                job.Message = Message;
                job.ApplyDate = DateTime.Now;

                db.ApplyForJobs.Add(job);
                db.SaveChanges();

                ViewBag.result = "تمت الاضافة بنجاح";
            }

            else
            {
                ViewBag.result = "المعذرة, لقد سبق وتقدمت الى نفس الوظيفة!";
            }

            return View();
        }


        //الاكشن دى بتجيب كل الوظائف اللى قدم عليها اليوزر الواحد
        [Authorize]
        public ActionResult GetJobsByUser()
        {
            var UserId = User.Identity.GetUserId(); //هترجع الاي دي بتاع اليوزر المتصل حاليا
            var Jobs = db.ApplyForJobs.Where(a => a.UserId == UserId);
            return View(Jobs.ToList());
        }


        //عرض تفاصيل الوظيفة التى يقوم باختيارها اليوزر من بين قائمة الوظائف التى قدم عليها
        [Authorize]
        public ActionResult DetailsOfJob(int Id)
        {
            var job = db.ApplyForJobs.Find(Id);

            if (job == null)
            {
                return HttpNotFound();
            }

            return View(job);
        }


        //الاكشن دى بتجيب كل الوظائف اللى نشرها اليوزر الحالي والناس قدمت عليها
        //هنربط بين الجدولين ونجيب العناصر المشتركة اللي بينهم بحيث الاي دى اللى 
        //نشر الوظائف هو الاي دى المسجل دخول حاليا
        [Authorize]
        public ActionResult GetJobsByPublisher()
        {
            var UserId = User.Identity.GetUserId();

            var Jobs = from app in db.ApplyForJobs
                       join job in db.Jobs
                       on app.JobId equals job.id
                       where job.User.Id == UserId
                       select app;

            var grouped = from j in Jobs
                          group j by j.jop.JobTitle
                          into gr
                          select new JobsViewModel //هنخزن ناتج عملية الجروبنج غى الفيو مودل ده
                          {
                              JobTitle = gr.Key, //الكى هو اللى عملت بيه جروبنج اللى هو جوب تايتل
                              Items = gr //الناس اللي قدمت عالوظيفة
                          };

            return View(grouped.ToList());
        }

        public ActionResult Edit(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        [HttpPost]
        public ActionResult Edit(ApplyForJob job)
        {
            if (ModelState.IsValid)
            {
                job.ApplyDate = DateTime.Now;
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetJobsByUser");
            }
            return View(job);
        }

        public ActionResult Delete(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(ApplyForJob job)
        {

            
            var myjob = db.ApplyForJobs.Find(job.Id);
            db.ApplyForJobs.Remove(myjob);
            db.SaveChanges();
            return RedirectToAction("GetJobsByUser");

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        //لما تكريت فيو الكونتاكت لست فى حاجة للداتا كونتيكست كلاس فسيب اخر اوبشن فاضى لان الكونتاكت مودل عير متصل بالداتابيز
        [HttpPost]
        public ActionResult Contact(ContactModel contact)
        {
            var Mail = new MailMessage(); //انشأنا ايميل
            Mail.From = new MailAddress(contact.Email); //المرسل
            Mail.To.Add(new MailAddress("Salahwekash@gmail.com"));//المستفبل
            Mail.Subject = contact.Subject;//موضوع الرسالة
            Mail.IsBodyHtml = true; //لتصير الرسالة اى البدى على شكل اتش تى ام ال وليس تيكست
            string body = "اسم المرسل :" + contact.Name + "<br>" +
                 "بريد المرسل :" + contact.Email + "<br>" +
                  "عنوان الرسالة :" + contact.Subject + "<br>" +
                   "نص الرسالة :<b>" + contact.Message+"</b>";
            Mail.Body = body;//الرسالة

            // عشان نبعت ميل فى الدوت نت لازم نتعامل مه الكلاس SmtpClient
            //اللي بنحدد من خلالها السرفر والبورت وبعض معطيات الارسال الاخري
            //السرفر هنا او الهوست هو الجيميل اول بارميتر ممكم تتعامل مع هوست اخر زي ياهو مثلا عادى
            //البارميتر التانى هو البورت ويختلف من سرفر لاخر ..ده بتاع الجيميل

            //هنا عملنا المرسل والمستقبل واحد فلما ابعت رسالة هبعتها لنفسي
            var LoginInfo = new NetworkCredential("Salahwekash@gmail.com", "write pw here from vedeo on device to avoid smtp error");//معلومات الارسال والبارميتر التاني ده الباسورد
                                                                                  //  ولانه كان ايرور ففى فديو شارحه (اكتبه منه) عالجهاز فى فولدر الام في سي اسمه Solved smtp server requires a secure connection 
            var smtpClient = new SmtpClient("smtp.gmail.com" , 587);               //لو بعت ايميل هيظهرلك ايرور حله انك تكتب الباسورد فى البارميتر التانى زي الفيديو اللى عالجهاز
            smtpClient.EnableSsl = true; //خلينا الجميل يسمح بالوضع الامن فى عملية تحويل البيانات من البراوزر للسرفر   
            smtpClient.Credentials = LoginInfo; //الارسال هيتم من خلال المعطيات دى
            smtpClient.Send(Mail);
            return RedirectToAction("Index"); //لما ترسل الرسالة بنجاح روح للاندكس
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string SearchName)
        {
            var result = db.Jobs.Where(a => a.JobTitle.Contains(SearchName)
             || a.JobContent.Contains(SearchName)
             || a.Category.CategoryName.Contains(SearchName)
             || a.Category.CategoryDescription.Contains(SearchName)).ToList();
            //هنبحث عن حاجة من ال4 دول


            return View(result);
        }
    }
}