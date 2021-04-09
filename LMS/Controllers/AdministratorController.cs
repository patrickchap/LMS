using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LMS.Models.LMSModels;

namespace LMS.Controllers
{
  [Authorize(Roles = "Administrator")]
  public class AdministratorController : CommonController
  {
    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Department(string subject)
    {
      ViewData["subject"] = subject;
      return View();
    }

    public IActionResult Course(string subject, string num)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      return View();
    }

    /// <summary>
    /// Returns a JSON array of all the courses in the given department.
    /// Each object in the array should have the following fields:
    /// "number" - The course number (as in 5530)
    /// "name" - The course name (as in "Database Systems")
    /// </summary>
    /// <param name="subject">The department subject abbreviation (as in "CS")</param>
    /// <returns>The JSON result</returns>
    public IActionResult GetCourses(string subject)
    {
            var query = from c in db.Courses
                        where c.DeptAbbr == subject
                        select new
                        {
                            number = c.Number,
                            name = c.Name
                        };
            return Json(query.ToArray());
        }


    


    /// <summary>
    /// Returns a JSON array of all the professors working in a given department.
    /// Each object in the array should have the following fields:
    /// "lname" - The professor's last name
    /// "fname" - The professor's first name
    /// "uid" - The professor's uid
    /// </summary>
    /// <param name="subject">The department subject abbreviation</param>
    /// <returns>The JSON result</returns>
    public IActionResult GetProfessors(string subject)
    {
            var query = from p in db.Professors
                        where p.DeptWorksIn == subject
                        select new
                        {
                            lname = p.LastName,
                            fname = p.FirstName,
                            uid = p.UId
                        };
            return Json(query.ToArray());
        }



    /// <summary>
    /// Creates a course.
    /// A course is uniquely identified by its number + the subject to which it belongs
    /// </summary>
    /// <param name="subject">The subject abbreviation for the department in which the course will be added</param>
    /// <param name="number">The course number</param>
    /// <param name="name">The course name</param>
    /// <returns>A JSON object containing {success = true/false}.
    /// false if the course already exists, true otherwise.</returns>
    public IActionResult CreateCourse(string subject, int number, string name)
    {
            var query2 = from r in db.Courses
                         where(r.DeptAbbr == subject && r.Number == number)
                         select r;
            if (query2.Any())
            {
                return Json(new { success = false });
            }

            Courses c = new Courses();
            c.DeptAbbr = subject;
            c.Number = number;
            c.Name = name;

            var query = from o in db.Courses
                        select new
                        {
                            id = o.CatalogId
                        };

            int max = 0;
            foreach (var x in query)
            {
              
                int id = Int32.Parse(x.id);

                if (id > max)
                {
                    max = id;
                }
            }
            max += 1;
            string idNumber = max.ToString();
            while(idNumber.Length < 5)
            {
                idNumber = "0" + idNumber;
            }
            c.CatalogId = idNumber;

            db.Courses.Add(c);
            db.SaveChanges();

            return Json(new { success = true });
    }



        /// <summary>
        /// Creates a class offering of a given course.
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <param name="number">The course number</param>
        /// <param name="season">The season part of the semester</param>
        /// <param name="year">The year part of the semester</param>
        /// <param name="start">The start time</param>
        /// <param name="end">The end time</param>
        /// <param name="location">The location</param>
        /// <param name="instructor">The uid of the professor</param>
        /// <returns>A JSON object containing {success = true/false}. 
        /// false if another class occupies the same location during any time 
        /// within the start-end range in the same semester, or if there is already
        /// a Class offering of the same Course in the same Semester,
        /// true otherwise.</returns>
        public IActionResult CreateClass(string subject, int number, string season, int year, DateTime start, DateTime end, string location, string instructor)
        {
            var query = from l in db.Classes
                        where (l.Location == location && ((start >= l.StartTime && start <= l.EndTime) || (end >= l.StartTime && end <= l.EndTime)))
                        select l;
            if (query.Any())
            {
                return Json(new { success = false });
            }

            var query2 = from o in db.Courses
                         join l in db.Classes
                         on o.CatalogId equals l.CourseCatalogId

                         where (
                            o.DeptAbbr == subject &&
                            o.Number == number &&
                            l.Semester == season &&
                            l.Year == year
                            )
                         select l;

            if (query2.Any())
            {
                return Json(new { success = false });
            }

            var query3 = from o in db.Courses

                         where (
                            o.DeptAbbr == subject &&
                            o.Number == number
                         )
                         select o.CatalogId;

            Classes c = new Classes();
            c.Location = location;
            c.Semester = season;
            c.TaughtBy = instructor;
            c.StartTime = start;
            c.EndTime = end;
            c.CourseCatalogId = query3.First();
            db.Classes.Add(c);
            db.SaveChanges();
                     
      return Json(new { success = true });
    }


    /*******End code to modify********/

  }
}