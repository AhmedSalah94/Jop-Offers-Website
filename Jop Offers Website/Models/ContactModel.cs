using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jop_Offers_Website.Models
{
    public class ContactModel
    {
        [Required]
        public string Name { get; set; } //اسم المرسل

        [Required]
        public string Email { get; set; } //بريج المرسل


        [Required]
        public string Subject { get; set; } //موضوع الرسالة

        [Required]
        public string Message { get; set; } //الرسالة

    }
}