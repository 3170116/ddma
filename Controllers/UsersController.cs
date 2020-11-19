using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ddma.Models;
using ddma.Summaries;

namespace ddma.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public User GetUser(int id)
        {
            var user = _context.Users.Include(x => x.TaskAssignmentUsers).SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            return user;
        }

        /// <summary>
        /// Επιστρέφει κάποιες βασικές πληροφορίες του χρήστη.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/summary")]
        public UserSummary GetUserSummary(int id)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            return new UserSummary(user);
        }

        /// <summary>
        /// Φέρνει πίσω τα task που έχει αναλάβει ο συγκεκριμένος χρήστης.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{id}/taskAssignments")]
        public IEnumerable<TaskAssignment> GetTaskAssignmentUser(int id)
        {
            return _context.TaskAssignmentUsers.Include("TaskAssignment").Where(x => x.UserId == id).Select(x => x.TaskAssignment).ToList();
        }

        /// <summary>
        /// Θα καλείται όταν ο χρήστης κάνει edit το προφίλ του ή θέλει να αλλάξει τα credentials του.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public User PutUser(int id, User user)
        {

            if (!user.IsValid())
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            var editUser = _context.Users.SingleOrDefault(x => x.Id == id);

            if (editUser == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            editUser.PasswordHash = user.PasswordHash;
            editUser.NickName = user.NickName;
            editUser.TimeZone = user.TimeZone;

            _context.SaveChanges();

            return editUser;

        }

        /// <summary>
        /// Καλείται όταν κάνουμε edit ένα asset. Αν δεν θέλουμε να έχει τοποθεσία, τότε location = null.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="assetId"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        [HttpPut("{id}/Assets/{assetId}")]
        public Asset PutAsset(int id, int assetId, Asset asset)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            var editAsset = _context.Assets.Include(x => x.Location).SingleOrDefault(x => x.Id == assetId);

            if (editAsset == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            if (!asset.IsValid())
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            //ελέγχουμε αν το asset ανήκει σε αυτήν την εταιρία
            var company = _context.Companies.Include(x => x.Assets).SingleOrDefault(x => x.Id == editAsset.CompanyId);

            if (company == null || !company.Assets.Any(x => x.Id == assetId))
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }


            var location = asset.Location;

            editAsset.Name = asset.Name;
            editAsset.Description = asset.Description ?? "";

            editAsset.Location = location;
            var editLocation = editAsset.Location;

            //περίπτωση 1: είχε ήδη location και εξακολουθεί να έχει
            if (editLocation != null && location != null)
            {
                editLocation.X = location.X;
                editLocation.Y = location.Y;
                editLocation.Country = location.Country;
                editLocation.City = location.City;
                editLocation.Street = location.Street;
                editLocation.Number = location.Number;
                editLocation.ZipCode = location.ZipCode;
            }

            //περίπτωση 2: είχε ήδη location ενώ τώρα δεν έχει
            if (editLocation != null && location == null)
            {
                editAsset.LocationId = null;

                _context.Locations.Remove(editLocation);
                _context.SaveChanges();
            }

            //περίπτωση 3: δεν είχε location ενώ τώρα έχει
            if (editLocation == null && location != null)
            {
                editLocation = new Location();

                editLocation.X = location.X;
                editLocation.Y = location.Y;
                editLocation.Country = location.Country;
                editLocation.City = location.City;
                editLocation.Street = location.Street;
                editLocation.Number = location.Number;
                editLocation.ZipCode = location.ZipCode;

                _context.Locations.Add(editLocation);
                _context.SaveChanges();

                editAsset.LocationId = editLocation.Id;
            }

            //περίπτωση 4: δεν είχε location και ούτε τώρα έχει
            //δεν χρειάζεται να γίνει κάτι

            _context.SaveChanges();

            _context.AssetLogs.Add(new AssetLog()
            {
                AssetId = assetId,
                UserId = id,
                CreatedAt = DateTime.UtcNow,
                AssetLogType = Enums.AssetLogType.EDIT,
                Title = "Asset '" + asset.Name + "' edit from user '" + user.NickName + "'",
                Description = ""
            });
            _context.SaveChanges();

            return editAsset;
        }

        /// <summary>
        /// Θα καλείται όταν θέλει να κάνει ο χρήστης edit ένα task.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskAssignmentId"></param>
        /// <param name="taskAssignment"></param>
        /// <returns></returns>
        [HttpPut("{id}/TaskAssignments/{taskAssignmentId}")]
        public TaskAssignment PutTaskAssignment(int id, int taskAssignmentId, TaskAssignment taskAssignment)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            var editTask = _context.TaskAssignments.Include(x => x.TaskAssignmentGroup).Include(x => x.TaskAssignmentUsers).SingleOrDefault(x => x.Id == taskAssignmentId);

            if (editTask == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            if (!taskAssignment.IsValid())
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }


            var logType = Enums.TaskLogType.EDIT;
            var notificationType = Enums.NotificationType.EDIT_TASK;
            var logDescr = "";
            
            if (editTask.Priority != taskAssignment.Priority)
            {
                logType = Enums.TaskLogType.CHANGED_PRIORITY;
                notificationType = Enums.NotificationType.CHANGED_TASK_PRIORITY;
                logDescr = "Priority changed from " + Enum.GetName(typeof(Enums.TaskAssignmentPriority), editTask.Priority) + " to " + Enum.GetName(typeof(Enums.TaskAssignmentPriority), taskAssignment.Priority);
            } else if (editTask.Status != taskAssignment.Status)
            {
                logType = Enums.TaskLogType.CHANGED_STATUS;
                notificationType = Enums.NotificationType.CHANGED_TASK_STATUS;
                logDescr = "Status changed from " + Enum.GetName(typeof(Enums.TaskAssignmentStatus), editTask.Status) + " to " + Enum.GetName(typeof(Enums.TaskAssignmentStatus), taskAssignment.Status);
            }
            else if (editTask.Deadline != taskAssignment.Deadline)
            {
                logType = Enums.TaskLogType.CHANGED_DEADLINE;
                notificationType = Enums.NotificationType.CHANGED_TASK_DEADLINE;
                logDescr = "Deadline changed from " + editTask.Deadline == null ? "" : editTask.Deadline.ToString() + " to " + taskAssignment.Deadline == null ? "" : taskAssignment.Deadline.ToString();
            } else if (editTask.TaskAssignmentGroupId != taskAssignment.TaskAssignmentGroupId)
            {
                logType = Enums.TaskLogType.CHANGED_TASK_GROUP;
                notificationType = Enums.NotificationType.CHANGED_TASK_GROUP;
                logDescr = "Task moved from group '" + editTask.TaskAssignmentGroup.Name + "' to '" + taskAssignment.TaskAssignmentGroup.Name + "'";
            }

            editTask.Title = taskAssignment.Title;
            editTask.Description = taskAssignment.Description;
            editTask.Priority = taskAssignment.Priority;
            editTask.Status = taskAssignment.Status;
            editTask.Deadline = taskAssignment.Deadline;

            editTask.TaskAssignmentGroupId = taskAssignment.TaskAssignmentGroupId;


            foreach (var taskAssignmentUser in editTask.TaskAssignmentUsers)
            {
                _context.UserNotifications.Add(new UserNotification()
                {
                    UserId = taskAssignmentUser.UserId,
                    NotificationType = notificationType,
                    CreatedAt = DateTime.UtcNow,
                    Title = "Edit Task Assignment",
                    Description = "The task with name " + editTask.Title + " has been edit from user '" + user.NickName + "'."
                });
            }

            _context.TaskLogs.Add(new TaskLog()
            {
                TaskAssignmentId = editTask.Id,
                UserId = id,
                TaskLogType = logType,
                CreatedAt = DateTime.UtcNow,
                Title = "Task '" + editTask.Title + "' edit from '" + user.NickName + "'",
                Description = logDescr
            });
            _context.SaveChanges();

            return editTask;
        }

        /// <summary>
        /// Θα καλείται όταν κάνει ο χρήστης login.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public User GetUser(User user)
        {
            var loginUser = _context.Users.SingleOrDefault(x => x.Email == user.Email);

            if (loginUser == null || loginUser.PasswordHash != user.PasswordHash)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            return loginUser;
        }

        /// <summary>
        /// Θα καλείται όταν εγγράφεται νέος χρήστης.
        /// </summary>
        /// <returns></returns>
        [HttpPost("signUp")]
        public User PostUser(User user)
        {

            if (!user.IsValid())
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            var company = _context.Companies.Include(x => x.Users).Single(x => x.Id == user.CompanyId);

            if (company == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            if (_context.Users.Any(x => x.Email == user.Email))
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            _context.Users.Add(user);

            foreach (var companyUser in company.Users)
            {
                _context.UserNotifications.Add(new UserNotification()
                {
                    UserId = companyUser.Id,
                    NotificationType = Enums.NotificationType.ADDED_NEW_USER,
                    CreatedAt = DateTime.UtcNow,
                    Title = "New User",
                    Description = "User '" + companyUser.NickName + "' added to company by '" + user.NickName + "'."
                });
            }

            _context.SaveChanges();

            return user;

        }

        /// <summary>
        /// Καλείται όταν προσθέτουμε ένα asset. Αν δεν θέλουμε να έχει τοποθεσία, τότε location = null.
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        [HttpPost("{id}/Assets")]
        public Asset PostAsset(int id, Asset asset)
        {

            var user = _context.Users.SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            var location = asset.Location;

            if (location != null)
            {
                _context.Locations.Add(location);
                _context.SaveChanges();
            }


            asset.Description = asset.Description ?? "";
            asset.CreatedAt = DateTime.UtcNow;

            if (location == null)
            {
                asset.LocationId = null;
            }
            else
            {
                asset.LocationId = location.Id;
            }

            _context.Assets.Add(asset);
            _context.SaveChanges();

            _context.AssetLogs.Add(new AssetLog()
            {
                AssetId = asset.Id,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                AssetLogType = Enums.AssetLogType.ADDED,
                Title = "Asset '" + asset.Name + "' added from user '" + user.NickName + "'",
                Description = ""
            });
            _context.SaveChanges();

            return asset;
        }

        /// <summary>
        /// Θα καλείται όταν προσθέτει ο χρήστης ένα task.
        /// </summary>
        /// <param name="taskAssignment"></param>
        /// <returns></returns>
        [HttpPost("{id}/TaskAssignments")]
        public TaskAssignment PostTaskAssignment(int id, TaskAssignment taskAssignment)
        {

            var user = _context.Users.SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            if (!taskAssignment.IsValid())
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            taskAssignment.setStatus(0);

            taskAssignment.CreatedAt = DateTime.UtcNow;

            _context.TaskAssignments.Add(taskAssignment);
            _context.SaveChanges();

            _context.TaskLogs.Add(new TaskLog()
            {
                TaskAssignmentId = taskAssignment.Id,
                UserId = id,
                TaskLogType = Enums.TaskLogType.ADDED,
                CreatedAt = DateTime.UtcNow,
                Title = "Task '" + taskAssignment.Title + "' created from '" + user.NickName + "'",
                Description = ""
            });
            _context.SaveChanges();

            return taskAssignment;

        }

        /// <summary>
        /// Θα καλείται όταν κάνει ανάθεσει ο super-supervisor ή κάποιος supervisor ένα task σε έναν employee.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskAssignmentUser"></param>
        /// <returns></returns>
        [HttpPost("{id}/TaskAssignmentUsers")]
        public int PostTaskAssignmentUser(int id, TaskAssignmentUser taskAssignmentUser)
        {

            var user = _context.Users.SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            //ελέγχουμε αν έχει γίνει ήδη assign αυτό το task σε αυτόν τον χρήστη
            var newTaskAssignmentUser = _context.TaskAssignmentUsers.SingleOrDefault(x => x.TaskAssignmentId == taskAssignmentUser.TaskAssignmentId && x.UserId == taskAssignmentUser.UserId);

            if (newTaskAssignmentUser != null)
            {
                HttpContext.Response.StatusCode = 400;
                return newTaskAssignmentUser.Id;
            }

            //ελέγχουμε αν ανήκει ο συγκεκριμένος χρήστης στην εταιρεία.
            var company = _context.Companies.Include("Users").Include("TaskAssignmentGroups.TaskAssignments").SingleOrDefault(x => x.Id == user.CompanyId);
            var taskAssignment = company.GetTaskAssignments().SingleOrDefault(x => x.Id == taskAssignmentUser.TaskAssignmentId);

            if (company == null || taskAssignment == null || !company.Users.Any(x => x.Id == taskAssignmentUser.UserId))
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            //ελέγχουμε αν ανήκει ο χρήστης που αναθέτει το task στην εταιρεία και δεν είναι employee
            var assignedFromUser = company.Users.SingleOrDefault(x => x.Id == taskAssignmentUser.AssignedFromUserId);
            if (assignedFromUser == null)
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            if (assignedFromUser.RoleId == Enums.UserRole.EMPLOYEE)
            {
                HttpContext.Response.StatusCode = 403;
                return 0;
            }


            taskAssignmentUser.AssignedAt = DateTime.UtcNow;
            taskAssignmentUser.UserId = user.Id;

            _context.TaskAssignmentUsers.Add(taskAssignmentUser);

            _context.UserNotifications.Add(new UserNotification()
            {
                UserId = taskAssignmentUser.UserId,
                NotificationType = Enums.NotificationType.ASSIGNED_TASK,
                CreatedAt = DateTime.UtcNow,
                Title = "New Task Assignment",
                Description = "The task with name " + company.GetTaskAssignments().Single(x => x.Id == taskAssignmentUser.TaskAssignmentId).Title + " has been assigned to you from " + assignedFromUser.NickName + "."
            });

            _context.TaskLogs.Add(new TaskLog()
            {
                TaskAssignmentId = taskAssignmentUser.TaskAssignmentId,
                UserId = id,
                TaskLogType = Enums.TaskLogType.ADDED_USER,
                CreatedAt = DateTime.UtcNow,
                Title = "User '" + user.NickName + "' is from now assigned to task '" + taskAssignment.Title + "'",
                Description = ""
            });

            _context.SaveChanges();


            return taskAssignmentUser.Id;

        }

        /// <summary>
        /// Θα καλείται όταν συνδέει ο χρήστης ένα task με ένα asset.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskAssignmentUser"></param>
        /// <returns></returns>
        [HttpPost("{id}/TaskAssignmentAssets")]
        public int PostTaskAssignmentAsset(int id, TaskAssignmentAsset taskAssignmentAsset)
        {

            var user = _context.Users.SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            var company = _context.Companies.Include(x => x.Users).Include(x => x.Assets).SingleOrDefault(x => x.Id == user.CompanyId);

            if (company == null)
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            if (!company.Users.Any(x => x.Id == user.Id))
            {
                HttpContext.Response.StatusCode = 403;
                return 0;
            }

            if (!company.Assets.Any(x => x.Id == taskAssignmentAsset.AssetId))
            {
                HttpContext.Response.StatusCode = 403;
                return 0;
            }

            var taskAssignment = _context.TaskAssignments.Include(x => x.TaskAssignmentUsers).Include(x => x.TaskAssignmentAssets).SingleOrDefault(x => x.Id == taskAssignmentAsset.TaskAssignmentId);

            if (taskAssignment == null)
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            //ελέγχουμε αν συνδέονται ήδη το task με το asset
            if (taskAssignment.TaskAssignmentAssets.Any(x => x.AssetId == taskAssignmentAsset.AssetId))
            {
                HttpContext.Response.StatusCode = 400;
                return 0;
            }

            taskAssignmentAsset.UserId = user.Id;
            _context.TaskAssignmentAssets.Add(taskAssignmentAsset);

            foreach (var taskUser in taskAssignment.TaskAssignmentUsers.Where(x => x.UserId != user.Id))
            {
                _context.UserNotifications.Add(new UserNotification()
                {
                    UserId = id,
                    NotificationType = Enums.NotificationType.ADDED_ASSET_TO_TASK,
                    CreatedAt = DateTime.UtcNow,
                    Title = "Added Asset to Task",
                    Description = "The asset with name " + company.Assets.Single(x => x.Id == taskAssignmentAsset.AssetId).Name + " has been added to task '" + taskAssignment.Title + "' from '" + user.NickName+ "'."
                });
            }
            
            _context.TaskLogs.Add(new TaskLog()
            {
                TaskAssignmentId = taskAssignment.Id,
                UserId = id,
                TaskLogType = Enums.TaskLogType.ADDED_ASSET_TO_TASK,
                CreatedAt = DateTime.UtcNow,
                Title = "User '" + user.NickName + "' added asset '" + company.Assets.Single(x => x.Id == taskAssignmentAsset.AssetId).Name + "'.",
                Description = ""
            });

            _context.SaveChanges();


            return taskAssignmentAsset.Id;

        }

        /// <summary>
        /// Θα καλείται όταν ο χρήστης προσθέτει ένα comment σε ένα task.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost("{id}/Comments")]
        public Comment PostComment(int id, Comment comment)
        {

            var user = _context.Users.SingleOrDefault(x => x.Id == id);
            var taskAssignment = _context.TaskAssignments.Include(x => x.TaskAssignmentUsers).SingleOrDefault(x => x.Id == comment.TaskAssignmentId);

            if (user == null || taskAssignment == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            if (!comment.IsValid())
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            _context.Comments.Add(comment);

            foreach (var taskAssignmentUser in taskAssignment.TaskAssignmentUsers)
            {
                _context.UserNotifications.Add(new UserNotification()
                {
                    UserId = taskAssignmentUser.UserId,
                    NotificationType = Enums.NotificationType.ADDED_COMMENT,
                    CreatedAt = DateTime.UtcNow,
                    Title = "Added Comment",
                    Description = "User '" + user.NickName + "' added a new comment to task '" + taskAssignment.Title + "'."
                });
            }

            _context.TaskLogs.Add(new TaskLog()
            {
                TaskAssignmentId = comment.TaskAssignmentId,
                UserId = id,
                TaskLogType = Enums.TaskLogType.ADDED_COMMENT,
                CreatedAt = DateTime.UtcNow,
                Title = "User '" + user.NickName + "' post a comment",
                Description = ""
            });

            _context.SaveChanges();

            return comment;

        }

        /// <summary>
        /// Θα καλείται όταν διαγράφει μια ανάθεση ενός εργαζομένου από ένα task.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskAssignmentUserId"></param>
        /// <returns></returns>
        [HttpDelete("{id}/TaskAssignmentUsers/{taskAssignmentUserId}")]
        public int DeleteTaskAssignmentUser(int id, int taskAssignmentUserId)
        {

            var user = _context.Users.SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            var taskAssignmentUser = _context.TaskAssignmentUsers.Include(x => x.TaskAssignment).SingleOrDefault(x => x.Id == taskAssignmentUserId);

            if (taskAssignmentUser == null)
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            _context.UserNotifications.Add(new UserNotification()
            {
                UserId = taskAssignmentUser.UserId,
                NotificationType = Enums.NotificationType.UNASSIGNED_TASK,
                CreatedAt = DateTime.UtcNow,
                Title = "Remove Task Assignment",
                Description = "The task with name " + taskAssignmentUser.TaskAssignment.Title + " is not assigned to you yet. User '" + user.NickName + "' removed your assignment."
            });

            _context.TaskLogs.Add(new TaskLog()
            {
                TaskAssignmentId = taskAssignmentUserId,
                UserId = id,
                TaskLogType = Enums.TaskLogType.REMOVED_USER,
                CreatedAt = DateTime.UtcNow,
                Title = "User '" + user.NickName + "' is not assigned any more for task '" + taskAssignmentUser.TaskAssignment.Title + "'",
                Description = ""
            });

            _context.TaskAssignmentUsers.Remove(taskAssignmentUser);

            _context.SaveChanges();

            return id;

        }

        /// <summary>
        /// Θα καλείται όταν αφαιρεί ένα asset από ένα task.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskAssignmentUserId"></param>
        /// <returns></returns>
        [HttpDelete("{id}/TaskAssignmentAssets/{taskAssignmentAssetId}")]
        public int DeleteTaskAssignmentAsset(int id, int taskAssignmentAssetId)
        {

            var user = _context.Users.SingleOrDefault(x => x.Id == id);
            var company = _context.Companies.Include("TaskAssignmentGroups.TaskAssignments").Include(x => x.Assets).SingleOrDefault(x => x.Id == user.CompanyId);

            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            if (company == null)
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            var taskAssignmentAsset = _context.TaskAssignmentAssets.SingleOrDefault(x => x.Id == taskAssignmentAssetId);

            if (taskAssignmentAsset == null)
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            //ελέγχουμε αν το task και το asset ανήκουν στην εταιρεία του χρήστη
            if (!company.GetTaskAssignments().Any(x => x.Id == taskAssignmentAsset.TaskAssignmentId) || !company.Assets.Any(x => x.Id == taskAssignmentAsset.AssetId))
            {
                HttpContext.Response.StatusCode = 403;
                return 0;
            }

            _context.TaskAssignmentAssets.Remove(taskAssignmentAsset);
            _context.SaveChanges();

            return id;

        }

        /// <summary>
        /// Θα καλείται όταν διαγράφει έναν υπάλληλο ο super-supervisor.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public int DeleteEmployee(int id)
        {

            var user = _context.Users.SingleOrDefault(x => x.Id == id);
            var company = _context.Companies.Include(x => x.Users).SingleOrDefault(x => x.Id == user.CompanyId);

            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            foreach (var companyUser in company.Users.Where(x => x.Id != id))
            {
                _context.UserNotifications.Add(new UserNotification()
                {
                    UserId = companyUser.Id,
                    NotificationType = Enums.NotificationType.REMOVED_USER,
                    CreatedAt = DateTime.UtcNow,
                    Title = "Removed User",
                    Description = "User '" + user.NickName + "' removed from company."
                });
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return id;

        }

    }
}
