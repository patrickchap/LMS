using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LMS.Models.LMSModels;

namespace LMS.Controllers
{
  [Authorize(Roles = "Professor")]
  public class ProfessorController : CommonController
  {
    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Students(string subject, string num, string season, string year)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      return View();
    }

    public IActionResult Class(string subject, string num, string season, string year)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      return View();
    }

    public IActionResult Categories(string subject, string num, string season, string year)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      return View();
    }

    public IActionResult CatAssignments(string subject, string num, string season, string year, string cat)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      ViewData["cat"] = cat;
      return View();
    }

    public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      ViewData["cat"] = cat;
      ViewData["aname"] = aname;
      return View();
    }

    public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      ViewData["cat"] = cat;
      ViewData["aname"] = aname;
      return View();
    }

    public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      ViewData["cat"] = cat;
      ViewData["aname"] = aname;
      ViewData["uid"] = uid;
      return View();
    }

    /*******Begin code to modify********/


    /// <summary>
    /// Returns a JSON array of all the students in a class.
    /// Each object in the array should have the following fields:
    /// "fname" - first name
    /// "lname" - last name
    /// "uid" - user ID
    /// "dob" - date of birth
    /// "grade" - the student's grade in this class
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetStudentsInClass(string subject, int num, string season, int year)
    {
            var query = from o in db.Courses
                        join l in db.Classes
                        on o.CatalogId equals l.CourseCatalogId

                        join e in db.Enrolled
                        on l.ClassId equals e.ClassId

                        join s in db.Students
                        on e.StudentId equals s.UId

                        where (
                          o.DeptAbbr == subject &&
                          o.Number == num &&
                          l.Semester == season &&
                          l.Year == year
                        )
                        select new
                        {
                            fname = s.FirstName,
                            lname = s.LastName,
                            uid = s.UId,
                            dob = s.Dob,
                            grade = e.Grade
                        };
      return Json(query.ToArray());
    }



    /// <summary>
    /// Returns a JSON array with all the assignments in an assignment category for a class.
    /// If the "category" parameter is null, return all assignments in the class.
    /// Each object in the array should have the following fields:
    /// "aname" - The assignment name
    /// "cname" - The assignment category name.
    /// "due" - The due DateTime
    /// "submissions" - The number of submissions to the assignment
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class, 
    /// or null to return assignments from all categories</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category)
        {
            if (category == null)
            {
                var query = from o in db.Courses
                            join l in db.Classes
                            on o.CatalogId equals l.CourseCatalogId
                            join a in db.AssignmentCategories
                            on l.ClassId equals a.ClassId
                            join a2 in db.Assignments
                            on a.CategoryId equals a2.CategoryId
                            join s in db.Submission
                            on a2.AssignmentId equals s.AssignmentId
                            where o.Name == subject && o.Number == num && l.Semester == season && l.Year == year
                            select new
                            {
                                aname = a2.Name,
                                cname = a.Name,
                                due = a2.DueDate,
                                submissions = s.StudentId.Count()
                            };
                return Json(query.ToArray());
            }
            else
            {
                var query = from o in db.Courses
                            join l in db.Classes
                            on o.CatalogId equals l.CourseCatalogId
                            join a in db.AssignmentCategories
                            on l.ClassId equals a.ClassId
                            join a2 in db.Assignments
                            on a.CategoryId equals a2.CategoryId
                            join s in db.Submission
                            on a2.AssignmentId equals s.AssignmentId
                            where o.Name == subject && o.Number == num && l.Semester == season && l.Year == year && a.Name == category
                            select new
                            {
                                aname = a2.Name,
                                cname = a.Name,
                                due = a2.DueDate,
                                submissions = s.StudentId.Count()
                            };
                return Json(query.ToArray());
            }
        }


        /// <summary>
        /// Returns a JSON array of the assignment categories for a certain class.
        /// Each object in the array should have the folling fields:
        /// "name" - The category name
        /// "weight" - The category weight
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentCategories(string subject, int num, string season, int year)
    {
            var query = from o in db.Courses
                        join l in db.Classes
                        on o.CatalogId equals l.CourseCatalogId

                        join ac in db.AssignmentCategories
                        on l.ClassId equals ac.ClassId
                        where (
                          o.DeptAbbr == subject &&
                          o.Number == num &&
                          l.Semester == season &&
                          l.Year == year
                        )
                        select new
                        {
                            name = ac.Name,
                            weight = ac.GradingWeight
                        };

                  return Json(query.ToArray());
    }

    /// <summary>
    /// Creates a new assignment category for the specified class.
    /// If a category of the given class with the given name already exists, return success = false.
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The new category name</param>
    /// <param name="catweight">The new category weight</param>
    /// <returns>A JSON object containing {success = true/false} </returns>
    public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight)
    {
            var query = from o in db.Courses
                        join l in db.Classes
                        on o.CatalogId equals l.CourseCatalogId
                        where (
                          o.DeptAbbr == subject &&
                          o.Number == num &&
                          l.Semester == season &&
                          l.Year == year
                        )
                        select new
                        {
                            classid = l.ClassId
                        };
            try
            {
                AssignmentCategories ac = new AssignmentCategories();
                ac.ClassId = query.First().classid;
                ac.Name = category;
                ac.GradingWeight = catweight;
                db.AssignmentCategories.Add(ac);
                db.SaveChanges();

            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.Write(e);
                return Json(new { success = false });
            }
                 
      return Json(new { success = true });
    }

    /// <summary>
    /// Creates a new assignment for the given class and category.
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The new assignment name</param>
    /// <param name="asgpoints">The max point value for the new assignment</param>
    /// <param name="asgdue">The due DateTime for the new assignment</param>
    /// <param name="asgcontents">The contents of the new assignment</param>
    /// <returns>A JSON object containing success = true/false</returns>
    public IActionResult CreateAssignment(string subject, int num, string season, int year, string category, string asgname, int asgpoints, DateTime asgdue, string asgcontents)
    {
            var query = from o in db.Courses
                        join l in db.Classes
                        on o.CatalogId equals l.CourseCatalogId

                        join ac in db.AssignmentCategories
                        on l.ClassId equals ac.ClassId

                        join a in db.Assignments
                        on ac.CategoryId equals a.CategoryId

                        where (
                          o.DeptAbbr == subject &&
                          o.Number == num &&
                          l.Semester == season &&
                          l.Year == year &&
                          ac.Name == category
                        )

                        select new
                        {
                            id = ac.CategoryId
                        };

            try
            {
                Assignments a = new Assignments();
                a.CategoryId = query.First().id;
                a.Name = asgname;
                a.MaxPoint = asgpoints;
                a.DueDate = asgdue;
                a.Contents = asgcontents;
                db.Assignments.Add(a);
                db.SaveChanges();

                var classID = from o in db.Courses
                              join l in db.Classes
                              on o.CatalogId equals l.CourseCatalogId
                              where (
                                  o.DeptAbbr == subject &&
                                  o.Number == num &&
                                  l.Year == year &&
                                  l.Semester == season
                              )
                              select l.ClassId;

                var query2 = from e in db.Enrolled
                             where e.ClassId == classID.First()
                             select e.StudentId;

                foreach(var q in query2)
                {
                    string grade = autoGradeSubmission(q, subject, num, season, year);
                    var query3 = (from en in db.Enrolled
                                  where (en.StudentId == q && en.ClassId == classID.First())
                                  select en).FirstOrDefault();
                    query3.Grade = grade;
                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e);
                return Json(new { success = false });
            }



            return Json(new { success = true });
        }


    /// <summary>
    /// Gets a JSON array of all the submissions to a certain assignment.
    /// Each object in the array should have the following fields:
    /// "fname" - first name
    /// "lname" - last name
    /// "uid" - user ID
    /// "time" - DateTime of the submission
    /// "score" - The score given to the submission
    /// 
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The name of the assignment</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname)
    {
            var query = from o in db.Courses
                        join l in db.Classes
                        on o.CatalogId equals l.CourseCatalogId

                        join ac in db.AssignmentCategories
                        on l.ClassId equals ac.ClassId

                        join a in db.Assignments
                        on ac.CategoryId equals a.CategoryId

                        join s in db.Submission
                        on a.AssignmentId equals s.AssignmentId

                        join st in db.Students
                        on s.StudentId equals st.UId

                        where (
                            o.DeptAbbr == subject &&
                            o.Number == num &&
                            l.Semester == season &&
                            l.Year == year &&
                            ac.Name == category &&
                            a.Name == asgname
                        )
                        select new
                        {
                            fname = st.FirstName,
                            lname = st.LastName,
                            uid = st.UId,
                            time = s.Time,
                            score = s.Score
                        };
      return Json(query.ToArray());
    }


    /// <summary>
    /// Set the score of an assignment submission
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The name of the assignment</param>
    /// <param name="uid">The uid of the student who's submission is being graded</param>
    /// <param name="score">The new score for the submission</param>
    /// <returns>A JSON object containing success = true/false</returns>
    public IActionResult GradeSubmission(string subject, int num, string season, int year, string category, string asgname, string uid, int score)
    {
            var query = (from o in db.Courses
                         join l in db.Classes
                         on o.CatalogId equals l.CourseCatalogId

                         join ac in db.AssignmentCategories
                         on l.ClassId equals ac.ClassId

                         join a in db.Assignments
                         on ac.CategoryId equals a.CategoryId

                         join s in db.Submission
                         on a.AssignmentId equals s.AssignmentId

                         where (
                           
                           o.DeptAbbr == subject &&
                           o.Number == num &&
                           l.Semester == season &&
                           l.Year == year &&
                           ac.Name == category &&
                           a.Name == asgname &&
                           s.StudentId == uid
                         )

                         select s).FirstOrDefault();
            query.Score = score;
            db.SaveChanges();
            string grade = autoGradeSubmission(uid, subject, num, season, year);


            var classID = from o in db.Courses
                          join l in db.Classes
                          on o.CatalogId equals l.CourseCatalogId
                          where (
                              o.DeptAbbr == subject &&
                              o.Number == num &&
                              l.Year == year &&
                              l.Semester == season
                          )
                          select l.ClassId;

            var query2 = (from en in db.Enrolled
                          where (en.StudentId == uid && en.ClassId == classID.First())
                          select en).FirstOrDefault();
            query2.Grade = grade;
            db.SaveChanges();





      return Json(new { success = true });
    }

    public string autoGradeSubmission(string uid, string subject, int num, string season, int year)
        {

            var query1 = from c in db.Courses
                        join cl in db.Classes
                        on c.CatalogId equals cl.CourseCatalogId
                        join ac in db.AssignmentCategories
                        on cl.ClassId equals ac.ClassId
                        join a in db.Assignments
                        on ac.CategoryId equals a.CategoryId
                        join s in db.Submission
                        on a.AssignmentId equals s.AssignmentId
                        where (
                        //s.StudentId == uid &&
                        c.DeptAbbr == subject &&
                        c.Number == num &&
                        cl.Semester == season &&
                        cl.Year == year
                        )
                        select new
                        {
                            assingment = a,
                            classid = cl.ClassId
                        }
                        ;
            var cid = query1.First().classid;

            var query2 = from q in query1
                         join s in db.Submission
                         on new { A = q.assingment.AssignmentId, B = uid } equals new { A = s.AssignmentId, B = s.StudentId }
                         into joined
                         from j in joined.DefaultIfEmpty()
                         select new
                         {
                             score = j == null ? 0 : j.Score,
                             maxPoints = q.assingment.MaxPoint,
                             catid = q.assingment.CategoryId
                         };
            var query3 = from a in db.AssignmentCategories
                         where a.ClassId == cid
                         select new
                         {
                             cid = a.CategoryId,
                             weight = a.GradingWeight
                         };
                         

            double totalWtdAvg = 0;
            double totalWeight = 0;
       

            foreach(var x in query3)
            {
                int totalScore = 0;
                int totalMaxPoints = 0;
                foreach(var x2 in query2)
                {
                    if(x2.catid == x.cid)
                    {
                        totalScore += x2.score;
                        totalMaxPoints += x2.maxPoints;
               
                    }
                }

                if(totalMaxPoints == 0)
                {
                    continue;
                }

                totalWtdAvg += (totalScore / totalMaxPoints) * x.weight;
                totalWeight += x.weight;

            }
            double scale = 100 / totalWeight;
            double overallPercent = totalWtdAvg * scale;

            //step 7
            if (overallPercent >= 0.93)
            {
                return "A";
            }
            else if (overallPercent >= 0.9)
            {
                return "A-";
            }
            else if (overallPercent >= 0.87)
            {
                return "B+";
            }
            else if (overallPercent >= 0.83)
            {
                return "B";
            }
            else if (overallPercent >= 0.8)
            {
                return "B-";
            }
            else if (overallPercent >= 0.77)
            {
                return "C+";
            }
            else if (overallPercent >= 0.73)
            {
                return "C";
            }
            else if (overallPercent >= 0.7)
            {
                return "C-";
            }
            else if (overallPercent >= 0.67)
            {
                return "D+";
            }
            else if (overallPercent >= 0.63)
            {
                return "D";
            }
            else if (overallPercent >= 0.6)
            {
                return "D-";
            }
            else
            {
                return "E";
            }



        }


    /// <summary>
    /// Returns a JSON array of the classes taught by the specified professor
    /// Each object in the array should have the following fields:
    /// "subject" - The subject abbreviation of the class (such as "CS")
    /// "number" - The course number (such as 5530)
    /// "name" - The course name
    /// "season" - The season part of the semester in which the class is taught
    /// "year" - The year part of the semester in which the class is taught
    /// </summary>
    /// <param name="uid">The professor's uid</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetMyClasses(string uid)
    {
            var query = from l in db.Classes
                        join o in db.Courses
                        on l.CourseCatalogId equals o.CatalogId
                        where (l.TaughtBy == uid)
                        select new
                        {
                            subject = o.DeptAbbr,
                            number = o.Number,
                            name = o.Name,
                            season = l.Semester,
                            year = l.Year
                        };
      return Json(query.ToArray());
    }


    /*******End code to modify********/

  }
}