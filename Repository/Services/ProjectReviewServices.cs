using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Repository.Interfaces;
using Repository.Models;
using Repository.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Repository.Services
{
    public class ProjectReviewServices : IProjectReviewServices
    {
        WesternSchedulerContext context;
        public ProjectReviewServices(WesternSchedulerContext _context)
        {
            context = _context;

        }

        public JsonModel GetProjectReviewSchedulerList()
        {
           var projectReviewList = context.ProjectRevisions.Include(p => p.Project.ProjectNumber.ProjectManager).Include(p => p.Project.ProjectNumber.ProjectDeveloper).Include(p => p.Project.ProjectNumber.Client).Include(p => p.Project.ProjectNumber.Location).Include(p => p.Employee).Include(p => p.Project.ProjectNumber).ToList();
           // var projectReviewList = context.ProjectRevisions.ToList();
            return new JsonModel(projectReviewList, "", (int)Repository.ViewModel.HttpStatusCode.OK, "");
        }

        public JsonModel EventSearchByFilter(SerchModel search)
        {
            EventResponseModel model = new EventResponseModel();
            if (search.ClientName != "")
            {
               var ProjectReviewList = context.ProjectRevisions.Where(p=> p.Project.DepartmentId == search.DepartmentId && p.Project.ProjectNumber.Client.ClientName.Contains(search.ClientName)).Include(p => p.Project).Include(p => p.Project.ProjectNumber).Include(p => p.Project.ProjectNumber.Client).Include(p => p.Project.ProjectNumber.Location).ToList();

                model.StartDate = context.ProjectRevisions.Where(p =>   p.Project.DepartmentId == search.DepartmentId && p.Project.ProjectNumber.Client.ClientName.Contains(search.ClientName)).Select(p => p.StartDate).Min().Value.Date.ToString("yyyy-MM-dd");
                model.EndDate = context.ProjectRevisions.Where(p =>  p.Project.DepartmentId == search.DepartmentId && p.Project.ProjectNumber.Client.ClientName.Contains(search.ClientName)).Select(p => p.EndDate).Max().Value.Date.ToString("yyyy-MM-dd");
                foreach(var x in ProjectReviewList)
                {
                    EventIntialInfo obj = new EventIntialInfo();
                    if (x.Project.ProjectNumber != null)
                    {
                        obj.ClientName = x.Project.ProjectNumber.Client.ClientName;
                        obj.LocationName = x.Project.ProjectNumber.Location.LocationName;
                        obj.StyleBackGroundColor = x.Project.ProjectNumber.Location.Style_BackgroundColor;
                        obj.StyleBorder = x.Project.ProjectNumber.Location.Style_Border;
                        obj.StyleClosedColor = x.Project.ProjectNumber.Location.Style_ClosedColor;
                        obj.StyleColor = x.Project.ProjectNumber.Location.Style_Color;
                        obj.ActiveStyle = x.Project.ProjectNumber.Location.ActiveStyle;
                        obj.PendingStyle = x.Project.ProjectNumber.Location.PendingStyle;
                        obj.ClosedStyle = x.Project.ProjectNumber.Location.ClosedStyle;
                        obj.AddressLine1 = x.Project.ProjectNumber.AddressLine1;
                        obj.AddressLine2 = x.Project.ProjectNumber.AddressLine2;
                    }
                    obj.DepartmentId = x.Project.DepartmentId;
                    obj.EmployeeId = x.EmployeeId;
                    obj.EndDate = x.EndDate;
                    obj.StartDate = x.StartDate;
                    obj.Status = x.Project.Status;
                    obj.ProjectType = x.Project.ProjectType;
                    obj.RevisionId = x.ProjectRevisionId;
                    model.EventInfo.Add(obj);

                }


                return new JsonModel(model, "", (int)Repository.ViewModel.HttpStatusCode.OK, "");
            }
            else  
            {
                var projectReviewList = context.ProjectRevisions.Where(p =>p.Project.ProjectNumber.ProjectNumberId.Equals(search.ProjectNumberId)).Include(p => p.Project).Include(p => p.Project.ProjectNumber).Include(p => p.Project.ProjectNumber.Client).Include(p => p.Project.ProjectNumber.Location).ToList();

                model.StartDate = context.ProjectRevisions.Where(p => p.Project.DepartmentId == search.DepartmentId && p.Project.ProjectNumber.ProjectNumberId.Equals(search.ProjectNumberId)).Select(p => p.StartDate).Min().Value.Date.ToString("yyyy-MM-dd");
                model.EndDate = context.ProjectRevisions.Where(p => p.Project.DepartmentId == search.DepartmentId && p.Project.ProjectNumber.ProjectNumberId.Equals(search.ProjectNumberId)).Select(p => p.EndDate).Max().Value.Date.ToString("yyyy-MM-dd");
                foreach (var x in projectReviewList)
                {
                    EventIntialInfo obj = new EventIntialInfo();
                    if (x.Project.ProjectNumber != null)
                    {
                        obj.ClientName = x.Project.ProjectNumber.Client.ClientName;
                        obj.LocationName = x.Project.ProjectNumber.Location.LocationName;
                        obj.StyleBackGroundColor = x.Project.ProjectNumber.Location.Style_BackgroundColor;
                        obj.StyleBorder = x.Project.ProjectNumber.Location.Style_Border;
                        obj.StyleClosedColor = x.Project.ProjectNumber.Location.Style_ClosedColor;
                        obj.StyleColor = x.Project.ProjectNumber.Location.Style_Color;
                        obj.ActiveStyle = x.Project.ProjectNumber.Location.ActiveStyle;
                        obj.PendingStyle = x.Project.ProjectNumber.Location.PendingStyle;
                        obj.ClosedStyle = x.Project.ProjectNumber.Location.ClosedStyle;
                        obj.AddressLine1 = x.Project.ProjectNumber.AddressLine1;
                        obj.AddressLine2 = x.Project.ProjectNumber.AddressLine2;
                    }
                    obj.DepartmentId = x.Project.DepartmentId;
                    obj.EmployeeId = x.EmployeeId;
                    obj.EndDate = x.EndDate;
                    obj.StartDate = x.StartDate;
                    obj.Status = x.Project.Status;
                    obj.ProjectType = x.Project.ProjectType;
                    obj.RevisionId = x.ProjectRevisionId;
                    model.EventInfo.Add(obj);

                }
                return new JsonModel(model, "", (int)Repository.ViewModel.HttpStatusCode.OK, "");
            }

        }

        public JsonModel FilterSuggestion(string search)
        {
            if (context.ProjectRevisions.Any(p => p.Project.ProjectNumber.Client.ClientName.StartsWith(search)))
            {
                var projectReviewList = context.ProjectRevisions.Where(p => p.Project.ProjectNumber.Client.ClientName.StartsWith(search) || p.Project.ProjectNumber.ProjectNumber.StartsWith(search)).Select(p => new ClientSearch { Name = p.Project.ProjectNumber.Client.ClientName, ClientId = p.Project.ProjectNumber.ClientId }).ToList();
                return new JsonModel(projectReviewList, "", (int)Repository.ViewModel.HttpStatusCode.OK, "");
            }
            else
            {
                var projectReviewList = context.ProjectRevisions.Where(p => p.Project.ProjectNumber.ProjectNumber.StartsWith(search) || p.Project.ProjectNumber.ProjectNumber.Contains(search)).Select(p => new ProjectSearch { Name = p.Project.ProjectNumber.ProjectNumber, ProjectNumberId = p.Project.ProjectNumber.ProjectNumberId }).ToList();
                return new JsonModel(projectReviewList, "", (int)Repository.ViewModel.HttpStatusCode.OK, "");
            }

        }

        public JsonModel ChangeProjectStatus(ProjectModel project)
        {
            var projectDetail = context.Projects.Where(p => p.ProjectId == project.ProjectId).FirstOrDefault();

            if(projectDetail!=null)
            {
                projectDetail.Status = project.Status;
                context.Entry(projectDetail).State = EntityState.Modified;
                context.SaveChanges();
            }
            return new JsonModel(projectDetail, "Updated Successfully", (int)Repository.ViewModel.HttpStatusCode.OK, "");

        }

        public JsonModel UpdateProjectSchedularOffDay(ProjectScheduleViewModel model)
        {


            var objData = context.Projects.Where(p => p.ProjectId == model.ProjectId).FirstOrDefault();
            // var objData = new Projects();
            objData.ProjectType = model.ProjectType;
            objData.DateModified = DateTime.Now;
            context.Entry(objData).State = EntityState.Modified;
            context.SaveChanges();
            var objrevison = context.ProjectRevisions.Where(p => p.ProjectRevisionId == model.ProjectRevisionId).FirstOrDefault();
            objrevison.StartDate = model.StartDate.AddMinutes(model.TimezoneOffset);
            objrevison.EndDate = model.EndDate.AddMinutes(model.TimezoneOffset);
            objrevison.AllDay = model.Allday;
            context.Entry(objrevison).State = EntityState.Modified;
            context.SaveChanges();
            return new JsonModel(null, "Update Successfully", (int)Repository.ViewModel.HttpStatusCode.OK, "");


        }
        public JsonModel SaveProjectSchedularOffDay(ProjectScheduleViewModel model)
        {
            // var objData = context.Projects.Where(p => p.ProjectId == model.ProjectId).FirstOrDefault();
            var objData = new Projects();
            objData.Status = model.Status;
            objData.ProjectType = model.ProjectType;
            objData.DateModified = DateTime.Now;
            objData.DateCreated = DateTime.Now;
            objData.DepartmentId = model.Departmentid;
            objData.ProjectNumberId = null;
            context.Entry(objData).State = EntityState.Added;
            context.SaveChanges();
            //var objrevison = context.ProjectRevisions.Where(p => p.ProjectRevisionId == model.ProjectRevisionId).FirstOrDefault();
            Models.ProjectRevisions objrevison = new Models.ProjectRevisions();
            objrevison.ProjectId = objData.ProjectId;
            //objrevison.ProjectRevisionId = 1;
            objrevison.RevisionNumber = 0;
            objrevison.EmployeeId = model.ResourceId;
            objrevison.StartDate = model.StartDate.AddMinutes(model.TimezoneOffset);
            objrevison.EndDate = model.EndDate.AddMinutes(model.TimezoneOffset);
            objrevison.AllDay = model.Allday;
            // context.Entry(objrevison).State = EntityState.Modified;
            context.ProjectRevisions.Add(objrevison);
            context.SaveChanges();
            //    //context.ProjectRevisions.Add(objrevison);

            return new JsonModel(null, "Save Successfully", (int)Repository.ViewModel.HttpStatusCode.OK, "");



        }


        public JsonModel GetProjectReviewSchedulerById(int id)
        {

            var projectReviewList = context.ProjectRevisions.Where(p => p.ProjectRevisionId == id).Include(p => p.Project.ProjectNumber.ProjectManager).Include(p => p.Project.ProjectNumber.ProjectDeveloper).Include(p => p.Project.ProjectNumber.Client).Include(p => p.Project.ProjectNumber.Location).Include(p => p.Employee).Include(p => p.Project.ProjectNumber);

            return new JsonModel(projectReviewList, "", (int)Repository.ViewModel.HttpStatusCode.OK, "");


        }


        public JsonModel UpdateProjectReviewScheduler(ProjectScheduleViewModel model)
        {
            var clientModel = context.Clients.Where(p => p.ClientId == model.clientId).FirstOrDefault();

            //    // Models.Clients clientData = new Models.Clients();
            clientModel.ClientName = model.ClientName;
            clientModel.City = model.City;
            clientModel.State = model.State;
            clientModel.ZipCode = model.ZipCode;
            clientModel.AddressLine1 = model.Address1;
            clientModel.AddressLine2 = model.Address2;
            if (clientModel != null)
            {
                context.Entry(clientModel).State = EntityState.Modified;
                context.SaveChanges();
            }

            var projectNumber = context.ProjectNumbers.Where(p => p.ProjectNumberId == model.ProjectNumberId).FirstOrDefault();

            //Models.ProjectNumbers obj = new Models.ProjectNumbers();
            if (model.ProjectManagerId > 0)
                projectNumber.ProjectManagerId = model.ProjectManagerId;
            if (model.ProjectDeveloperId > 0)
                projectNumber.ProjectDeveloperId = model.ProjectDeveloperId;
            projectNumber.NickName = model.NickName;
            projectNumber.LocationId = model.LocationId;
            projectNumber.AddressLine1 = model.Address1;
            projectNumber.AddressLine2 = model.Address2;
            projectNumber.ClientId = clientModel.ClientId;
            projectNumber.DateModified = DateTime.Now;

            if (projectNumber != null)
            {
                context.Entry(projectNumber).State = EntityState.Modified;
                context.SaveChanges();
            }

            //    //string TotalPNnumber = (context.ProjectNumbers.ToList().Count + 1).ToString();
            //    //obj.ProjectNumber = "PN0000" + TotalPNnumber;
            //    //context.ProjectNumbers.Add(obj);
            //    //context.SaveChanges();
            foreach (var x in model.AddressDescriptors)
            {
                if(x.ProjectNumberId == 0)
                {
                    x.ProjectNumberId = projectNumber.ProjectNumberId;
                    x.AddressDescriptorId = 0;
                    context.Entry(x).State = EntityState.Added;
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(x).State = EntityState.Modified;
                    context.SaveChanges();
                }


            }
            //context.AddressDescriptors.AddRange(model.AddressDescriptors);
            //context.SaveChanges();


            //    //Models.Projects objData = new Projects();

            var objData = context.Projects.Where(p => p.ProjectId == model.ProjectId).FirstOrDefault();

            objData.Status = model.Status;
            objData.ProjectType = model.ProjectType;
            objData.DateModified = DateTime.Now;
            objData.DateCreated = DateTime.Now;
            objData.DepartmentId = model.Departmentid;
            objData.ProjectNumberId = projectNumber.ProjectNumberId;
            context.Entry(objData).State = EntityState.Modified;
            context.SaveChanges();


            // End date calculation
            DateTime EndDateTime = model.StartDate;
            TimeSpan sp = new TimeSpan(17, 0, 0);
            EndDateTime = EndDateTime.Date + sp;
            EndDateTime = EndDateTime.AddMinutes(model.TimezoneOffset);
            double availableHours = (EndDateTime - model.StartDate).TotalHours;
            if (availableHours >= model.ProjectHours)
            {
                EndDateTime = model.StartDate.AddHours(model.ProjectHours);
            }
            else
            {
                double totalHours = model.ProjectHours;
                DateTime newStartDate = model.StartDate;
                EndDateTime = newStartDate;
                do
                {
                    EndDateTime = EndDateTime.AddHours(availableHours);
                    totalHours -= availableHours;
                    if (totalHours > 0)
                    {
                        EndDateTime = EndDateTime.AddHours(16);
                    }
                    availableHours = totalHours <= 8 ? totalHours : 8;
                }
                while (totalHours > 0);
            }


            var objrevison = context.ProjectRevisions.Where(p => p.ProjectRevisionId == model.ProjectRevisionId).FirstOrDefault();
            //    //Models.ProjectRevisions objrevison = new Models.ProjectRevisions();
            if(model.ProjectManagerId > 0)
                objrevison.ProjectManagerId = model.ProjectManagerId;
            if(model.ProjectDeveloperId > 0)
                objrevison.ProjectDeveloperId = model.ProjectDeveloperId;
            objrevison.ProjectId = objData.ProjectId;
            objrevison.Hours = model.ProjectHours;
            //objrevison.ProjectRevisionId = 1;
            objrevison.RevisionNumber = model.NumberOfRevision;
            objrevison.EmployeeId = model.ResourceId;
            objrevison.StartDate = model.StartDate;
            objrevison.EndDate = EndDateTime;
            context.Entry(objrevison).State = EntityState.Modified;
            context.SaveChanges();
            //    //context.ProjectRevisions.Add(objrevison);

            return new JsonModel(null, "Save Successfully", (int)Repository.ViewModel.HttpStatusCode.OK, "");
            //    //var projectReviewList = "";
            //return new JsonModel(projectReviewList, "", (int)Repository.ViewModel.HttpStatusCode.OK, "");
        }

        public JsonModel SaveProjectSchedulerWithProjectNumber(ProjectScheduleViewModel model)
        {
            var projectNumber = context.ProjectNumbers.Where(p=> p.ClientId == model.clientId && p.NickName == model.NickName).FirstOrDefault();
            if (projectNumber == null) {
                var newClientId = 0;
                
                if (model.clientId == null)
                {
                    Models.Clients newClient = new Models.Clients();
                    newClient.ClientName = model.ClientName;
                    context.Entry(newClient).State = EntityState.Added;
                    context.SaveChanges();
                    newClientId = newClient.ClientId;
                }
                

                Models.ProjectNumbers newProjectNumber = new Models.ProjectNumbers();
                newProjectNumber.ProjectNumber = model.ProjectNumber;
                newProjectNumber.ClientId = model.clientId == null ? newClientId : model.clientId;
                newProjectNumber.NickName = model.NickName;
                newProjectNumber.ProjectManagerId = model.ProjectManagerId;
                newProjectNumber.ProjectDeveloperId = model.ProjectDeveloperId;
                newProjectNumber.NickName = model.NickName;
                newProjectNumber.LocationId = model.LocationId;
                newProjectNumber.AddressLine1 = model.Address1;
                newProjectNumber.AddressLine2 = model.Address2;
                newProjectNumber.DateCreated = DateTime.Now;
                newProjectNumber.DateModified = DateTime.Now;
                newProjectNumber.City = model.City;
                newProjectNumber.ZipCode = model.ZipCode;

                context.ProjectNumbers.Add(newProjectNumber);
                context.SaveChanges();

                foreach (var x in model.AddressDescriptors)
                {
                    x.ProjectNumberId = newProjectNumber.ProjectNumberId;
                    x.AddressDescriptorId = 0;
                    context.Entry(x).State = EntityState.Added;
                    //context.Entry(x.ProjectNumber).State = EntityState.Detached;
                    context.SaveChanges();
                }
                projectNumber = newProjectNumber;
            }

            Models.Projects pro = new Models.Projects();
            pro.ProjectNumberId = projectNumber.ProjectNumberId;
            pro.Status = model.Status;
            pro.ProjectType = model.ProjectType;
            pro.DepartmentId = model.Departmentid;
            pro.DateCreated = DateTime.Now;
            pro.DateModified = DateTime.Now;
            context.Entry(pro).State = EntityState.Added;
            context.SaveChanges();

            // End date calculation
            DateTime EndDateTime = model.StartDate;
            TimeSpan sp = new TimeSpan(17, 0, 0);
            EndDateTime = EndDateTime.Date + sp;
            EndDateTime = EndDateTime.AddMinutes(model.TimezoneOffset);
            double availableHours = (EndDateTime - model.StartDate).TotalHours;
            if (availableHours >= model.ProjectHours)
            {
                EndDateTime = model.StartDate.AddHours(model.ProjectHours);
            }
            else
            {
                double totalHours = model.ProjectHours;
                DateTime newStartDate = model.StartDate;
                EndDateTime = newStartDate;
                do
                {
                    EndDateTime = EndDateTime.AddHours(availableHours);
                    totalHours -= availableHours;
                    if (totalHours > 0)
                    {
                        EndDateTime = EndDateTime.AddHours(16);
                    }
                    availableHours = totalHours <= 8 ? totalHours : 8;
                }
                while (totalHours > 0);
            }

            Models.ProjectRevisions objrevison = new Models.ProjectRevisions();
            objrevison.ProjectManagerId = model.ProjectManagerId;
            objrevison.ProjectDeveloperId = model.ProjectDeveloperId;
            objrevison.ProjectId = pro.ProjectId;
            objrevison.Hours = model.ProjectHours;
            //objrevison.ProjectRevisionId = 1;
            objrevison.RevisionNumber = model.NumberOfRevision;
            objrevison.EmployeeId = model.ResourceId;
            objrevison.StartDate = model.StartDate.AddMinutes(model.TimezoneOffset);
            objrevison.EndDate = EndDateTime;
            // context.Entry(objrevison).State = EntityState.Modified;
            context.ProjectRevisions.Add(objrevison);
            context.SaveChanges();
            //    //context.ProjectRevisions.Add(objrevison);

            var NewprojectNumbers = context.ProjectNumbers.Select(p => new ViewModelProjectNumber { ProjectNumberId= p.ProjectNumberId, ProjectNumber= p.ProjectNumber }).ToList();

            return new JsonModel(NewprojectNumbers, "Save Successfully", (int)Repository.ViewModel.HttpStatusCode.OK, "");
        }

        public JsonModel GetProjectByClientId(int clientId, string nickName) {
            var projectNum = context.ProjectNumbers.Where(p=> p.ClientId == clientId && p.NickName == nickName).Include(p => p.AddressDescriptors).FirstOrDefault();

            var clientName = context.Clients.Where(p=>p.ClientId == clientId).FirstOrDefault().ClientName;

            if (projectNum != null) {
                var project = context.Projects.Where(p=> p.ProjectNumberId == projectNum.ProjectNumberId).Include(p => p.ProjectNumber.ProjectManager).Include(p => p.ProjectNumber.ProjectDeveloper).Include(p=>p.ProjectRevisions).ToList();

                return new JsonModel( new {
                    projectNum,
                    project,
                    clientName
                }, "", (int)Repository.ViewModel.HttpStatusCode.OK, "");
            }


            return new JsonModel( new {
                projectNum,
                clientName
            }, "", (int)Repository.ViewModel.HttpStatusCode.OK, "");
        }


        public JsonModel DeleteProjectScheduler(int id)
        {

            var projectsRevision = context.ProjectRevisions.Where(p => p.ProjectRevisionId == id).FirstOrDefault();

            if (projectsRevision != null)
            {
                context.Entry(projectsRevision).State = EntityState.Deleted;
                context.SaveChanges();
            }
            
            return new JsonModel(null, "Delete Successfully", (int)Repository.ViewModel.HttpStatusCode.OK, "");
        }

        public JsonModel DeleteProjectSchedulerWithProjectNumber(int id)
        {
            return new JsonModel(null, "Delete Successfully", (int)Repository.ViewModel.HttpStatusCode.OK, "");
        }

        public JsonModel UpdateProjectSchedulerWithProjectNumber(ProjectScheduleViewModel model)
        {


            var clientModel = context.Clients.Where(p => p.ClientId == model.clientId).FirstOrDefault();

            //    // Models.Clients clientData = new Models.Clients();
            clientModel.ClientName = model.ClientName;
            clientModel.City = model.City;
            clientModel.State = model.State;
            clientModel.ZipCode = model.ZipCode;
            clientModel.AddressLine1 = model.Address1;
            clientModel.AddressLine2 = model.Address2;
            if (clientModel != null)
            {
                context.Entry(clientModel).State = EntityState.Modified;
                context.SaveChanges();
            }

            var projectNumber = context.ProjectNumbers.Where(p => p.ProjectNumberId == model.ProjectNumberId).FirstOrDefault();

            //Models.ProjectNumbers obj = new Models.ProjectNumbers();
            projectNumber.ProjectManagerId = model.ProjectManagerId;
            projectNumber.ProjectDeveloperId = model.ProjectDeveloperId;
            projectNumber.NickName = model.NickName;
            projectNumber.LocationId = model.LocationId;
            projectNumber.AddressLine1 = model.Address1;
            projectNumber.AddressLine2 = model.Address2;
            projectNumber.ClientId = clientModel.ClientId;
            projectNumber.DateModified = DateTime.Now;

            if (projectNumber != null)
            {
                context.Entry(projectNumber).State = EntityState.Modified;
                context.SaveChanges();
            }

            //    //string TotalPNnumber = (context.ProjectNumbers.ToList().Count + 1).ToString();
            //    //obj.ProjectNumber = "PN0000" + TotalPNnumber;
            //    //context.ProjectNumbers.Add(obj);
            //    //context.SaveChanges();
            foreach (var x in model.AddressDescriptors)
            {
                x.ProjectNumberId = projectNumber.ProjectNumberId;
                context.Entry(projectNumber).State = EntityState.Modified;
                context.SaveChanges();

            }
            //context.AddressDescriptors.AddRange(model.AddressDescriptors);
            //context.SaveChanges();


            //    //Models.Projects objData = new Projects();

            var objData = context.Projects.Where(p => p.ProjectId == model.ProjectId).FirstOrDefault();
            objData.Status = model.Status;
            objData.ProjectType = model.ProjectType;
            objData.DateModified = DateTime.Now;
            objData.DateCreated = DateTime.Now;
            objData.DepartmentId = model.Departmentid;
            objData.ProjectNumberId = projectNumber.ProjectNumberId;
            context.Entry(objData).State = EntityState.Modified;
            context.SaveChanges();
            var objrevison = context.ProjectRevisions.Where(p => p.ProjectRevisionId == model.ProjectRevisionId).FirstOrDefault();
            // Models.ProjectRevisions objrevison = new Models.ProjectRevisions();
            objrevison.ProjectManagerId = model.ProjectManagerId;
            objrevison.ProjectDeveloperId = model.ProjectDeveloperId;
            objrevison.ProjectId = objData.ProjectId;
            objrevison.Hours = model.ProjectHours;
            //objrevison.ProjectRevisionId = 1;
            objrevison.RevisionNumber = 1;
            objrevison.EmployeeId = model.ResourceId;
            objrevison.StartDate = model.StartDate;
            objrevison.EndDate = model.EndDate;
            context.Entry(objrevison).State = EntityState.Modified;
            //context.ProjectRevisions.Add(objrevison);
            context.SaveChanges();
            //    //context.ProjectRevisions.Add(objrevison);

            return new JsonModel(null, "Save Successfully", (int)Repository.ViewModel.HttpStatusCode.OK, "");
        }

        public JsonModel SplitProjectRevision(int revisionId)
        {
            var projectRevisionDetails = context.ProjectRevisions.Where(p => p.ProjectRevisionId == revisionId).FirstOrDefault();
            var list = context.Projects.Where(p => p.ProjectId == projectRevisionDetails.ProjectId).FirstOrDefault();

            Models.Projects newProject = new Models.Projects();
            newProject.CreatedBy = list.CreatedBy;
            newProject.DateCreated = DateTime.Now;
            newProject.DateModified = DateTime.Now;
            newProject.DepartmentId = list.DepartmentId;
           // newProject.Employee = list.Employee;            
            newProject.ModifiedBy = list.ModifiedBy;
            newProject.ParentProjectId = list.ParentProjectId;
            newProject.ProjectNumber = list.ProjectNumber;
            newProject.ProjectNumberId = list.ProjectNumberId;
            //newProject.ProjectRevisions = list.ProjectRevisions;
            newProject.ProjectType = list.ProjectType;
            newProject.Status = list.Status;

            context.Entry(newProject).State = EntityState.Added;
            context.SaveChanges();


            if (list.ProjectType == "ClientProject")
            {
                var splitHours = (double)projectRevisionDetails.Hours / 2;
                projectRevisionDetails.EndDate = projectRevisionDetails.StartDate.Value.AddHours(splitHours);
                projectRevisionDetails.DateModified = DateTime.Now;
                projectRevisionDetails.Hours = splitHours;
                context.Entry(projectRevisionDetails).State = EntityState.Modified;
                ProjectRevisions obj = new ProjectRevisions();
                obj.ProjectId = newProject.ProjectId;
                obj.RevisionNumber = projectRevisionDetails.RevisionNumber;
                obj.StartDate = projectRevisionDetails.EndDate;
                obj.EndDate = projectRevisionDetails.EndDate.Value.AddHours(splitHours);
                obj.EmployeeId = projectRevisionDetails.EmployeeId;
                obj.Hours = splitHours;
                obj.AllDay = false;
                obj.ProjectManagerId = projectRevisionDetails.ProjectManagerId;
                obj.ProjectDeveloperId = projectRevisionDetails.ProjectDeveloperId;
                obj.DateCreated = DateTime.Now;
                // projectRevisionDetails.ProjectRevisionId = 0;
                context.Entry(obj).State = EntityState.Added;

                context.SaveChanges();
            }
            else
            {
                var splitHours = (double)(projectRevisionDetails.EndDate-projectRevisionDetails.StartDate).Value.Hours/2;
               
                projectRevisionDetails.EndDate = projectRevisionDetails.StartDate.Value.AddHours(splitHours);
                projectRevisionDetails.AllDay = false;
                context.Entry(projectRevisionDetails).State = EntityState.Modified;
                context.SaveChanges();
                ProjectRevisions obj = new ProjectRevisions();
                obj.ProjectId = newProject.ProjectId;
                obj.RevisionNumber = projectRevisionDetails.RevisionNumber;
                obj.StartDate = projectRevisionDetails.EndDate;
                obj.EndDate = projectRevisionDetails.EndDate.Value.AddHours(splitHours);
                obj.Hours = projectRevisionDetails.Hours;
                obj.AllDay = false;
                obj.EmployeeId = projectRevisionDetails.EmployeeId;
                obj.ProjectManagerId = projectRevisionDetails.ProjectManagerId;
                obj.ProjectDeveloperId = projectRevisionDetails.ProjectDeveloperId;
                obj.DateCreated = DateTime.Now;
               // projectRevisionDetails.ProjectRevisionId = 0;
                context.Entry(obj).State = EntityState.Added;
                context.SaveChanges();

            }
            return new JsonModel(null, "Save Successfully", (int)Repository.ViewModel.HttpStatusCode.OK, "");
        }

        public JsonModel DuplicateProjectRevision(int revisionId)
        {
            var projectRevisionDetails = context.ProjectRevisions.Where(p => p.ProjectRevisionId == revisionId).FirstOrDefault();
            var list = context.Projects.Where(p => p.ProjectId == projectRevisionDetails.ProjectId).FirstOrDefault();

            Models.Projects newProject = new Models.Projects();
            newProject.CreatedBy = list.CreatedBy;
            newProject.DateCreated = DateTime.Now;
            newProject.DateModified = DateTime.Now;
            newProject.DepartmentId = list.DepartmentId;
            // newProject.Employee = list.Employee;
            // newProject.EmployeeId = list.EmployeeId;
            newProject.ModifiedBy = list.ModifiedBy;
            newProject.ParentProjectId = list.ParentProjectId;
            newProject.ProjectNumber = list.ProjectNumber;
            newProject.ProjectNumberId = list.ProjectNumberId;
            //newProject.ProjectRevisions = list.ProjectRevisions;
            newProject.ProjectType = list.ProjectType;
            newProject.Status = list.Status;

            context.Entry(newProject).State = EntityState.Added;
            context.SaveChanges();


            if (list.ProjectType == "ClientProject")
            {
                ProjectRevisions obj = new ProjectRevisions();
                obj.ProjectId = newProject.ProjectId;
                obj.RevisionNumber = projectRevisionDetails.RevisionNumber;
                obj.StartDate = projectRevisionDetails.StartDate;
                obj.EndDate = projectRevisionDetails.EndDate;
                obj.Hours = projectRevisionDetails.Hours;
                obj.AllDay = false;
                obj.EmployeeId = projectRevisionDetails.EmployeeId;
                obj.ProjectManagerId = projectRevisionDetails.ProjectManagerId;
                obj.ProjectDeveloperId = projectRevisionDetails.ProjectDeveloperId;
                obj.DateCreated = DateTime.Now;
                // projectRevisionDetails.ProjectRevisionId = 0;
                context.Entry(obj).State = EntityState.Added;
                context.SaveChanges();
            }
            else
            {
                ProjectRevisions obj = new ProjectRevisions();
                obj.ProjectId = newProject.ProjectId;
                obj.RevisionNumber = projectRevisionDetails.RevisionNumber;
                obj.StartDate = projectRevisionDetails.StartDate;
                obj.EndDate = projectRevisionDetails.EndDate;
                obj.Hours = projectRevisionDetails.Hours;
                obj.AllDay = projectRevisionDetails.AllDay;
                obj.EmployeeId = projectRevisionDetails.EmployeeId;
                obj.ProjectManagerId = projectRevisionDetails.ProjectManagerId;
                obj.ProjectDeveloperId = projectRevisionDetails.ProjectDeveloperId;
                obj.DateCreated = DateTime.Now;
                // projectRevisionDetails.ProjectRevisionId = 0;
                context.Entry(obj).State = EntityState.Added;
                context.SaveChanges();

            }
            return new JsonModel(null, "Save Successfully", (int)Repository.ViewModel.HttpStatusCode.OK, "");
        }

        public JsonModel SaveProjectScheduler(ProjectScheduleViewModel model)
        {
            var clientId = model.clientId;

            if (clientId == 0) {
                Models.Clients clientData = new Models.Clients();
                clientData.ClientName = model.ClientName;
                clientData.City = model.City;
                clientData.State = model.State;
                clientData.ZipCode = model.ZipCode;
                clientData.AddressLine1 = model.Address1;
                clientData.AddressLine2 = model.Address2;
                context.Clients.Add(clientData);
                context.SaveChanges();

                clientId = clientData.ClientId;
            }
            
            var projectNumberOrigin = context.ProjectNumbers.Where(p => p.ProjectNumberId == model.ProjectId).FirstOrDefault();

            if (projectNumberOrigin == null) {
                Models.ProjectNumbers obj = new Models.ProjectNumbers();

                if(model.ProjectManagerId > 0)
                    obj.ProjectManagerId = model.ProjectManagerId;
                if(model.ProjectDeveloperId > 0)
                    obj.ProjectDeveloperId = model.ProjectDeveloperId;
                obj.NickName = model.NickName;
                obj.LocationId = model.LocationId;
                obj.AddressLine1 = model.Address1;
                obj.AddressLine2 = model.Address2;
                obj.ClientId = clientId;
                obj.DateCreated = DateTime.Now;
                obj.DateModified = DateTime.Now;
                //string TotalPNnumber = (context.ProjectNumbers.ToList().Count + 1).ToString();
                obj.ProjectNumber = model.ProjectNumber;
                context.ProjectNumbers.Add(obj);
                context.SaveChanges();
                foreach (var x in model.AddressDescriptors)
                {
                    x.ProjectNumberId = obj.ProjectNumberId;
                    x.AddressDescriptorId = 0;
                    context.Entry(x).State = EntityState.Added;
                    //context.Entry(x.ProjectNumber).State = EntityState.Detached;
                    context.SaveChanges();
                }
                projectNumberOrigin = obj;

            }
            

            //context.SaveChanges();

            Models.Projects objData = new Projects();


            objData.Status = model.Status;
            objData.ProjectType = model.ProjectType;
            objData.DateModified = DateTime.Now;
            objData.DateCreated = DateTime.Now;
            // objData.EmployeeId = model.ResourceId;
            objData.DepartmentId = model.Departmentid;
            objData.ProjectNumberId = projectNumberOrigin.ProjectNumberId;
            context.Projects.Add(objData);
            context.SaveChanges();

            // End date calculation
            DateTime EndDateTime = model.StartDate;
            TimeSpan sp = new TimeSpan(17, 0, 0);
            EndDateTime = EndDateTime.Date + sp;
            EndDateTime = EndDateTime.AddMinutes(model.TimezoneOffset);
            double availableHours = (EndDateTime - model.StartDate.AddMinutes(model.TimezoneOffset)).TotalHours;
            if (availableHours >= model.ProjectHours)
            {
                EndDateTime = model.StartDate.AddMinutes(model.TimezoneOffset).AddHours(model.ProjectHours);
            }
            else
            {
                double totalHours = model.ProjectHours;
                DateTime newStartDate = model.StartDate.AddMinutes(model.TimezoneOffset);
                EndDateTime = newStartDate;
                do
                {
                    EndDateTime = EndDateTime.AddHours(availableHours);
                    totalHours -= availableHours;
                    if (totalHours > 0)
                    {
                        EndDateTime = EndDateTime.AddHours(16);
                    }
                    availableHours = totalHours <= 8 ? totalHours : 8;
                }
                while (totalHours > 0);
            }

            Models.ProjectRevisions objrevison = new Models.ProjectRevisions();

            if(projectNumberOrigin.ProjectManagerId > 0)
                objrevison.ProjectManagerId = projectNumberOrigin.ProjectManagerId;
            if(model.ProjectDeveloperId > 0)
                objrevison.ProjectDeveloperId = model.ProjectDeveloperId;
            objrevison.ProjectId = objData.ProjectId;
            objrevison.Hours = model.ProjectHours;
            objrevison.RevisionNumber = model.NumberOfRevision;
            objrevison.EmployeeId = model.ResourceId;
            objrevison.StartDate = model.StartDate.AddMinutes(model.TimezoneOffset);
            objrevison.EndDate = EndDateTime;
            context.ProjectRevisions.Add(objrevison);
            context.SaveChanges();

            return new JsonModel(null, "Save Successfully test", (int)Repository.ViewModel.HttpStatusCode.OK, "");
        }

        public JsonModel AlterRevision(RevisionInfoModel model)
        {
            var projectRevisionDetails = context.ProjectRevisions.Where(p => p.ProjectRevisionId == model.RevisionId).FirstOrDefault();
            var projectDetail = context.Projects.Where(p => p.ProjectId == model.ProjectId).FirstOrDefault();
            if (model.ResourceId > 0)
            {
                //Update resource
                // projectDetail.EmployeeId = model.ResourceId;
                projectDetail.DateModified = DateTime.Now;
                context.Entry(projectDetail).State = EntityState.Modified;
                context.SaveChanges();
            }
            

            // get exact hours from startdate and enddate

            DateTime startHour = model.StartDate;
            TimeSpan ts = new TimeSpan(9, 0, 0);
            startHour = startHour.Date + ts;
            startHour = startHour.AddMinutes(model.TimezoneOffset);

            DateTime endHour = model.StartDate;
            ts = new TimeSpan(17, 0, 0);
            endHour = endHour.Date + ts;
            endHour = endHour.AddMinutes(model.TimezoneOffset);

            double projectHours = 0.0;
            double endlimit_start = (endHour - model.StartDate.AddMinutes(model.TimezoneOffset)).TotalHours;
            double start_startlimit = (model.StartDate.AddMinutes(model.TimezoneOffset) - startHour).TotalHours;
            DateTime newStart = endlimit_start >= 0 ? (start_startlimit >= 0 ? model.StartDate.AddMinutes(model.TimezoneOffset) : startHour) : startHour.AddDays(1);

            DateTime tmpStart = model.StartDate.AddMinutes(model.TimezoneOffset);
            DateTime tmpEnd = model.EndDate.AddMinutes(model.TimezoneOffset);

            do
            {
                endlimit_start = (endHour - tmpStart).TotalHours;
                start_startlimit = (tmpStart - startHour).TotalHours;
                double endlimit_end = (endHour - tmpEnd).TotalHours;
                if (endlimit_start >= 0)
                {
                    if (start_startlimit >= 0)
                    {
                        if (endlimit_end > 0)
                        {
                            projectHours += (tmpEnd - tmpStart).TotalHours;
                        } 
                        else
                        {
                            projectHours += endlimit_start;
                        }
                        
                    }
                    else
                    {
                        if (endlimit_end > 0)
                        {
                            projectHours += (tmpEnd - startHour).TotalHours;
                        } 
                        else
                        {
                            projectHours += 8;
                        }
                    }
                }
                startHour = startHour.AddDays(1);
                endHour = endHour.AddDays(1);
                tmpStart = startHour;
            }
            while (tmpEnd > tmpStart);

            DateTime EndDateTime = newStart;
            TimeSpan sp = new TimeSpan(17, 0, 0);
            EndDateTime = EndDateTime.Date + sp;
            EndDateTime = EndDateTime.AddMinutes(model.TimezoneOffset);
            double availableHours = (EndDateTime - newStart).TotalHours;
            if (availableHours >= projectHours)
            {
                EndDateTime = newStart.AddHours(projectHours);
            }
            else
            {
                double totalHours = projectHours;
                DateTime newStartDate = newStart;
                EndDateTime = newStartDate;
                do
                {
                    EndDateTime = EndDateTime.AddHours(availableHours);
                    totalHours -= availableHours;
                    if (totalHours > 0)
                    {
                        EndDateTime = EndDateTime.AddHours(16);
                    }
                    availableHours = totalHours <= 8 ? totalHours : 8;
                }
                while (totalHours > 0);
            }

            projectRevisionDetails.Hours = projectHours;
            //Update startdate, endDate, hours
            projectRevisionDetails.StartDate = newStart;
            projectRevisionDetails.EndDate = EndDateTime;
            projectRevisionDetails.DateModified = DateTime.Now;
            if(model.ResourceId > 0)
                projectRevisionDetails.EmployeeId = model.ResourceId;
            context.Entry(projectRevisionDetails).State = EntityState.Modified;
            context.SaveChanges();
           
            return new JsonModel(null, "Save Successfully", (int)Repository.ViewModel.HttpStatusCode.OK, "");
        }

        public JsonModel GetEventByRevisionId(int projectRevisionId)
        {
            var projectlist = context.ProjectRevisions.Where(p=>p.ProjectRevisionId==projectRevisionId).Include(p => p.Employee).Include(p => p.Project.ProjectNumber).Include(p => p.Project.ProjectNumber.ProjectDeveloper).Include(p => p.Project.ProjectNumber.ProjectManager).Include(p => p.Project.ProjectNumber.Location).Include(p => p.Project.ProjectNumber.Client).Include(p => p.Project.ProjectNumber.AddressDescriptors).ToList();
            return new JsonModel(projectlist, "", (int)Repository.ViewModel.HttpStatusCode.OK, "");
        }

        public JsonModel GetEventsByFilter(DateTime? startdDate, DateTime? endDate, int departmentId, int timezoneOffset)
        {

            List<EventIntialInfo> objMdel = new List<EventIntialInfo>();

            var projectlist = 
                context.ProjectRevisions.Where
                    (p => p.StartDate.Value.Date <= endDate && 
                     p.EndDate.Value.Date >= startdDate && 
                     p.Project.DepartmentId == departmentId).
                     Include(p=>p.Project).
                     Include(p=>p.Project.ProjectNumber).
                     Include(p=>p.Project.ProjectNumber.Client).
                     Include(p=>p.Project.ProjectNumber.Location).
                     Include(p => p.ProjectManager).
                     Include(p => p.ProjectDeveloper).
                     ToList();

            foreach(var x in projectlist) {
                EventIntialInfo obj = new EventIntialInfo();
                if (x.Project.ProjectNumber != null)
                {
                    obj.ClientName = x.Project.ProjectNumber.Client != null ? x.Project.ProjectNumber.Client.ClientName : "";
                    obj.LocationName = x.Project.ProjectNumber.Location != null ? x.Project.ProjectNumber.Location.LocationName : "";
                    obj.StyleBackGroundColor = x.Project.ProjectNumber.Location != null ? x.Project.ProjectNumber.Location.Style_BackgroundColor : "";
                    obj.StyleBorder = x.Project.ProjectNumber.Location != null ? x.Project.ProjectNumber.Location.Style_Border : "";
                    obj.StyleClosedColor = x.Project.ProjectNumber.Location != null ? x.Project.ProjectNumber.Location.Style_ClosedColor : "";
                    obj.StyleColor = x.Project.ProjectNumber.Location != null ? x.Project.ProjectNumber.Location.Style_Color : "";
                    obj.ActiveStyle = x.Project.ProjectNumber.Location != null ? x.Project.ProjectNumber.Location.ActiveStyle : "";
                    obj.PendingStyle = x.Project.ProjectNumber.Location != null ? x.Project.ProjectNumber.Location.PendingStyle : "";
                    obj.ClosedStyle = x.Project.ProjectNumber.Location != null ? x.Project.ProjectNumber.Location.ClosedStyle : "";
                    obj.AddressLine1 = x.Project.ProjectNumber.AddressLine1;
                    obj.AddressLine2 = x.Project.ProjectNumber.AddressLine2;
                    obj.ProjectManagerName = x.ProjectManager != null ? x.ProjectManager.ProjectManagerName : "";
                    obj.ProjectDeveloperName = x.ProjectDeveloper != null ? x.ProjectDeveloper.ProjectDeveloperName : "";
                }
                obj.DepartmentId = x.Project.DepartmentId;
                obj.EmployeeId = x.EmployeeId;
                obj.EndDate = x.EndDate;
                obj.StartDate = x.StartDate;
                obj.Status = x.Project.Status;
                obj.ProjectId = x.Project.ProjectId;
                obj.ProjectType = x.Project.ProjectType;
                obj.RevisionId = x.ProjectRevisionId;
                objMdel.Add(obj);
              }

            return new JsonModel(objMdel, "", (int)Repository.ViewModel.HttpStatusCode.OK, "");
        }

        public JsonModel GetEventsByDepartment(int departmentId)
        {

            List<EventIntialInfo> objMdel = new List<EventIntialInfo>();
            var projectlist = 
                context.ProjectRevisions.Where
                    (p=> p.Project.DepartmentId == departmentId).
                     Include(p=>p.Project).
                     Include(p=>p.Project.ProjectNumber).
                     Include(p=>p.Project.ProjectNumber.Client).
                     Include(p=>p.Project.ProjectNumber.Location).
                     ToList();


            foreach(var x in projectlist) {
                EventIntialInfo obj = new EventIntialInfo();
                if (x.Project.ProjectNumber != null)
                {
                    obj.ClientName = x.Project.ProjectNumber.Client.ClientName;
                    obj.LocationName = x.Project.ProjectNumber.Location.LocationName;
                    obj.StyleBackGroundColor = x.Project.ProjectNumber.Location.Style_BackgroundColor;
                    obj.StyleBorder = x.Project.ProjectNumber.Location.Style_Border;
                    obj.StyleClosedColor = x.Project.ProjectNumber.Location.Style_ClosedColor;
                    obj.StyleColor = x.Project.ProjectNumber.Location.Style_Color;
                    obj.ActiveStyle = x.Project.ProjectNumber.Location.ActiveStyle;
                    obj.PendingStyle = x.Project.ProjectNumber.Location.PendingStyle;
                    obj.ClosedStyle = x.Project.ProjectNumber.Location.ClosedStyle;
                    obj.AddressLine1 = x.Project.ProjectNumber.AddressLine1;
                    obj.AddressLine2 = x.Project.ProjectNumber.AddressLine2;
                }
                obj.DepartmentId = x.Project.DepartmentId;
                obj.EmployeeId = x.EmployeeId;
                obj.EndDate = x.EndDate;
                obj.StartDate = x.StartDate;
                obj.Status = x.Project.Status;
                obj.ProjectId = x.Project.ProjectId;
                obj.ProjectType = x.Project.ProjectType;
                obj.RevisionId = x.ProjectRevisionId;
                objMdel.Add(obj);
              }

            return new JsonModel(objMdel, "", (int)Repository.ViewModel.HttpStatusCode.OK, "");
        }

        public JsonModel GetInfoWithClientId(int id)
        {
            var list =
                context.ProjectNumbers.Where
                    (p => p.ClientId == id).
                     ToList();
            return new JsonModel(list, "", (int)Repository.ViewModel.HttpStatusCode.OK, "");
        }
    }
}
