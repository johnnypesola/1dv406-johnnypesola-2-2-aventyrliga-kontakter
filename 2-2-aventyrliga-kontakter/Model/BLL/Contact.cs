using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace _2_2_aventyrliga_kontakter.Model
{
    public class Contact
    {
        public int ContactId { get; set; }

        [Required(ErrorMessage = "Ett förnamn måste anges.")]
        [StringLength(50, ErrorMessage = "Förnamnet får bestå av max 50 tecken.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Ett efternamn måste anges.")]
        [StringLength(50, ErrorMessage = "Efternamnet får bestå av max 50 tecken.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "En e-postadress måste anges.")]
        [StringLength(50, ErrorMessage = "E-postadressen får bestå av max 50 tecken.")]
        [EmailAddress(ErrorMessage = "Den angivna e-postadressen kunde inte tolkas som en giltig e-postadress.")]
        public string EmailAddress { get; set; }
    }
}