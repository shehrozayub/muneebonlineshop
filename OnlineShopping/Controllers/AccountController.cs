using OnlineShopping.DAL;
using OnlineShopping.Filters;
using OnlineShopping.Models;
using OnlineShopping.Repository;
using OnlineShopping.Utility;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopping.Controllers
{
    [FrontPageActionFilter]
    public class AccountController : Controller
    {
        #region Other class references...
        // Instance on Unit of Work
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        #endregion

        #region Member Login ...         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _unitOfWork.GetRepositoryInstance<Tbl_Members>().GetFirstOrDefaultByParameter(i => i.EmailId == model.UserEmailId && i.Password == model.Password && i.IsDelete == false);
                if (user != null && user.IsActive == true)
                {
                    Session["MemberId"] = user.MemberId;
                    Response.Cookies["MemberName"].Value = user.FirstName;
                    var roles = _unitOfWork.GetRepositoryInstance<Tbl_MemberRole>().GetFirstOrDefaultByParameter(i => i.MemberId == user.MemberId);
                    if (roles != null && roles.RoleId != 1)
                    {
                        Response.Cookies["MemberRole"].Value = _unitOfWork.GetRepositoryInstance<Tbl_Roles>().GetFirstOrDefaultByParameter(i => i.RoleId == roles.RoleId).RoleName;
                    }
                    else
                    {
                        Response.Cookies["MemberRole"].Value = "Admin";
                        returnUrl = "/Admin/";
                    }
                    if (model.RememberMe)
                    {
                        Response.Cookies["RememberMe_UserEmailId"].Value = model.UserEmailId; Response.Cookies["RememberMe_Password"].Value = model.Password;
                    }
                    else
                    {
                        Response.Cookies["RememberMe_UserEmailId"].Expires = DateTime.Now.AddDays(-1); Response.Cookies["RememberMe_Password"].Expires = DateTime.Now.AddDays(-1);
                    }
                    ViewBag.redirectUrl = (!string.IsNullOrEmpty(returnUrl) ? HttpUtility.HtmlDecode(returnUrl) : "/");
                }
                else
                {
                    if (user != null && user.IsActive == false) ModelState.AddModelError("Password", "Your account in not verified");
                    else ModelState.AddModelError("Password", "Invalid username or password");
                }
            }
            return PartialView("_Login", model);
        }
        #endregion


        #region Member Registration ...         
        [AllowAnonymous]
        public ActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            model.UserType = 2;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Register(RegisterViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {                 // Adding Member                 
                Tbl_Members mem = new Tbl_Members();
                mem.FirstName = model.FirstName;
                mem.LastName = model.LastName;
                mem.EmailId = model.UserEmailId;
                mem.CreatedOn = DateTime.Now;
                mem.ModifiedOn = DateTime.Now;
                mem.Password = model.Password;
                mem.IsActive = true;
                mem.IsDelete = false;
                _unitOfWork.GetRepositoryInstance<Tbl_Members>().Add(mem);
                // Adding Member Role                 
                Tbl_MemberRole mem_Role = new Tbl_MemberRole();
                mem_Role.MemberId = mem.MemberId;
                mem_Role.RoleId = 2;
                _unitOfWork.GetRepositoryInstance<Tbl_MemberRole>().Add(mem_Role);

                TempData["VerificationLinlMsg"] = "You are registered successfully.";
                Session["MemberId"] = mem.MemberId;
                Response.Cookies["MemberName"].Value = mem.FirstName;
                Response.Cookies["MemberRole"].Value = "User";
                return RedirectToAction("Index", "Home");
            }
            return View("Register", model);
        }


        public JsonResult CheckEmailExist(string UserEmailId)
        {
            int LoginMemberId = Convert.ToInt32(Session["MemberId"]);
            var EmailExist = _unitOfWork.GetRepositoryInstance<Tbl_Members>().GetFirstOrDefaultByParameter(i => i.MemberId != LoginMemberId && i.EmailId == UserEmailId && i.IsDelete == false);
            return EmailExist == null ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Logout ...
        public ActionResult LogOut()
        {
            Session["MemberId"] = null;
            if (Request.Cookies["MemberRole"] != null)
                Response.Cookies["MemberRole"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index", "Home");
        }
        #endregion



        #region Unauthorize View to Page...
        public ActionResult UnauthorizeViewToPage()
        {
            return Redirect("/");
        }
        #endregion
    }
}