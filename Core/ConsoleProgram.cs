using System;
using System.Data;
using System.Reflection;
using System.Xml.Linq;
using Library.Danvas3.models;

namespace Danvas3
{
    class ConsoleProgram
    {
        static List<Course> courses = new List<Course>();
        static List<Person> people = new List<Person>();
        private static HashSet<string> courseCodes = new HashSet<string>();

        private static void ConsoleMain(string[] args)
        {

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("Danvas 3 - Learning Management System");
                Console.WriteLine("1. Create a course");
                Console.WriteLine("2. Create a student");
                Console.WriteLine("3. Add a student to a course");
                Console.WriteLine("4. Remove a student from a course");
                Console.WriteLine("5. List all courses");
                Console.WriteLine("6. Search for courses");
                Console.WriteLine("7. List all people");
                Console.WriteLine("8. Search for a person");
                Console.WriteLine("9. List all courses for a student");
                Console.WriteLine("10. Update a course's information");
                Console.WriteLine("11. Update a student's information");
                Console.WriteLine("12. Create an assignment");
                Console.WriteLine("13. View course information");
                Console.WriteLine("14. Grade a student's assignment");
                Console.WriteLine("15. Course Content Manager");
                Console.WriteLine("16. Calculate Student GPA");
                Console.WriteLine("17. Create Instructor or TA");
                Console.WriteLine("18. Assign Instructor or TA to Course");
                Console.WriteLine("0. Exit");

                int choice = GetChoice(18);

                switch (choice)
                {
                    case 1:
                        CreateCourse();
                        break;
                    case 2:
                        CreateStudent();
                        break;
                    case 3:
                        AddStudentToCourse();
                        break;
                    case 4:
                        RemoveStudentFromCourse();
                        break;
                    case 5:
                        ListAllCourses();
                        break;
                    case 6:
                        SearchCourses();
                        break;
                    case 7:
                        ListAllStudents();
                        break;
                    case 8:
                        SearchStudents();
                        break;
                    case 9:
                        ListCoursesForStudent();
                        break;
                    case 10:
                        UpdateCourse();
                        break;
                    case 11:
                        UpdateStudent();
                        break;
                    case 12:
                        CreateAssignment();
                        break;
                    case 13:
                        ShowCourseInfo();
                        break;
                    case 14:
                        AssignGradeToStudent();
                        break;
                    case 15:
                        GoToCourseManager();
                        break;
                    case 16:
                        CalculateGPA();
                        break;
                    case 17:
                        CreateFaculty();
                        break;
                    case 18:
                        AssignFaculty();
                        break;
                    case 0:
                        choice = 0;
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        static int GetChoice(int maxChoice)
        {
            Console.Write("Enter your choice: ");
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > maxChoice)
            {
                Console.Write("Invalid choice. Enter a number from 0 to {0}: ", maxChoice);
            }
            return choice;
        }

        static void CreateCourse()
        {
            bool uniqueCode = false;
            string code = "XXXX";
            while (!uniqueCode)
            {
                Console.Write("Enter the course code: ");
                code = Console.ReadLine().ToUpper().Replace(" ", "") ?? string.Empty;
                if (!IsCodeUnique(code))
                {
                    Console.Write("ERROR: Course code already in use. Try again. ");
                    Console.ReadKey();
                    return;
                }
                else uniqueCode = true;
            }
            courseCodes.Add(code);
            Console.Write("Enter the course name: ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter course credit hours: ");;
            int credits = Convert.ToInt32(Console.ReadLine() ?? string.Empty);
            Console.Write("Enter the course description: ");
            string description = Console.ReadLine() ?? string.Empty;
            Course course = new Course
            {
                Code = code,
                Name = name,
                Description = description,
                CreditHours = credits
            };
            courses.Add(course);
            Console.WriteLine("Course created successfully!");

            string defname = "Unweighted";
            int defweight = 1;
            AssignmentGroup unweightedGroup = new AssignmentGroup(defname, defweight); //create default unweighted assignment group
            course.AddAssignmentGroup(unweightedGroup);

            Console.ReadKey();
        }

        static void CreateStudent()
        {
            Console.Write("Enter the student's name: ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Enter Student Classification: (F)reshman, S(O)phomore, (J)unior, (S)enior");
            string classificationString = Console.ReadLine() ?? string.Empty;
            Person.ConvertStringToClassification(classificationString);

            try
            {
                Classification classification = Person.ConvertStringToClassification(classificationString);
                Person student = new Person(name, classification);
                people.Add(student);
                Console.WriteLine("Student created successfully!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid classification string: " + ex.Message);
            }

            Console.ReadKey();
        }

        static void AddStudentToCourse()
        {
            if (courses.Count == 0)
            {
                Console.WriteLine("There are no courses to add a student to.");
                Console.ReadKey();
                return;
            }
            if (people.Count == 0)
            {
                Console.WriteLine("There are no students to add to a course.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Enter the course code:");
            string code = Console.ReadLine() ?? string.Empty;

            Course course = courses.FirstOrDefault(c => c?.Code == code);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                Console.ReadKey();
                return;
            }

            if (course.Roster == null)
            {
                course.Roster = new List<Person>(); // create the roster list if it doesn't exist
            }

            Console.WriteLine("Enter ID of student to be added to course:");
            int id = int.Parse(Console.ReadLine());
            Person student = people?.FirstOrDefault(s => s?.ID == id);
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }
            if (course.Roster.Contains(student))
            {
                Console.WriteLine("Student is already enrolled in the course.");
                return;
            }

            course.Roster.Add(student);
            Console.WriteLine($"{student.Name} added to {course.Name} roster.");


            Console.ReadKey();
        }

        static void RemoveStudentFromCourse()
        {
            if (courses.Count == 0)
            {
                Console.WriteLine("There are no courses to remove a student from.");
                Console.ReadKey();
                return;
            }
            if (people.Count == 0)
            {
                Console.WriteLine("There are no students to remove from a course.");
                Console.ReadKey();
                return;
            }
            Console.Write("Enter the course code: ");
            string code = Console.ReadLine() ?? string.Empty;
            Course course = courses.Find(c => c?.Code == code);
            if (course == null)
            {
                Console.WriteLine("Course not found.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Enter ID of student to be removed from course:");
            int id = int.Parse(Console.ReadLine());
            Person student = people.FirstOrDefault(s => s?.ID == id);
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }
            if (!course.Roster.Contains(student))
            {
                Console.WriteLine("Student is not enrolled in this course.");
                return;
            }
            course.Roster.Remove(student);
            Console.WriteLine("{0} has been removed from {1}.", student.Name, course.Name);
            Console.ReadKey();
        }

        static void ListAllCourses()
        {
            var navigator = new ListNavigator<Course>(courses);


            if (courses.Count == 0)
            {
                Console.WriteLine("There are no courses to list.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Courses:");

            while (true)
            {
                var page = navigator.GetCurrentPage();
                foreach (var course in page)
                {
                    Console.WriteLine(course);
                }

                if (!navigator.HasNextPage)
                {
                    Console.WriteLine("Last page!");
                    break;
                }

                Console.WriteLine("Press any key to continue to the next page...");
                Console.ReadKey();
            }

            Console.ReadKey();
        }

        static void SearchCourses()
        {
            if (courses.Count == 0)
            {
                Console.WriteLine("There are no courses to search.");
                Console.ReadKey();
                return;
            }
            Console.Write("Enter the search query: ");
            string query = Console.ReadLine().ToLower() ?? string.Empty;
            List<Course> results = courses.FindAll(c => c.Name.ToLower().Contains(query) || c.Description.ToLower().Contains(query));
            if (results.Count == 0)
            {
                Console.WriteLine("No results found.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Search results:");
            foreach (Course course in results)
            {
                Console.WriteLine("{0} - {1}", course.Code, course.Name);
            }
            Console.ReadKey();
        }

        static void ListAllStudents()
        {
            var navigator = new ListNavigator<Person>(people);


            if (people.Count == 0)
            {
                Console.WriteLine("There are no students to list.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("People:");

            while (true)
            {
                var page = navigator.GetCurrentPage();
                foreach (var person in page)
                {
                    Console.WriteLine(person);
                }

                if (!navigator.HasNextPage)
                {
                    Console.WriteLine("Last page!");
                    break;
                }

                Console.WriteLine("Press any key to continue to the next page...");
                Console.ReadKey();
            }

            Console.ReadKey();
        }

        static void SearchStudents()
        {
            if (people.Count == 0)
            {
                Console.WriteLine("There are no students to search.");
                Console.ReadKey();
                return;
            }
            Console.Write("Enter the search query: ");
            string query = Console.ReadLine().ToLower() ?? string.Empty;
            List<Person> results = people.FindAll(s => s.Name.ToLower().Contains(query));
            if (results.Count == 0)
            {
                Console.WriteLine("No results found.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Search results:");
            foreach (Person student in results)
            {
                Console.WriteLine($"[{student.ID}] - {student.Name}");
            }
            Console.ReadKey();
        }

        static void ListCoursesForStudent()
        {
            if (people.Count == 0)
            {
                Console.WriteLine("There are no students to list courses for.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Enter a student ID to list courses for:");
            int id = int.Parse(Console.ReadLine() ?? string.Empty);
            Person student = people.FirstOrDefault(s => s?.ID == id);

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Console.WriteLine($"Courses for student {student.ID} ({student.Name}):");

            foreach (Course course in courses)
            {
                if (course.Roster.Contains(student))
                {
                    Console.WriteLine($"{course.Code} - {course.Name}");
                }
            }

            Console.ReadKey();
        }

        static void UpdateCourse()
        {
            if (courses.Count == 0)
            {
                Console.WriteLine("There are no courses to update.");
                Console.ReadKey();
                return;
            }
            Console.Write("Enter the course code: ");
            string code = Console.ReadLine() ?? string.Empty;
            Course course = courses.Find(c => c?.Code == code);
            if (course == null)
            {
                Console.WriteLine("Course not found.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Enter the new information for the course:");
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.Write("Description: ");
            string description = Console.ReadLine() ?? string.Empty;
            Console.Write("Credit Hours: ");
            int credits = Convert.ToInt32(Console.ReadLine() ?? string.Empty);
            course.Name = name;
            course.Description = description;
            course.CreditHours = credits;
            Console.WriteLine("{0} has been updated.", course.Name);
            Console.ReadKey();
        }

        static void UpdateStudent()
        {
            if (people.Count == 0)
            {
                Console.WriteLine("There are no students to update.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Enter ID for student to be updated:");
            int id = int.Parse(Console.ReadLine());

            Person student = people?.FirstOrDefault(s => s?.ID == id);

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Console.WriteLine($"Enter new name for {student.Name} (leave blank to keep current name):");
            string nameInput = Console.ReadLine() ?? string.Empty;
            string name = string.IsNullOrEmpty(nameInput) ? student.Name : nameInput;

            Console.WriteLine($"Enter new classification for {student.Name} (F)reshman, S(O)phomore, (J)unior, (S)enior (leave blank to keep current classification):");
            string classificationStringInput = Console.ReadLine() ?? string.Empty;
            Classification classification = string.IsNullOrEmpty(classificationStringInput) ? student.Classification : Person.ConvertStringToClassification(classificationStringInput);

            student.Name = name;
            student.Classification = classification;
            Console.WriteLine("{0} has been updated.", student.Name);
            Console.ReadKey();
        }

        static void CreateAssignment()
        {
            if (courses.Count == 0)
            {
                Console.WriteLine("There are no courses to create an assignment for.");
                Console.ReadKey();
                return;
            }
            Console.Write("Enter the course code: ");
            string code = Console.ReadLine().ToUpper().Replace(" ", "") ?? string.Empty;
            Course course = courses.Find(c => c?.Code == code);
            if (course == null)
            {
                Console.WriteLine("Course not found.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Enter the new assignment's information:");
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.Write("Description: ");
            string description = Console.ReadLine() ?? string.Empty;
            Console.Write("Total available points: ");
            int totalPoints = int.Parse(Console.ReadLine());
            Console.Write("Due date (mm/dd/yyyy): ");
            DateTime dueDate = DateTime.Parse(Console.ReadLine());
            Assignment assignment = new Assignment(name, description, totalPoints, dueDate);
            course.Assignments.Add(assignment);

            // automatically assign to default unweighted group
            string defaultGroupName = "Unweighted";
            AssignmentGroup defaultGroup = course?.AssignmentGroups?.Find(g => g?.Name == defaultGroupName);
            defaultGroup?.AddAssignment(assignment);

            Console.WriteLine("{0} has been added to {1}.", assignment.Name, course.Name);
            Console.ReadKey();
        }

        static void ShowCourseInfo()
        {
            if (courses.Count == 0)
            {
                Console.WriteLine("There are no courses to display.");
                Console.ReadKey();
                return;
            }
            Console.Write("Enter the course code: ");
            string code = Console.ReadLine() ?? string.Empty;
            Course course = courses?.Find(c => c?.Code == code);
            if (course == null)
            {
                Console.WriteLine("Course not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("{0} ({1})", course.Name, course.Code);
            Console.WriteLine("Credit Hours: {0}", course.CreditHours);
            Console.WriteLine("Description: {0}", course.Description);
            Console.WriteLine("------------");
            Console.WriteLine("Faculty:");

            foreach (var person in course.Faculty)
            {
                Console.WriteLine(person);
            }

            Console.WriteLine("Assignments:");
            foreach (Assignment assignment in course.Assignments)
            {
                Console.WriteLine("- {0} (#{1}) ({2} points, due {3})", assignment.Name, assignment.AssignmentId, assignment.TotalAvailablePoints, assignment.DueDate);
            }


            Console.ReadKey();
        }

        static void AssignGradeToStudent()
        {
            Console.Write("Enter course code: ");
            string courseCode = Console.ReadLine() ?? string.Empty;
            Course course = courses.Find(c => c?.Code == courseCode);
            if (course == null)
            {
                Console.WriteLine("Course not found.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter assignment ID: ");
            int assignmentId = int.Parse(Console.ReadLine());

            Assignment assignment = course?.Assignments.Find(a => a?.AssignmentId == assignmentId);

            if (assignment == null)
            {
                Console.WriteLine($"Assignment with ID {assignmentId} not found in course {courseCode}");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter student ID: ");
            int id = int.Parse(Console.ReadLine());
            Person student = people.FirstOrDefault(s => s?.ID == id);
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                Console.ReadKey();
                return;
            }
            if (!course.Roster.Contains(student))
            {
                Console.WriteLine("Student is not enrolled in this course.");
                return;
            }

            if (student is Instructor || student is TA)
            {
                Console.WriteLine("Only students can be assigned grades.");
                return;
            }

            Console.Write($"Enter grade for {student.Name}: ");
            int grade = int.Parse(Console.ReadLine());

            if (assignment.Grades.ContainsKey(student))
            {
                assignment.Grades[student] = grade;
            }
            else
            {
                assignment.Grades.Add(student, grade);
            }

            Console.WriteLine($"Grade of {grade} assigned to {student.Name} for assignment {assignment.Name} ({assignment.AssignmentId}) in course {courseCode}");
            Console.ReadKey();
        }

        static bool IsCodeUnique(string code) // prevent duplicate codes
        {
            return !courseCodes.Contains(code); // if code is in courseCodes has, return false
        }

        static void GoToCourseManager()
        {
            Console.Write("Enter course code to manage: ");
            string courseCode = Console.ReadLine() ?? string.Empty;
            Course course = courses.Find(c => c?.Code == courseCode);
            if (course == null)
            {
                Console.WriteLine("Course not found.");
                Console.ReadKey();
                return;
            }
            CourseManager.Start(course);
        }

        static void CalculateGPA()
        {
            double totalGradePoints = 0;
            int totalCreditHours = 0;
            double gpa = 0;

            Console.Write("Enter student ID: ");
            int id = int.Parse(Console.ReadLine());
            Person student = people.FirstOrDefault(s => s?.ID == id);
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                Console.ReadKey();
                return;
            }

            if (student is Instructor || student is TA)
            {
                Console.WriteLine("Only students can have GPAs.");
                Console.ReadKey();
                return;
            }

            foreach (Course course in courses)
            {
                if (course.Roster.Contains(student))
                {
                    totalCreditHours += course.CreditHours;
                    totalGradePoints += course.GetWeightedAverage(student);
                    double localAverage = course.GetWeightedAverage(student);
                    Console.WriteLine($"Grade for {course.Name}: {localAverage}");
                }
            }

            gpa = totalGradePoints / totalCreditHours;
            Console.WriteLine($"GPA for Student [{student.ID}] {student.Name}: {gpa}");
            Console.ReadKey();

        }

        static void CreateFaculty()
        {
            Console.Write("Enter the employee's name: ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Enter Classification: (T) TA / (I) Instructor");
            string classificationString = Console.ReadLine() ?? string.Empty;
            Person.ConvertStringToClassification(classificationString);

            try
            {
                Classification classification = Person.ConvertStringToClassification(classificationString);
                Person faculty = new Person(name, classification);
                people.Add(faculty);
                Console.WriteLine("Employee created successfully!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid classification string: " + ex.Message);
            }

            Console.ReadKey();

        }

        static void AssignFaculty()
        {

            if (courses.Count == 0)
            {
                Console.WriteLine("There are no courses to assign an employee to.");
                Console.ReadKey();
                return;
            }

            /*
            if (Person.nextEmplID == 1)
            {
                Console.WriteLine("There is no faculty to assign to a course.");
                Console.ReadKey();
                return;
            } */

            Console.WriteLine("Enter the course code:");
            string code = Console.ReadLine() ?? string.Empty;

            Course course = courses.FirstOrDefault(c => c?.Code == code);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                Console.ReadKey();
                return;
            }

            if (course.Faculty == null)
            {
                course.Faculty = new List<Person>(); // create the roster list if it doesn't exist
            }

            Console.WriteLine("Enter EMPLID of faculty to be added to course:");
            int id = int.Parse(Console.ReadLine() ?? string.Empty);
            Person employee = people?.FirstOrDefault(s => s?.emplID == id);
            if (employee == null)
            {
                Console.WriteLine("Employee not found.");
                Console.ReadKey();
                return;
            }
            if (course.Faculty.Contains(employee))
            {
                Console.WriteLine("Employee is already assigned to this course.");
                Console.ReadKey();
                return;
            }

            course.Faculty.Add(employee);
            Console.WriteLine($"{employee.Name} added to {course.Name} roster.");


            Console.ReadKey();

        }
    }
}