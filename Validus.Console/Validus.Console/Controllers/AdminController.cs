﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Validus.Console.BusinessLogic;
using Validus.Console.DTO;
using Validus.Core.LogHandling;
using Validus.Core.MVC;
using Validus.Models;

namespace Validus.Console.Controllers
{
    [Authorize(Roles = @"ConsoleRead, ConsoleAdmin")] //, MvcModelStateFilter]
    public class AdminController : Controller
    {
        public readonly IAdminModuleManager _adminModuleManager;
        public readonly ILogHandler _logHandler;

        public AdminController(IAdminModuleManager adminModuleManager, ILogHandler logHandler)
        {
            this._adminModuleManager = adminModuleManager;
            this._logHandler = logHandler;
        }
        #region User Team Link
        [Authorize(Roles = @"ConsoleAdmin, ConsoleUW, ConsoleRead")]
        public ActionResult PersonalSettings()
        {
            return View();
        }

        [Authorize(Roles = @"ConsoleAdmin")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageTeams()
        {
            return PartialView("_ManageTeams");
        }

        public ActionResult ManageUsers()
        {
            return PartialView("_ManageUsers");
        }

        public ActionResult ManageLinks()
        {
            return PartialView("_ManageLinks");
        }

        //[HttpGet]
        //[OutputCache(CacheProfile = "NoCacheProfile")]
        //public ActionResult GetTeam(int teamId)
        //{
        //    try
        //    {
        //        var team = _adminModuleManager.GetTeam(teamId);

        //        return new JsonNetResult
        //        {
        //            Data = team
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
        //        throw new HttpException(500, "Server Error");
        //    }
        //}

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetTeamBySubmissionTypeId(string submissionTypeId)
        {
            try
            {
                var team = _adminModuleManager.GetTeamBySubmissionTypeId(submissionTypeId);

                return new JsonNetResult
                {
                    Data = team
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetTeamsBasicData()
        {
            try
            {
                var teamList = _adminModuleManager.GetTeamsBasicData();

                return new JsonNetResult
                {
                    Data = teamList
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetTeamsFullData()
        {
            try
            {
                var teamList = _adminModuleManager.GetTeamsFullData();

                return new JsonNetResult
                {
                    Data = teamList
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetUser(int? userId)
        {
            try
            {
                var user = _adminModuleManager.GetUser(userId);

                return new JsonNetResult
                {
                    Data = user
                };

            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetUsers()
        {
            try
            {
                var teamList = _adminModuleManager.GetUsers();

                return new JsonNetResult
                {
                    Data = teamList
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetUsersInTeam(int teamId)
        {
            try
            {
                var teamList = _adminModuleManager.GetUsersInTeam(teamId);

                return new JsonNetResult
                {
                    Data = teamList
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }


        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult CreateTeam(TeamDto team)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("'Tilte' and 'Default QuoteExpiry Days' (Integer) are required");
                }

                return new JsonNetResult
                        {
                            Data = _adminModuleManager.CreateTeam(team)
                        };
            }
            catch (ArgumentException ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { ErrorMessage = ex.Message });
            }
            catch (ApplicationException ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { ErrorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { ErrorMessage = ex.Message });
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult EditTeam(TeamDto team)
        {
            try
            {
                var updatedTeam = _adminModuleManager.EditTeam(team);

                return new JsonNetResult
                {
                    Data = updatedTeam
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpDelete]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult DeleteTeam(Team team)
        {
            try
            {
                var result = _adminModuleManager.DeleteTeam(team);

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }

        }


        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult CreateUser(UserDto userDto)
        {
            try
            {
                var newUserId = _adminModuleManager.CreateUser(userDto);

                return new JsonNetResult
                    {
                        Data = newUserId
                    };
            }
            catch (ApplicationException ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(409, ex.Message);
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult EditUser(UserDto userDto)
        {
            try
            {
                var updatedUser = _adminModuleManager.EditUser(userDto);

                return new JsonNetResult
                    {
                        Data = updatedUser
                    };
            }
            catch (ApplicationException ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(400, "Bad Request: " + ex.ToString());
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpDelete]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult DeleteUser(User user)
        {
            try
            {
                var result = _adminModuleManager.DeleteUser(user);

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }

        }

        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult CreateLink(Link link)
        {
            if (!ModelState.IsValid)
                throw new HttpException(406, "Not Acceptable - Invalid Data");

            try
            {
                var newLink = _adminModuleManager.CreateLink(link);

                return new JsonNetResult
                {
                    Data = newLink
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult EditLink(Link link)
        {
            if (!ModelState.IsValid)
                throw new HttpException(406, "Not Acceptable - Invalid Data");

            try
            {
                var updatedLink = _adminModuleManager.EditLink(link);

                return new JsonNetResult
                {
                    Data = updatedLink
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpDelete]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult DeleteLink(Link link)
        {
            try
            {
                var result = _adminModuleManager.DeleteLink(link);

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetSelectedUserByName(string userName)
        {
            try
            {
                var result = _adminModuleManager.GetSelectedUserByName(userName);

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetUserPersonalSettings()
        {
            try
            {
                var result = _adminModuleManager.GetUserPersonalSettings();

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult SearchForUserByName(string userName)
        {
            try
            {
                var result = _adminModuleManager.SearchForUserByName(userName);

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetLinks()
        {
            try
            {
                var linksList = _adminModuleManager.GetLinks();

                return new JsonNetResult
                {
                    Data = linksList
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetLinksForTeam(int? teamId)
        {
            try
            {
                var linksList = _adminModuleManager.GetLinksForTeam(teamId);

                return new JsonNetResult
                {
                    Data = linksList
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult SaveLinksForTeam(TeamLinksDto teamLinksDto)
        {
            try
            {
                var result = _adminModuleManager.SaveLinksForTeam(teamLinksDto);

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetRequiredDataCreateUser()
        {
            try
            {
                var result = _adminModuleManager.GetRequiredDataCreateUser();

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetRequiredDataCreateTeam()
        {
            try
            {
                var result = _adminModuleManager.GetRequiredDataCreateTeam();

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetUserTeamLinks()
        {
            try
            {
                var result = _adminModuleManager.GetUserTeamLinks();

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }
        #endregion

        #region QuoteTemplate
        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult CreateQuoteTemplate(QuoteTemplate quoteTemplate)
        {
            if (!ModelState.IsValid)
                throw new HttpException(406, "Not Acceptable - Invalid Data");

            try
            {
                var newQuoteTemplate = _adminModuleManager.CreateQuoteTemplate(quoteTemplate);

                return new JsonNetResult
                {
                    Data = newQuoteTemplate
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult EditQuoteTemplate(QuoteTemplate quoteTemplate)
        {
            if (!ModelState.IsValid)
                throw new HttpException(406, "Not Acceptable - Invalid Data");

            try
            {
                var updatedQuoteTemplate = _adminModuleManager.EditQuoteTemplate(quoteTemplate);

                return new JsonNetResult
                {
                    Data = updatedQuoteTemplate
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpDelete]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult DeleteQuoteTemplate(QuoteTemplate quoteSheetTemplate)
        {
            try
            {
                var result = _adminModuleManager.DeleteQuoteTemplate(quoteSheetTemplate);

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetQuoteTemplates()
        {
            try
            {
                var quoteTemplatesList = _adminModuleManager.GetQuoteTemplates();

                return new JsonNetResult
                {
                    Data = quoteTemplatesList
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetQuoteTemplatesForTeam(int? teamId)
        {
            try
            {
                var quoteTemplatesList = _adminModuleManager.GetQuoteTemplatesForTeam(teamId);

                return new JsonNetResult
                {
                    Data = quoteTemplatesList
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }



        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetUserQuoteTemplates()
        {
            //try
            //{
            //    var result = _adminModuleManager.GetUserTeamLinks();

            //    return new JsonNetResult
            //    {
            //        Data = result
            //    };
            //}
            //catch (Exception ex)
            //{
            //    _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
            //    throw new HttpException(500, "Server Error");
            //}
            throw new NotImplementedException();
        }

        public ActionResult ManageQuoteSheetTemplates()
        {
            return PartialView("_ManageQuoteTemplates");
        }


        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult SaveQuoteTemplatesForTeam(TeamQuoteTemplatesDto teamQuoteTemplatesDto)
        {
            try
            {
                var result = _adminModuleManager.SaveQuoteTemplatesForTeam(teamQuoteTemplatesDto);

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }
#endregion

        #region Accelerators
        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult CreateAccelerator(AppAccelerator accelerator)
        {
            if (!ModelState.IsValid)
                throw new HttpException(406, "Not Acceptable - Invalid Data");

            try
            {
                var newAccelerator = _adminModuleManager.CreateAccelerator(accelerator);

                return new JsonNetResult
                {
                    Data = newAccelerator
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult EditAccelerator(AppAccelerator accelerator)
        {
            if (!ModelState.IsValid)
                throw new HttpException(406, "Not Acceptable - Invalid Data");

            try
            {
                var updatedAccelerator = _adminModuleManager.EditAccelerator(accelerator);

                return new JsonNetResult
                {
                    Data = updatedAccelerator
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpDelete]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult DeleteAccelerator(AppAccelerator accelerator)
        {
            try
            {
                var result = _adminModuleManager.DeleteAccelerator(accelerator);

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }


        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetAccelerators()
        {
            try
            {
                var appAcceleratorsList = _adminModuleManager.GetAccelerators();

                return new JsonNetResult
                {
                    Data = appAcceleratorsList
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetAcceleratorsForTeam(int? teamId)
        {
            try
            {
                var acceleratorsList = _adminModuleManager.GetAcceleratorsForTeam(teamId);

                return new JsonNetResult
                {
                    Data = acceleratorsList
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult SaveAcceleratorsForTeam(TeamAppAcceleratorsDto teamAcceleratorsDto)
        {
            try
            {
                var result = _adminModuleManager.SaveAcceleratorsForTeam(teamAcceleratorsDto);

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetUserTeamAccelerators()
        {
            try
            {
                var result = _adminModuleManager.GetUserTeamAccelerators();

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }


        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        [Authorize(Roles = @"ConsoleUW, ConsoleRead")]
        public ActionResult AcceleratorIndex()
        {
            var result = _adminModuleManager.GetUserTeamAccelerators();
            return View(result);
        }


        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        [Authorize(Roles = @"ConsoleUW, ConsoleRead")]
        public ActionResult GetAcceleratorMetaDataById(string Id)
        {
            var result = _adminModuleManager.GetAcceleratorMetaDataById(Id);
            this.HttpContext.Response.Clear();
            return this.Content(result, "text/xml");
        }
        #endregion

        #region Market Wording
        //GetMarketWordings
        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult CreateMarketWording(MarketWording marketWording)
        {
            if (!ModelState.IsValid)
                throw new HttpException(406, "Not Acceptable - Invalid Data");

            try
            {
                var newMarketWording = _adminModuleManager.CreateMarketWording(marketWording);

                return new JsonNetResult
                {
                    Data = newMarketWording
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult EditMarketWording(MarketWording marketWording)
        {
            if (!ModelState.IsValid)
                throw new HttpException(406, "Not Acceptable - Invalid Data");

            try
            {
                var updatedMarketWording = _adminModuleManager.EditMarketWording(marketWording);

                return new JsonNetResult
                {
                    Data = updatedMarketWording
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpDelete]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult DeleteMarketWording(MarketWording marketWording)
        {
            try
            {
                var result = _adminModuleManager.DeleteMarketWording(marketWording);

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }


        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetMarketWordings()
        {
            try
            {
                var appMarketWordingsList = _adminModuleManager.GetMarketWordings();

                return new JsonNetResult
                {
                    Data = appMarketWordingsList
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }


        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetMarketWordingsByPaging(String sSearch, Int32 sEcho, Int32 iSortCol_0, String sSortDir_0,
                                                      Int32 iDisplayLength = 10, Int32 iDisplayStart = 0)
        {
            try
            {
                Int32 iTotalRecords;
                Int32 iTotalDisplayRecords;
                String sortCol;

                switch (iSortCol_0)
                {
                    case 0:
                        sortCol = "WordingRefNumber";
                        break;
                    case 1:
                        sortCol = "Title";
                        break;
                    default:
                        sortCol = "WordingRefNumber";
                        break;
                }
                var aaData = this._adminModuleManager.GetMarketWordingsByPaging(sSearch, sortCol, sSortDir_0,
                                                                                iDisplayStart, iDisplayLength,
                                                                                out iTotalDisplayRecords,
                                                                                out iTotalRecords);
                return Json(new { sEcho, iTotalRecords, iTotalDisplayRecords, aaData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetMarketWordingsForTeamOffice(int? teamId, string officeId)
        {
            try
            {
                var marketWordingsList = _adminModuleManager.GetMarketWordingsForTeamOffice(teamId, officeId);

                return new JsonNetResult
                {
                    Data = marketWordingsList
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult SaveMarketWordingsForTeamOffice(TeamMarketWordingsDto teamMarketWordingsDto)
        {
            try
            {
                var result = _adminModuleManager.SaveMarketWordingsForTeamOffice(teamMarketWordingsDto);

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        #endregion

        #region SubjectToClause Wording
        //GetSubjectToClauseWordings
        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult CreateSubjectToClauseWording(SubjectToClauseWording subjectToClauseWording)
        {
            if (!ModelState.IsValid)
                throw new HttpException(406, "Not Acceptable - Invalid Data");

            try
            {
                var newSubjectToClauseWording = _adminModuleManager.CreateSubjectToClauseWording(subjectToClauseWording);

                return new JsonNetResult
                {
                    Data = newSubjectToClauseWording
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult EditSubjectToClauseWording(SubjectToClauseWording subjectToClauseWording)
        {
            if (!ModelState.IsValid)
                throw new HttpException(406, "Not Acceptable - Invalid Data");

            try
            {
                var updatedSubjectToClauseWording = _adminModuleManager.EditSubjectToClauseWording(subjectToClauseWording);

                return new JsonNetResult
                {
                    Data = updatedSubjectToClauseWording
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpDelete]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult DeleteSubjectToClauseWording(SubjectToClauseWording subjectToClauseWording)
        {
            try
            {
                var result = _adminModuleManager.DeleteSubjectToClauseWording(subjectToClauseWording);

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }


        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetSubjectToClauseWordings()
        {
            try
            {
                var appSubjectToClauseWordingsList = _adminModuleManager.GetSubjectToClauseWordings();

                return new JsonNetResult
                {
                    Data = appSubjectToClauseWordingsList
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetSubjectToClauseWordingsForTeamOffice(int? teamId, string officeId)
        {
            try
            {
                var SubjectToClauseWordingSettingsList = _adminModuleManager.GetSubjectToClauseWordingsForTeamOffice(teamId, officeId);

                return new JsonNetResult
                {
                    Data = SubjectToClauseWordingSettingsList
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult SaveSubjectToClauseWordingsForTeamOffice(TeamSubjectToClauseWordingsDto teamSubjectToClauseWordingsDto)
        {
            try
            {
                var result = _adminModuleManager.SaveSubjectToClauseWordingsForTeamOffice(teamSubjectToClauseWordingsDto);

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        #endregion


        #region TermsNCondition Wording
        //GetTermsNConditionWordings
        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult CreateTermsNConditionWording(TermsNConditionWording termsNConditionWording)
        {
            if (!ModelState.IsValid)
                throw new HttpException(406, "Not Acceptable - Invalid Data");

            try
            {
                var newTermsNConditionWording = _adminModuleManager.CreateTermsNConditionWording(termsNConditionWording);

                return new JsonNetResult
                {
                    Data = newTermsNConditionWording
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult EditTermsNConditionWording(TermsNConditionWording termsNConditionWording)
        {
            if (!ModelState.IsValid)
                throw new HttpException(406, "Not Acceptable - Invalid Data");

            try
            {
                var updatedTermsNConditionWording = _adminModuleManager.EditTermsNConditionWording(termsNConditionWording);

                return new JsonNetResult
                {
                    Data = updatedTermsNConditionWording
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpDelete]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult DeleteTermsNConditionWording(TermsNConditionWording termsNConditionWording)
        {
            try
            {
                var result = _adminModuleManager.DeleteTermsNConditionWording(termsNConditionWording);

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }


        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetTermsNConditionWordings()
        {
            try
            {
                var appTermsNConditionWordingsList = _adminModuleManager.GetTermsNConditionWordings();

                return new JsonNetResult
                {
                    Data = appTermsNConditionWordingsList
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetTermsNConditionWordingsByPaging(String sSearch, Int32 sEcho, Int32 iSortCol_0, String sSortDir_0,
                                                      Int32 iDisplayLength = 10, Int32 iDisplayStart = 0)
        {
            try
            {
                Int32 iTotalRecords;
                Int32 iTotalDisplayRecords;
                String sortCol;

                switch (iSortCol_0)
                {
                    case 0:
                        sortCol = "WordingRefNumber";
                        break;
                    case 1:
                        sortCol = "Title";
                        break;
                    default:
                        sortCol = "WordingRefNumber";
                        break;
                }
                var aaData = this._adminModuleManager.GetTermsNConditionWordingsByPaging(sSearch, sortCol, sSortDir_0,
                                                                                iDisplayStart, iDisplayLength,
                                                                                out iTotalDisplayRecords,
                                                                                out iTotalRecords);
                return Json(new { sEcho, iTotalRecords, iTotalDisplayRecords, aaData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetTermsNConditionWordingsForTeamOffice(int? teamId, string officeId)
        {
            try
            {
                var TermsNConditionWordingsList = _adminModuleManager.GetTermsNConditionWordingsForTeamOffice(teamId, officeId);

                return new JsonNetResult
                {
                    Data = TermsNConditionWordingsList
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult SaveTermsNConditionWordingsForTeamOffice(TeamTermsNConditionWordingsDto teamTermsNConditionWordingsDto)
        {
            try
            {
                var result = _adminModuleManager.SaveTermsNConditionWordingsForTeamOffice(teamTermsNConditionWordingsDto);

                return new JsonNetResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);
                throw new HttpException(500, "Server Error");
            }
        }

        #endregion

    }
}
