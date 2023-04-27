using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Library.Danvas3.models
{
    public class CourseManager
    {
        private static Course _course;
        public static void SetCourse(Course course)
        {
            _course = course;
        }
        public static void Start(Course course)
        {
            SetCourse(course);
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("Danvas 3 - Learning Management System");
                Console.WriteLine("============COURSE CONTENT MANAGER============");
                Console.WriteLine("Selected Course [{0}]: {1}", course.Code, course.Name);
                Console.WriteLine("1. Display Course Information");
                Console.WriteLine("2. Manage Assignments");
                Console.WriteLine("3. Manage Modules");
                Console.WriteLine("4. Manage Announcements");
                Console.WriteLine("5. Exit");

                int choice = GetChoice(5);

                switch (choice)
                {
                    case 1:
                        ShowCourseInfo();
                        break;
                    case 2:
                        ShowAssignmentMenu();
                        break;
                    case 3:
                        ManageModules();
                        break;
                    case 4:
                        ManageAnnouncements();
                        break;
                    case 5:
                        Console.WriteLine("Exiting Course Manager...");
                        Console.ReadKey();
                        return;
                    default:
                        Console.WriteLine("Invalid input, please try again.");
                        Console.ReadKey();
                        break;
                }
            }

            static int GetChoice(int maxChoice)
            {
                Console.Write("Enter your choice: ");
                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > maxChoice)
                {
                    Console.Write("Invalid choice. Enter a number from 0 to {0}: ", maxChoice);
                    Console.ReadKey();
                }
                return choice;
            }

            static void ShowCourseInfo()
            {
                Console.WriteLine(_course);

                Console.WriteLine("===========================");
                Console.WriteLine("Modules:");
                foreach (var module in _course?.Modules)
                {
                    Console.WriteLine(module);
                }

                Console.ReadKey();

                Console.WriteLine("===========================");
                Console.WriteLine("Assignments:");

                foreach (var group in _course.AssignmentGroups)
                {
                    if (group.Name != null)
                    {
                        Console.WriteLine($"---Group [ID: {group.GroupId}]: {group.Name} (Weight: {group.Weight})----");
                    }

                    foreach (var assignment in group.Assignments)
                    {
                        Console.WriteLine($"[{assignment.AssignmentId}] {assignment.Name}: {assignment.Grades.Count} submissions");
                    }
                }

                Console.ReadKey();
                Console.WriteLine("===========================");
                Console.WriteLine("Roster:");

                foreach (var person in _course.Roster)
                {
                    Console.WriteLine(person);
                }
                Console.ReadKey();
                Console.WriteLine("===========================");

            }

            static void ShowAssignmentMenu()
            {
                Console.Clear();
                Console.WriteLine("Danvas 3 - Learning Management System");
                Console.WriteLine("============COURSE ASSIGNMENT MANAGER============");
                Console.WriteLine("Selected Course [{0}]: {1}", _course.Code, _course.Name);
                Console.WriteLine("1. Display Course Information");
                Console.WriteLine("2. Create Assignment");
                Console.WriteLine("3. Remove Assignment");
                Console.WriteLine("4. Create Group");
                Console.WriteLine("5. Delete Group (all assignments inside it will automatically become unweighted)");
                Console.WriteLine("6. Add Assignment to Group");
                Console.WriteLine("7. Remove Assignment from Group");
                Console.WriteLine("0. Return");

                int choice2 = GetChoice(8);
                switch (choice2)
                {
                    case 1:
                        ShowCourseInfo();
                        break;
                    case 2:
                        CreateAssignment();
                        break;
                    case 3:
                        RemoveAssignment();
                        break;
                    case 4:
                        CreateGroup();
                        break;
                    case 5:
                        DeleteGroup();
                        break;
                    case 6:
                        AssignmentToGroup();
                        break;
                    case 7:
                        RemoveAssignmentFromGroup();
                        break;
                    case 0:
                        Console.WriteLine("Returning...");
                        Console.ReadKey();
                        return;
                    default:
                        Console.WriteLine("Invalid input, please try again.");
                        Console.ReadKey();
                        break;
                }

            }

            static void CreateAssignment()
            {

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
                _course.Assignments.Add(assignment);

                // automatically assign to default unweighted group
                string defaultGroupName = "Unweighted";
                AssignmentGroup defaultGroup = _course.AssignmentGroups.Find(g => g?.Name == defaultGroupName);
                defaultGroup?.AddAssignment(assignment);

                Console.WriteLine("{0} has been added to {1}.", assignment.Name, _course.Name);
                Console.ReadKey();

            }

            static void RemoveAssignment()
            {
                Console.WriteLine("Enter assignment ID:");
                int aID = int.Parse(Console.ReadLine());
                _course?.RemoveAssignment(aID);
            }

            static void CreateGroup()
            {
                Console.Write("Enter Group Name: ");
                string name = Console.ReadLine() ?? string.Empty;
                Console.Write("Enter Group Weight: ");
                double weight = double.Parse(Console.ReadLine() ?? string.Empty);

                AssignmentGroup newGroup = new AssignmentGroup(name, weight);
                _course.AddAssignmentGroup(newGroup);
                Console.Write("Group [{0}] {1} has been added to {2} ", newGroup.GroupId, newGroup.Name, _course.Name);
                Console.ReadKey();
            }

            static void DeleteGroup()
            {
                Console.Write("Enter Group Name: ");
                string groupName = Console.ReadLine() ?? string.Empty;

                bool isMatch = groupName.Equals("Unweighted"); //cannot remove unweighted group
                if (isMatch)
                {
                    Console.Write("~ERROR~ Cannot remove unweighted group ");
                    Console.ReadKey();
                    return;
                }

                var groupToDelete = _course?.AssignmentGroups?.FirstOrDefault(ag => ag?.Name == groupName);

                if (groupToDelete != null)
                {
                    _course?.AssignmentGroups.Remove(groupToDelete);
                    Console.WriteLine($"Group '{groupName}' Deleted.");
                }
                else
                {
                    Console.WriteLine($"No assignment group found with name '{groupName}'.");
                }
                Console.ReadKey();
            }

            static void AssignmentToGroup()
            {
                // Get the group name from the user
                Console.Write("Enter the name of the group to add the assignment to: ");
                string groupName = Console.ReadLine() ?? string.Empty;

                // Find the group in the course
                AssignmentGroup group = _course?.AssignmentGroups?.Find(g => g?.Name == groupName);
                if (group == null)
                {
                    Console.WriteLine($"Group with name '{groupName}' not found.");
                    Console.ReadKey();
                    return;
                }

                // Get the assignment ID from the user
                Console.Write("Enter the ID of the assignment to add to the group: ");
                int assignmentId;
                if (!int.TryParse(Console.ReadLine(), out assignmentId))
                {
                    Console.WriteLine("Invalid assignment ID.");
                    Console.ReadKey();
                    return;
                }

                // Find the assignment in the course
                Assignment assignment = _course?.Assignments?.Find(a => a?.AssignmentId == assignmentId);
                if (assignment == null)
                {
                    Console.WriteLine($"Assignment with ID '{assignmentId}' not found.");
                    Console.ReadKey();
                    return;
                }

                // Add the assignment to the group
                group.AddAssignment(assignment);
                Console.WriteLine($"Assignment '{assignment.Name}' added to group '{group.Name}'.");

                // remove from unweighted default group
                AssignmentGroup defgroup = _course?.AssignmentGroups?.Find(g => g?.Name == "Unweighted");
                defgroup.RemoveAssignment(assignment);

                Console.ReadKey();

            }

            static void RemoveAssignmentFromGroup()
            {
                // Get the group name from the user
                Console.Write("Enter the name of the group to remove the assignment from: ");
                string groupName = Console.ReadLine() ?? string.Empty;
                AssignmentGroup defgroup = _course?.AssignmentGroups?.Find(g => g?.Name == "Unweighted");

                // Find the group in the course
                AssignmentGroup group = _course?.AssignmentGroups?.Find(g => g?.Name == groupName);
                if (group == null)
                {
                    Console.WriteLine($"Group with name '{groupName}' not found.");
                    Console.ReadKey();
                    return;
                }

                // Display existing assignments

                foreach (var Assignment in group.Assignments)
                {
                    Console.WriteLine($"[{Assignment.AssignmentId}] {Assignment.Name}: {Assignment.Grades.Count}");
                }


                // Get the assignment ID from the user
                Console.Write("Enter ID of the assignment to add to the group: ");
                int assignmentId = int.Parse(Console.ReadLine()); ;
                Assignment assignment = _course?.Assignments?.Find(a => a?.AssignmentId == assignmentId);
                if (assignment == null)
                {
                    Console.WriteLine($"Assignment with ID {assignmentId} not found in course");
                    Console.ReadKey();
                    return;
                }

                // If the assignment was found, remove it from its group and add it to "Unweighted" group
                if (assignment != null)
                {
                    if (!group.Assignments.Contains(assignment))
                    {
                        Console.WriteLine("Assignment not contained in this group");
                        Console.ReadKey();
                        return;
                    }
                    group?.Assignments?.Remove(assignment);

                    // Add the assignment to the default Unweighted group
                    defgroup?.Assignments?.Add(assignment);

                    Console.WriteLine($"Assignment {assignmentId} removed from group {group?.Name} and added to unweighted group.");
                }
            }

            static void ManageModules()
            {
                Console.Clear();
                Console.WriteLine("Danvas 3 - Learning Management System");
                Console.WriteLine("============COURSE MODULE MANAGER============");
                Console.WriteLine("Selected Course [{0}]: {1}", _course.Code, _course.Name);
                Console.WriteLine("1. Create Module");
                Console.WriteLine("2. View Modules");
                Console.WriteLine("3. Update Module");
                Console.WriteLine("4. Delete Module");
                Console.WriteLine("0. Return");

                int choice2 = GetChoice(4);
                switch (choice2)
                {
                    case 1:
                        CreateModule();
                        break; 
                    case 2:
                        ReadModules();
                        break;
                    case 3:
                        UpdateModule();
                        break;
                    case 4:
                        DeleteModule();
                        break; 
                    case 0:
                        Console.WriteLine("Returning...");
                        Console.ReadKey();
                        return;
                    default:
                        Console.WriteLine("Invalid input, please try again.");
                        Console.ReadKey();
                        break;
                }

            }

            static void CreateModule()
            {
                Console.Write("Enter the module name: ");
                string name = Console.ReadLine() ?? string.Empty;
                Console.Write("Enter the module description: ");
                string description = Console.ReadLine() ?? string.Empty;
                Module module = new Module
                {
                    Name = name,
                    Description = description
                };

                _course?.AddModule(module);
                Console.WriteLine("Module [{0}] {1} added to {2}", module.ID, module.Name, _course?.Name);
                Console.ReadKey();
            }

            static void ReadModules()
            {
                foreach(var module in _course.Modules)
                {
                    Console.WriteLine(module);
                }
                Console.ReadKey();
            }

            static void UpdateModule()
            {

                if (_course.Modules.Count == 0)
                {
                    Console.WriteLine("There are no courses to update.");
                    Console.ReadKey();
                    return;
                }
                Console.Write("Enter the module ID: ");
                int moduleID = Convert.ToInt32(Console.ReadLine() ?? string.Empty);;
                Module module = _course?.Modules.Find(c => c?.ID == moduleID);
                if (module == null)
                {
                    Console.WriteLine("Module not found.");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine("Enter the new information for the module:");
                Console.Write("Name: ");
                string name = Console.ReadLine() ?? string.Empty;
                Console.Write("Description: ");
                string description = Console.ReadLine() ?? string.Empty;
                module.Name = name;
                module.Description = description;
                Console.WriteLine("{0} has been updated.", module.Name);
                Console.ReadKey();
            }

            static void DeleteModule()
            {
                if (_course.Modules.Count == 0)
                {
                    Console.WriteLine("There are no courses to update.");
                    Console.ReadKey();
                    return;
                }
                Console.Write("Enter the module ID: ");
                int moduleID = Convert.ToInt32(Console.ReadLine() ?? string.Empty); ;
                Module module = _course?.Modules.Find(c => c?.ID == moduleID);
                if (module == null)
                {
                    Console.WriteLine("Module not found.");
                    Console.ReadKey();
                    return;
                }

                _course?.RemoveModule(module);
                Console.WriteLine("Module has been removed.", module.Name);
                Console.ReadKey();
            }


            static void ManageAnnouncements()
            { 
                Console.Clear();
                Console.WriteLine("Danvas 3 - Learning Management System");
                Console.WriteLine("============COURSE ANNOUNCEMENT MANAGER============");
                Console.WriteLine("Selected Course [{0}]: {1}", _course.Code, _course.Name);
                Console.WriteLine("1. Create Announcement");
                Console.WriteLine("2. View Announcements");
                Console.WriteLine("3. Update Announcement");
                Console.WriteLine("4. Delete Announcement");
                Console.WriteLine("0. Return");

                int choice2 = GetChoice(4);
                switch (choice2)
                {
                    case 1:
                        CreateAnnouncement();
                        break;
                    case 2:
                        ReadAnnouncements();
                        break;
                    case 3:
                        UpdateAnnouncement();
                        break;
                    case 4:
                        DeleteAnnouncement();
                        break;
                    case 0:
                        Console.WriteLine("Returning...");
                        Console.ReadKey();
                        return;
                    default:
                        Console.WriteLine("Invalid input, please try again.");
                        Console.ReadKey();
                        break;
                }

            }


            static void CreateAnnouncement()
            {
                Console.Write("Enter title: ");
                string title = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter message: ");
                string content = Console.ReadLine() ?? string.Empty;

                Announcement announcement = new Announcement
                {
                    Title = title,
                    Content = content,
                    PublishDate = DateTime.Now
                };

                _course?.AddAnnouncement(announcement);

                Console.WriteLine("Announcement created successfully!");

                Console.ReadKey();

            }
            static void ReadAnnouncements()
            {
                var announcements = _course.Announcements;
                if (announcements.Count == 0)
                {
                    Console.WriteLine("No announcements found.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("\nAnnouncements:");
                foreach (var a in announcements)
                {
                    Console.WriteLine("------------------------------------------------------");
                    Console.WriteLine($"{a.Title} [{a.ID}] ({a.PublishDate})");
                    Console.WriteLine($"{a.Content}");
                    Console.WriteLine("~~Press any key to continue...~~");
                    Console.ReadKey();
                }
                Console.ReadKey();
            }
            static void UpdateAnnouncement()
            {
                Console.Write("Enter announcement ID: ");
                int announcementId = int.Parse(Console.ReadLine() ?? string.Empty);

                var announcementToUpdate = _course.GetAnnouncementById(announcementId);
                if (announcementToUpdate == null)
                {
                    Console.WriteLine("Announcement not found.");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Enter new title (or press enter to keep the same): ");
                string newTitle = Console.ReadLine() ?? string.Empty;
                if (!string.IsNullOrEmpty(newTitle))
                {
                    announcementToUpdate.Title = newTitle;
                }

                Console.Write("Enter new message (or press enter to keep the same): ");
                string newContent = Console.ReadLine() ?? string.Empty;
                if (!string.IsNullOrEmpty(newContent))
                {
                    announcementToUpdate.Content = newContent;
                }

                Console.WriteLine("Announcement updated successfully!");
                Console.ReadKey();
            }
            static void DeleteAnnouncement()
            {
                Console.Write("Enter announcement ID: ");
                int announcementIdToDelete = int.Parse(Console.ReadLine() ?? string.Empty);

                var announcementToDelete = _course.GetAnnouncementById(announcementIdToDelete);
                if (announcementToDelete == null)
                {
                    Console.WriteLine("Announcement not found.");
                    Console.ReadKey();
                    return;
                }

                _course.DeleteAnnouncement(announcementIdToDelete);

                Console.WriteLine("Announcement deleted successfully!");
                Console.ReadKey();
            }
        }
    }

}

