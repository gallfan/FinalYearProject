using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MechanicTracker;

namespace MechanicTracker
{
    [MetadataType(typeof(BookingMeta))]
    public partial class booking
    { }

    public class BookingMeta
    {
        [Display(Name = "Complete Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CompleteDate { get; set; }

        [Display(Name = "Email"), DataType(DataType.EmailAddress, ErrorMessage = "Must be a Email Address")]
        [Required(ErrorMessage = "Customer Email is Required"), StringLength(50, MinimumLength = 2, ErrorMessage = "Customer email must be between 2 and 50 characters long")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Booking Date is Required"), Display(Name = "Booking Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime BookingDate { get; set; }

        [Required]
        public string Status { get; set; }

        [StringLength(400, ErrorMessage = "Description is maximum 400 characters")]
        public string Description { get; set; }
    }

    [MetadataType(typeof(MakeMeta))]
    public partial class make
    { }

    public class MakeMeta
    {
        [Display(Name = "Make")]
        public string MakeName { get; set; }
    }

    [MetadataType(typeof(ModelMeta))]
    public partial class model
    { }

    public class ModelMeta
    {
        [Display(Name = "Model")]
        public string ModelName { get; set; }
    }

    [MetadataType(typeof(CarMeta))]
    public partial class car
    { }

    public class CarMeta
    {
        [Required(ErrorMessage = "Model is Required")]
        public int ModelID { get; set; }

        [Required(ErrorMessage = "Car Registration is Required"), StringLength(15, MinimumLength = 4, ErrorMessage = "Registration must be between 4 and 15 characters long")]
        public string Registration { get; set; }

        [Required(ErrorMessage = "Fuel Type is Required"), Display(Name = "Fuel Type")]
        public string FuelType { get; set; }
    }

    [MetadataType(typeof(JobCategoryMeta))]
    public partial class jobscategory
    { }

    public class JobCategoryMeta
    {
        [Display(Name = "Job Type")]
        public string Type { get; set; }
    }

    [MetadataType(typeof(JobListMeta))]
    public partial class jobslist
    { }

    public class JobListMeta
    {
        [Display(Name = "Job Name")]
        public string Name { get; set; }
    }

    [MetadataType(typeof(JobMeta))]
    public partial class job
    { }

    public class JobMeta
    {
        [Required(ErrorMessage = "Job List is Required")]
        public short JobsListID { get; set; }
        [Required(ErrorMessage = "Booking is Required")]
        public int BookingID { get; set; }
        [Display(Name = "Time Allowed")]
        public Nullable<short> TimeAllowed { get; set; }
        [Display(Name = "Time Taken")]
        public Nullable<short> TimeTaken { get; set; }
        [StringLength(2000, ErrorMessage = "Comment is maximum 2000 characters")]
        public string Comments { get; set; }
        [Required]
        public string Status { get; set; }
        [Display(Name = "Note"), StringLength(300, ErrorMessage = "Note is maximum 300 characters")]
        public string Notes { get; set; }
        [Display(Name = "Started"), DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
        public Nullable<System.DateTime> TimeStarted { get; set; }
        [Required(ErrorMessage = "Job Difficulty is Required"), Range(1, 5, ErrorMessage = "Must be between 1 and 5")]
        public sbyte Difficulty { get; set; }
        [Display(Name = "Complete Date"), DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> CompleteDate { get; set; }
    }

    [MetadataType(typeof(MechanicMeta))]
    public partial class mechanic
    { }

    public class MechanicMeta
    {
        [Display(Name = "First Name"), Required(ErrorMessage = "First Name is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 30 charaters long")]
        public string Firstname { get; set; }
        [Display(Name = "Last Name"), Required(ErrorMessage = "Last Name is Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 charaters long")]
        public string LastName { get; set; }

        [Display(Name = "Email"), DataType(DataType.EmailAddress, ErrorMessage = "Must be a Email Address"), Required(ErrorMessage = "Email is Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Email must be between 2 and 50 characters long")]
        public string Email { get; set; }

        [Display(Name = "Phone Number"), DataType(DataType.PhoneNumber, ErrorMessage = "Must be a phone number"), Required(ErrorMessage = "Phone Number is Required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Skill level is Required"), Range(1, 5, ErrorMessage = "Must be between 1 and 5"), Display(Name = "Skill Level")]
        public sbyte SkillLevel { get; set; }
        public bool Available { get; set; }
        [Required(ErrorMessage = "Username is Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 50 charaters long")]
        public string Username { get; set; }
    }

    [MetadataType(typeof(TimesheetMeta))]
    public partial class timessheet
    { }

    public class TimesheetMeta
    {
        [Required(ErrorMessage = "Job is Required")]
        public int JobID { get; set; }
        [Required(ErrorMessage = "Start Time is Required"), Display(Name = "Start Time")]
        [DataType(DataType.DateTime, ErrorMessage = "Must be a Date Time")]
        public System.DateTime StartTime { get; set; }
        [Display(Name = "End Time"), DataType(DataType.DateTime, ErrorMessage = "Must be a Date Time")]
        public Nullable<System.DateTime> EndTime { get; set; }
    }

    [MetadataType(typeof(NotificationMeta))]
    public partial class notification
    { }

    public class NotificationMeta
    {
        [Required(ErrorMessage = "Mechanic is Required")]
        public int MechanicID { get; set; }
        [Required(ErrorMessage = "Date is Required"), DataType(DataType.DateTime, ErrorMessage = "Must be a Date Time")]
        public System.DateTime Date { get; set; }
        [Required(ErrorMessage = "Message is Required"), StringLength(1000, ErrorMessage = "Message is maximum 1000 characters")]
        public string Message { get; set; }
        public bool Read { get; set; }
    }
}